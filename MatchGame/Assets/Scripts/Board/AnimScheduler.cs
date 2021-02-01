using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Summoner.MatchGame {
	public class AnimScheduler : MonoBehaviour {
		[SerializeField] float gravity = -40.0f;
		[SerializeField] float bounce = 0.2f;
		[SerializeField] float threshold = -10f;

		public int isPlaying { get; private set; }

		public Coroutine Drop( Cell cell ) {
			return StartCoroutine( FreeFall( cell.transform, cell.block.transform ) );
		}
		
		IEnumerator FreeFall( Transform cell, Transform block ) {
			var start = block.position;
			var end = cell.position;
			Debug.DrawLine( start, end, Color.white, 1 );
			var v = 0.0f;
			++isPlaying;
			do {
				while ( block.position.y > end.y ) {
					v += gravity * Time.deltaTime;
					var p = block.position;
					p.y += v * Time.deltaTime;
					var t = (p.y - end.y) / (start.y - end.y);
					p.x = Mathf.Lerp( end.x, start.x, t );
					block.position = p;
					yield return null;
				}

				if ( v < threshold ) {
					v = -v * bounce;
					block.position += 2 * (end - block.position);
				}
				else {
					break;
				}
			} while ( true );

			block.position = end;
			--isPlaying;
		}

		private IEnumerator Lerp( Transform cell, Transform block ) {
			++isPlaying;
			var start = block.position;
			var end = cell.position;

			foreach ( var t in Summoner.Lerp.NormalizedDuration( 0.5f ) ) {
				block.position = Vector3.Lerp( start, end, t );
				yield return null;
			}
			--isPlaying;
		}
	}
}
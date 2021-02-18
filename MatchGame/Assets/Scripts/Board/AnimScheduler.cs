using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Summoner.MatchGame {
	public interface IAnim {
		Coroutine Play( System.Action onFinished );
	}

	public class AnimScheduler : MonoBehaviour {
		private static readonly YieldInstruction waitTick = new WaitForSeconds( 0.15f );
		public int isPlaying { get; private set; }
		private IList<IList<IAnim>> timeline = null;

		private void Awake() {
			timeline = new List<IList<IAnim>>();
			MoveTimeline();
		}

		public void MoveTimeline() {
			timeline.Add( new List<IAnim>() );
		}

		public void Drop( Cell cell ) {
			var anim = cell.block.GetComponent<FallAnim>();
			if ( anim == null ) { 
				anim = cell.block.gameObject.AddComponent<FallAnim>();
				timeline.Last().Add( anim );
			}

			anim.Add( cell.transform.position );
		}

		public Coroutine Play() {
			return StartCoroutine( PlayAux() );
		}

		private IEnumerator PlayAux() {
			Play( timeline[0] );
			for ( var i = 1; i < timeline.Count; ++i ) {
				yield return waitTick;
				Play( timeline[i] );
			}

			timeline.Clear();
			MoveTimeline();

			yield return new WaitWhile( () => isPlaying > 0 );
		}

		private void Play( IList<IAnim> anims ) {
			foreach ( var anim in anims ) {
				++isPlaying;
				anim.Play( () => --isPlaying );
			}
		}
	}
}
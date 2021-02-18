using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Summoner.MatchGame {
	public class FallAnim : MonoBehaviour, IAnim {
		private static readonly float gravity = 20.0f;
		private static readonly float bounce = 0.15f;
		private static readonly float threshold = 15f;

		private IList<Vector3> destinations = new List<Vector3>();
		private float v = 3.0f;

		public void Add( Vector3 destination ) {
			destinations.Add( destination );
		}

		public Coroutine Play( System.Action onFinished ) {
			return StartCoroutine( PlayAux( onFinished ) );
		}

		private IEnumerator PlayAux( System.Action onFinished ) {
			foreach ( var destination in destinations ) {
				yield return StartCoroutine( FreeFall( destination ) );
			}

			if ( v > threshold ) { 
				yield return StartCoroutine( Bounce() );
			}

			destinations.Clear();
			Destroy( this );
			onFinished();
		}

		private IEnumerator FreeFall( Vector3 end ) {
			var start = transform.position;
			var displacement = end - start;
			var distance = displacement.magnitude;
			var p = 0.0f;
			while ( p < distance ) {
				v += gravity * Time.deltaTime;
				p += v * Time.deltaTime;

				transform.position = Vector3.Lerp( start, end, p / distance );
				yield return null;
			}

			transform.position = end;
		}

		private IEnumerator Bounce() {
			var end = transform.position;
			var p = end;
			v = v * -bounce;
			do {
				p.y -= v * Time.deltaTime;
				transform.position = p;
				yield return null;
				v += gravity * Time.deltaTime;
			} while ( p.y > end.y );

			transform.position = end;
		}
	}
}
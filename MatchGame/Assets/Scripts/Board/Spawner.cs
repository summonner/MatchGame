using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Summoner.MatchGame {
	public sealed class Spawner : MonoBehaviour {
		[SerializeField]
		private List<Block> color;

		//[System.Serializable]
		//public struct Block {
		//	public byte color;
		//	public int type;
		//}


//#if UNITY_EDITOR
//		void OnDrawGizmos() {
//			var l = 0.3f;
//			var p = transform.position + new Vector3( 0, -l, 0 );
//			UnityEditor.Handles.color = Color.green;
//			UnityEditor.Handles.DrawLine( p, p + new Vector3( 0, 2 * l, 0 ) );
//			UnityEditor.Handles.DrawLine( p, p + new Vector3( l, l, 0 ) );
//			UnityEditor.Handles.DrawLine( p, p + new Vector3( -l, l, 0 ) );
//		}
//#endif
	}
}
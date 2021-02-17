using UnityEngine;
using System.Collections.Generic;

namespace Summoner.MatchGame {

	public interface ICell {
		IBlock block { get; }
		bool isSpawner { get; }
	}

	[SelectionBase]
	public sealed class Cell : MonoBehaviour, ICell {
		public CubeCoordinate coord { get; private set; }
		public Block block = null;
		[HideInInspector]public Spawner spawner = null;
		public bool isSpawner => spawner != null;

		IBlock ICell.block {
			get {
				return block;
			}
		}

#if UNITY_EDITOR
		void OnDrawGizmos() {
			if ( isSpawner == false ) {
				return;
			}

			UnityEditor.Handles.matrix = Matrix4x4.Translate( transform.position );
			DrawArrow( Color.green );
		}

		public void DrawArrow( Color color ) {
			var l = 0.3f;
			var p = new Vector3( 0, -l, 0 );
			UnityEditor.Handles.color = color;
			UnityEditor.Handles.DrawLine( p, p + new Vector3( 0, 2 * l, 0 ) );
			UnityEditor.Handles.DrawLine( p, p + new Vector3( l, l, 0 ) );
			UnityEditor.Handles.DrawLine( p, p + new Vector3( -l, l, 0 ) );
		}
#endif
	}
}
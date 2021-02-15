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

			var l = 0.3f;
			var p = transform.position + new Vector3( 0, -l, 0 );
			UnityEditor.Handles.color = Color.green;
			UnityEditor.Handles.DrawLine( p, p + new Vector3( 0, 2 * l, 0 ) );
			UnityEditor.Handles.DrawLine( p, p + new Vector3( l, l, 0 ) );
			UnityEditor.Handles.DrawLine( p, p + new Vector3( -l, l, 0 ) );
		}
#endif
	}
}
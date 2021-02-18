using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Summoner.MatchGame {
	public sealed class Spawner : MonoBehaviour {
		private SpawnTable.Drawer colors;
		private SpawnTable.Drawer types;

		public void Init( SpawnTable.Drawer colors, SpawnTable.Drawer types ) {
			this.colors = colors;
			this.types = types;
		}

		public IBlock Draw() {
			var color = colors.Draw();
			var type = types.Draw();
			return new Archetype( color, type );
		}

		private class Archetype : IBlock { 
			public byte color { get; private set; }
			public byte type { get; private set; }

			public Archetype( byte color, byte type ) {
				this.color = color;
				this.type = type;
			}
		}

#if UNITY_EDITOR
		void OnDrawGizmos() {
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
using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Collections.Generic;

namespace Summoner.MatchGame {
	public static class TileCursor {
		private static readonly Vector3[] hexPoints = HexPoints( 0.5f ).ToArray();
		private static IEnumerable<Vector3> HexPoints( float radius ) {
			var start = 30f;
			for ( var i = 0; i < 6; ++i ) {
				var angle = (i * 60f + start) * Mathf.Deg2Rad;
				var x = Mathf.Sin( angle );
				var y = Mathf.Cos( angle );
				var z = 0f;
				yield return new Vector3( x, y, z ) * radius;
			}
		}

		public static void Draw( Vector3 position ) {
			Handles.matrix = Matrix4x4.Translate( position );
			Handles.color = new Color( 1f, 0.5f, 0f );

			var thickness = 3.0f;
			for ( var i = 1; i < 6; ++i ) {
				Handles.DrawLine( hexPoints[i - 1], hexPoints[i], thickness );
			}
			Handles.DrawLine( hexPoints[5], hexPoints[0], thickness );
		}
	}
}
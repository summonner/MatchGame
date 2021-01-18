using UnityEngine;
using System.Collections.Generic;

namespace Summoner.MatchGame {
	public class CoordConverter : MonoBehaviour {
		public Vector2 spacing { get; private set; }
		public float cellRadius { get; private set; }
		public float gap { get; private set; }

		public void Init( float cellRadius, float gap ){
			this.cellRadius = cellRadius;
			this.gap = gap;
			spacing = new Vector2(
				cellRadius * 1.5f + gap,
				cellRadius * Mathf.Sqrt( 3 ) + gap
			);
		}

		public Vector3 Hex2Board( CubeCoordinate hex ) {
			Vector3 position;
			position.y = spacing.y * (hex.z + 0.5f * hex.x);
			position.x = spacing.x * hex.x;
			position.z = 0;
			return position;
		}

		public Vector3 Hex2World( CubeCoordinate hex ) {
			var local = Hex2Board( hex );
			return transform.TransformPoint( local );
		}

		public CubeCoordinate Board2Hex( Vector3 board ) {
			var x = board.x / spacing.x;
			var z = board.y / spacing.y - 0.5f * x;
			return CubeCoordinate.CubeRound( x, z );
		}

		public CubeCoordinate World2Hex( Vector3 world ) {
			var local = transform.InverseTransformPoint( world );
			return Board2Hex( local );
		}
	}
}
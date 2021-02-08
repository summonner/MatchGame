using UnityEngine;
using System.Collections.Generic;
using System.Collections;

namespace Summoner.MatchGame {
	public struct Column {
		public readonly CubeCoordinate bottom;
		public readonly CubeCoordinate up;
		public readonly CubeCoordinate top;
		public readonly bool hasSpawner;

		public Column( CubeCoordinate bottom, CubeCoordinate top, bool hasSpawner ) {
			Debug.Assert( bottom.q == top.q && bottom.r < top.r );
			this.bottom = bottom;
			this.up = FlatTopDirection.N;
			this.top = top;
			this.hasSpawner = hasSpawner;
		}

		public IEnumerable<CubeCoordinate> BottomToTop() {
			for ( var coord = bottom; Less( coord, top ); coord += up ) {
				yield return coord;
			}
			yield return top;
		}

		public IEnumerable<CubeCoordinate> TopToBottom() {
			for ( var coord = top; Less( bottom, coord ); coord -= up ) {
				yield return coord;
			}
			yield return bottom;
		}

		private static bool Less( CubeCoordinate a, CubeCoordinate b ) {
			return a.r < b.r;
		}
	}
}
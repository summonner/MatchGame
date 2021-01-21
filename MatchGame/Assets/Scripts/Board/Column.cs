using UnityEngine;
using System.Collections.Generic;
using System.Collections;

namespace Summoner.MatchGame {
	public struct Column {
		public readonly CubeCoordinate bottom;
		public readonly CubeCoordinate up;
		public readonly CubeCoordinate top;
		public readonly bool hasSpawner;

		public Column( CubeCoordinate bottom, CubeCoordinate up, CubeCoordinate top, bool hasSpawner ) {
			this.bottom = bottom;
			this.up = up;
			this.top = top;
			this.hasSpawner = hasSpawner;
		}

		public IEnumerable<CubeCoordinate> BottomToTop() {
			for ( var coord = bottom; coord != top; coord += up ) {
				yield return coord;
			}
			yield return top;
		}

		public IEnumerable<CubeCoordinate> TopToBottom() {
			for ( var coord = top; coord != bottom; coord -= up ) {
				yield return coord;
			}
			yield return bottom;
		}
	}
}
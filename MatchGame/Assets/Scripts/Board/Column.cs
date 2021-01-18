using UnityEngine;
using System.Collections.Generic;
using System.Collections;

namespace Summoner.MatchGame {
	public struct Column : IEnumerable<CubeCoordinate> {
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

		public IEnumerator<CubeCoordinate> GetEnumerator() {
			for ( var coord = bottom; coord != top; coord += up ) {
				yield return coord;
			}
			yield return top;
		}

		IEnumerator IEnumerable.GetEnumerator() {
			return GetEnumerator();
		}
	}
}
using UnityEngine;
using System.Collections.Generic;

namespace Summoner.MatchGame {
	public sealed class BottomLeftToTopRight : IComparer<CubeCoordinate> {
		public static readonly BottomLeftToTopRight Instance = new BottomLeftToTopRight();
		public int Compare( CubeCoordinate left, CubeCoordinate right ) {
			var heightDiff = Height( left ) - Height( right );
			if ( heightDiff != 0 ) {
				return heightDiff;
			}

			return left.x - right.x;
		}

		private int Height( CubeCoordinate coord ) {
			return coord.z - coord.y;
		}
	}
}
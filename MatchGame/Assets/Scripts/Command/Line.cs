using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Summoner.MatchGame {
	public class Line : ICommand {

		private readonly CubeCoordinate selected;
		private readonly CubeCoordinate direction;
		public Line( CubeCoordinate selected, CubeCoordinate direction ) {
			this.selected = selected;
			this.direction = direction;
		}

		public void Apply( IBoard board ) {
			board.Destroy( Area( board ) );
			
		}

		private IEnumerable<CubeCoordinate> Area( IBoard board ) {
			yield return selected;
			for ( var coord = selected + direction; board[coord] != null; coord += direction ) {
				yield return coord;
			}

			for ( var coord = selected - direction; board[coord] != null; coord -= direction ) {
				yield return coord;
			}
		}

		public void Undo( IBoard board ) {
			return;
		}
	}
}
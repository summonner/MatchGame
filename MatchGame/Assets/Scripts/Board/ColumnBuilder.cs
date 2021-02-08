using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Summoner.MatchGame {
	public static class ColumnBuilder {
		public static IEnumerable<Column> Build( IBoard board ) {
			var groups = from cell in board.cells
						 group cell.Key by cell.Key.q into g
						 select (from coord in g
								 orderby coord.r 
								 select coord);

			foreach ( var group in groups ) {
				var hasSpawner = HasSpawner( board, group );
				yield return new Column( group.First(), group.Last(), hasSpawner );
			}
		}

		private static bool HasSpawner( IBoard board, IEnumerable<CubeCoordinate> column ) {
			foreach ( var coord in column ) {
				if ( board[coord].isSpawner ) {
					return true;
				}
			}
			return false;
		}
	}
}
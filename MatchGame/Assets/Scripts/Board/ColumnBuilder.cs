using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Summoner.MatchGame {
	public static class ColumnBuilder {
		public static IEnumerable<Column> Build( IBoard board ) {
			var groups = from cell in board.cells
						 group cell.Key by cell.Key.q into g
						 orderby g.Key
						 select (from coord in g
								 orderby coord.r 
								 select coord);

			foreach ( var group in groups ) {
				var up = FlatTopDirection.N;
				var bottom = group.First();
				var top = group.Last() + up;
				for ( var coord = bottom; coord.r < top.r; coord += up ) {
					var cell = board[coord];
					if ( cell == null ) {
						if ( bottom != coord ) {
							yield return new Column( bottom, coord - up, false );
						}
						bottom = coord + up;
					}
					else if ( cell.isSpawner ) {
						yield return new Column( bottom, coord, true );
						bottom = coord + up;
					}
				}

				top -= up;
				if ( bottom.r < top.r ) {
					yield return new Column( bottom, top, false );
				}
	//			var hasSpawner = HasSpawner( board, group );
	//			yield return new Column( group.First(), group.Last(), hasSpawner );
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
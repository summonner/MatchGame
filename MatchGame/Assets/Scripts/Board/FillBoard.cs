using UnityEngine;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Summoner.MatchGame {
	public class FillBoard {

		public async Task Do( IBoard board ) {
			foreach ( var column in board.columns ) {
				var numEmpties = FillEmpties( board, column );

				if ( column.hasSpawner == true ) {
					SpawnBlocks( board, column, numEmpties );
				}
			}

			await board.WaitAnim();

			var slipped = false;
			do {
				slipped = SlipBlocks( board );

				if ( slipped == true ) {
					SpawnBlocks( board );
				}

				await board.WaitAnim();
			} while ( slipped );
		}

		private int FillEmpties( IBoard board, Column column ) {
			var numEmpties = 0;
			foreach ( var coord in column.BottomToTop() ) {
				var cell = board[coord];
				Debug.Assert( cell != null );
				if ( cell.block == null ) {
					numEmpties += 1;
				}
				else if ( numEmpties > 0 ) {
					var dropTo = coord - column.up * numEmpties;
					Debug.Assert( board[dropTo] != null && board[dropTo].block == null, dropTo.ToString() );
					board.Drop( coord, dropTo );
				}
			}

			return numEmpties;
		}

		private void SpawnBlocks( IBoard board, Column column, int numEmpties ) {
			var coord = column.top + column.up;
			var offset = column.up * numEmpties;
			for ( var i = 0; i < numEmpties; ++i ) {
				var block = board.Spawn( coord - offset, offset );
				coord += column.up;
			}
		}

		private IEnumerable<CubeCoordinate> pullDirections {
			get {
				yield return FlatTopDirection.N;
				if ( Random.value > 0.5f ) {
					yield return FlatTopDirection.NE;
					yield return FlatTopDirection.NW;
				}
				else {
					yield return FlatTopDirection.NW;
					yield return FlatTopDirection.NE;
				}
			}
		}

		private bool SlipBlocks( IBoard board ) {
			var moved = false;
			foreach ( var cell in board.cells ) {
				if ( cell.Value.block != null ) {
					continue;
				}

				foreach ( var pull in pullDirections ) {
					var up = cell.Key + pull;
					if ( board[up]?.block != null ) {
						board.Drop( up, cell.Key );
						moved = true;
						break;
					}
				}
			}

			return moved;
		}

		private void SpawnBlocks( IBoard board ) {
			foreach ( var column in board.columns ) {
				var needSpawn = board[column.top].block == null
							 && column.hasSpawner == true;
				if ( needSpawn ) {
					board.Spawn( column.top, FlatTopDirection.N );
				}
			}
		}
	}
}
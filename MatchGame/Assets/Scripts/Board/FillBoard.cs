using UnityEngine;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Summoner.MatchGame {
	public class FillBoard {

		public async Task Do( IBoard board ) {
			var slipped = false;
			do {
				foreach ( var column in board.columns ) {
					var numEmpties = DropBlocks( board, column );

					if ( column.hasSpawner == true ) {
						SpawnBlocks( board, column, numEmpties );
					}
				}

				await board.WaitAnim();

				slipped = false;
				foreach ( var column in board.columns ) {
					slipped |= SlipBlock( board, column );
				}

				await board.WaitAnim();
			}
			while ( slipped );
		}

		private int DropBlocks( IBoard board, Column column ) {
			var numEmpties = 0;
			foreach ( var coord in column.BottomToTop() ) {
				var cell = board[coord];
				Debug.Assert( cell != null );
				if ( cell.block == null ) {
					numEmpties += 1;
				}
				else if ( numEmpties > 0 ) {
					var dropTo = coord - column.up * numEmpties;
					Debug.Assert( board[dropTo] != null );
					Debug.Assert( board[dropTo].block == null );
					board.Drop( coord, dropTo );
				}
			}

			return numEmpties;
		}

		private bool SlipBlock( IBoard board, Column column ) {
			foreach ( var coord in column.TopToBottom() ) {
				var cell = board[coord];
				if ( cell.block != null ) {
					return false;
				}

				foreach ( var side in new[] { FlatTopDirection.NE, FlatTopDirection.NW } ) {
					if ( board[coord + side]?.block == null ) {
						continue;
					}

					var cell2 = board[coord + side + FlatTopDirection.S];
					var isEmpty = cell2 != null && cell2.block == null;
					if ( isEmpty ) {
						continue;
					}

					board.Drop( coord + side, coord );
					return true;
				}
			}

			return false;
		}

		private void SpawnBlocks( IBoard board, Column column, int numEmpties ) {
			var coord = column.top + column.up;
			var offset = column.up * numEmpties;
			for ( var i = 0; i < numEmpties; ++i ) {
				var block = board.Spawn( coord - offset, offset );
				coord += column.up;
			}
		}
	}
}
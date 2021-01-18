using UnityEngine;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Summoner.MatchGame {
	public class FillBoard {

		public async Task Do( IBoard board ) {
			foreach ( var column in board.columns ) {
				var numEmpties = DropBlocks( board, column );

				if ( column.hasSpawner == true ) {
					SpawnBlocks( board, column, numEmpties );
				}
			}

			await board.WaitAnim();
		}

		private int DropBlocks( IBoard board, Column column ) {
			var numEmpties = 0;
			foreach ( var coord in column ) {
				var cell = board[coord];
				Debug.Assert( cell != null );
				if ( cell.block == null ) {
					numEmpties += 1;
				}
				else {
					var dropTo = coord - column.up * numEmpties;
					Debug.Assert( board[dropTo] != null && board[dropTo].block == null );
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
	}
}
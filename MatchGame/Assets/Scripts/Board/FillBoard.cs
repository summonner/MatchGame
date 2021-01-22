using UnityEngine;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Summoner.MatchGame {
	public class FillBoard {

		public async Task Do( IBoard board ) {
			var moved = false;
			do {
				moved = false;
				moved |= DropBlocks( board );
				moved |= SpawnBlocks( board );

				await board.WaitAnim();
			} while ( moved );
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

		private bool DropBlocks( IBoard board ) {
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

		private bool SpawnBlocks( IBoard board ) {
			var moved = false;
			foreach ( var column in board.columns ) {
				var needSpawn = board[column.top].block == null
							 && column.hasSpawner == true;
				if ( needSpawn ) {
					board.Spawn( column.top, FlatTopDirection.N );
					moved = true;
				}
			}

			return moved;
		}
	}
}
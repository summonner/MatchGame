using UnityEngine;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace Summoner.MatchGame {
	public class FillBoard {
		private readonly IBoard board;
		private readonly IList<Column> columns;

		public FillBoard( IBoard board ) {
			this.board = board;
			this.columns = ColumnBuilder.Build( board ).ToArray();
		}

		public async Task Do() {
			foreach ( var column in columns ) {
				var numEmpties = FillEmpties( board, column );

				if ( column.hasSpawner == true ) {
					SpawnBlocks( board, column, numEmpties );
				}
			}
			await board.WaitAnim();

			while( FindSlipMoves( board ) ) {
				ApplyMoves( board );
				SpawnBlocks( board );
				await board.WaitAnim();
			};
		}

		private int FillEmpties( IBoard board, Column column ) {
			var numEmpties = 0;
			foreach ( var coord in column.BottomToTop() ) {
				var cell = board[coord];
				//Debug.Assert( cell != null );
				if ( cell == null ) {
					numEmpties = 0;
				}
				else if ( cell.block == null ) {
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
			board.Spawn( column.top, numEmpties );
		}

		private IEnumerable<CubeCoordinate> pullDirections {
			get {
				yield return FlatTopDirection.N;
				if ( UnityEngine.Random.value > 0.5f ) {
					yield return FlatTopDirection.NE;
					yield return FlatTopDirection.NW;
				}
				else {
					yield return FlatTopDirection.NW;
					yield return FlatTopDirection.NE;
				}
			}
		}

		private IDictionary<CubeCoordinate, CubeCoordinate> moves = new Dictionary<CubeCoordinate, CubeCoordinate>( 8 * 8 );
		private bool FindSlipMoves( IBoard board ) {
			moves.Clear();
			foreach ( var cell in TraverseEmptyCells( board ) ) {
				foreach ( var pull in pullDirections ) {
					var target = cell.Key + pull;
					if ( board[target]?.block == null ) {
						continue;
					}

					if ( moves.TryGetValue( target, out var move ) ) {
						var abandon = move == FlatTopDirection.S
								   || UnityEngine.Random.value > 0.5f;
						if ( abandon ) {
							continue;
						}
					}

					moves[target] = pull * -1;
					break;
				}
			}

			return moves.Count > 0;
		}

		private IEnumerable<KeyValuePair<CubeCoordinate, ICell>> TraverseEmptyCells( IBoard board ) {
			foreach ( var cell in board.cells ) {
				var hasBlock = cell.Value.block != null
							&& moves.ContainsKey( cell.Key ) == false;
				if ( hasBlock ) {
					continue;
				}

				yield return cell;
			}
		}

		private void ApplyMoves( IBoard board ) {
			foreach ( var cell in board.cells ) {
				if ( cell.Value.block == null ) {
					continue;
				}

				if ( moves.TryGetValue( cell.Key, out var move ) == false ) {
					continue;
				}

				board.Drop( cell.Key, cell.Key + move );
			}
		}

		private void SpawnBlocks( IBoard board ) {
			foreach ( var column in columns ) {
				var needSpawn = board[column.top].block == null
							 && column.hasSpawner == true;
				if ( needSpawn ) {
					board.Spawn( column.top, 1 );
				}
			}
		}
	}
}
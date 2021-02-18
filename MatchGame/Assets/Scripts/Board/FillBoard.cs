using UnityEngine;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace Summoner.MatchGame {
	public class FillBoard {
		private readonly IBoard board = null;
		private readonly IList<CubeCoordinate> spawners = null;
		private IDictionary<CubeCoordinate, CubeCoordinate> moves = null;


		public FillBoard( IBoard board ) {
			this.board = board;
			spawners = (from cell in board.cells
					   where cell.Value.isSpawner
					   select cell.Key)
					   .ToArray();
			moves = new Dictionary<CubeCoordinate, CubeCoordinate>( board.cells.Count() );
		}

		public async Task Do() {
			SpawnBlocks( board );	// TODO : 초기배치를 먼저하도록하고 제거.
			FindStraightMoves();
			board.MoveTimeline();

			while ( FindSlipMoves( board ) ) {
				ApplyMoves( board );
				SpawnBlocks( board );
				board.MoveTimeline();
			};

			await board.WaitAnim();
			Debug.Assert( moves.Count <= 0 );
			moves.Clear();
		}

		private void FindStraightMoves() {
			foreach ( var cell in board.cells ) {
				if ( cell.Value.block == null ) {
					continue;
				}

				var coord = cell.Key;
				while ( IsEmpty( coord + FlatTopDirection.S ) ) {
					moves[coord] = FlatTopDirection.S;
					coord += FlatTopDirection.S;
				}
			}
		}

		private bool IsEmpty( CubeCoordinate coord ) {
			var cell = board[coord];
			return cell != null
				&& cell.block == null;
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

		private bool FindSlipMoves( IBoard board ) {
			foreach ( var cell in TraverseEmptyCells( board ) ) {
				if ( moves.ContainsKey( cell.Key + FlatTopDirection.N ) ) {
					continue;
				}

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
				if ( hasBlock || cell.Value.isSpawner ) {
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
				moves.Remove( cell.Key );
			}
		}

		private void SpawnBlocks( IBoard board ) {
			foreach ( var spawner in spawners ) {
				if ( board[spawner].block == null ) {
					board.Spawn( spawner, 1 );
				}
			}
		}
	}
}
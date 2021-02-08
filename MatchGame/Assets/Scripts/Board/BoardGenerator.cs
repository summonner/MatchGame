using UnityEngine;
using System.Collections.Generic;
using Summoner.Util.Extension;

namespace Summoner.MatchGame {
	public class BoardGenerator : MonoBehaviour {
		[SerializeField] private Game game = null;
		[SerializeField] private GameObject template = null;
		[SerializeField] private float cellRadius = 0.5f;
		[SerializeField] private float gap = 0.02f;
		[SerializeField] private int size = 8;

		void Awake() {
			Debug.Assert( template != null );
			Debug.Assert( game != null );

			var container = CreateContainer();

			var converter = container.gameObject.AddComponent<CoordConverter>();
			converter.Init( cellRadius, gap );

			var cells = SpawnCells( converter, container );

			var bounds = CalculateBounds( cells );
			var inputReceiver = AddInputReceiver( container, bounds );
			inputReceiver.converter = converter;

			var board = gameObject.GetOrAddComponent<Board>();
			board.Init( cells, converter );

			Destroy( this );
			game.Init( board, inputReceiver );
		}

		private Transform CreateContainer() {
			var containerObj = new GameObject( "Cells" );
			containerObj.layer = gameObject.layer;

			var container = containerObj.transform;
			container.Reset( transform );
			return container;
		}

		private IDictionary<CubeCoordinate, Cell> SpawnCells( CoordConverter converter, Transform container ) {
			var cells = new SortedList<CubeCoordinate, Cell>( size * size, new BottomLeftToTopRight() );

			foreach ( var coord in TraversePoints() ) {
				var cellObj = Instantiate( template, container );
				var cell = cellObj.GetOrAddComponent<Cell>();
				cell.Init( coord, converter.Hex2Board( coord ), IsSpawner( coord ) );
				cells.Add( coord, cell );
			}

			var test = new CubeCoordinate( 2, 3 );
			Destroy( cells[test].gameObject );
			cells.Remove( test );
			return cells;
		}

		private IEnumerable<CubeCoordinate> TraversePoints() {
			for ( var x = 0; x < size; ++x ) {
				var offset = x / 2;
				for ( var y = 0 - offset; y < size - offset; ++y ) {
					yield return new CubeCoordinate( x, y );
				}
			}
		}

		private bool IsSpawner( CubeCoordinate coord ) {
			var list = new List<CubeCoordinate>( new[] {
				new CubeCoordinate( 2, 2 ),
				new CubeCoordinate( 2, 6 ),
				new CubeCoordinate( 3, 6 ),
			} );
			return list.Contains( coord );
		}

		private InputReceiver AddInputReceiver( Transform pivot, Bounds bounds ) {
			pivot.localPosition -= bounds.center;
			var pivotObj = pivot.gameObject;

			var collider = pivotObj.AddComponent<BoxCollider2D>();
			collider.size = bounds.size;
			collider.offset = bounds.center;

			return pivotObj.AddComponent<InputReceiver>();
		}

		private Bounds CalculateBounds( IDictionary<CubeCoordinate, Cell> cells ) {
			var bounds = new Bounds();
			
			foreach ( var cell in cells.Values ) {
				bounds.Encapsulate( cell.transform.localPosition );
			}

			bounds.Expand( cellRadius );
			return bounds;
		}

		public class BottomLeftToTopRight : IComparer<CubeCoordinate> {
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
}
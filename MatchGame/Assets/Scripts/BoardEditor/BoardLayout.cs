using UnityEngine;
using System.Collections.Generic;

namespace Summoner.MatchGame {
	[SelectionBase]
	public sealed class BoardLayout : MonoBehaviour {
		private static readonly float cellRadius = 0.5f;
		private static readonly float gap = 0.02f;

		public Board Generate() {
			return Instantiate( this ).Init();
		}

		private Board Init() {
			var board = CreateContainer();
			transform.Reset( board.transform );

			IDictionary<CubeCoordinate, Cell> tiles = new SortedList<CubeCoordinate, Cell>( transform.childCount, BottomLeftToTopRight.Instance );
			ReloadTiles( ref tiles );
			board.Init( tiles, converter );

			var bounds = CalculateBounds( tiles );
			transform.localPosition -= bounds.center;
			AddInputReceiver( bounds );

			Destroy( this );
			return board;
		}

		private Board CreateContainer() {
			var board = new GameObject( "board" );
			board.layer = gameObject.layer;
			board.transform.Reset();
			return board.AddComponent<Board>();
		}

		public void ReloadTiles( ref IDictionary<CubeCoordinate, Cell> tiles ) {
			if ( tiles == null ) {
				tiles = new Dictionary<CubeCoordinate, Cell>( transform.childCount );
			}

			tiles.Clear();
			List<Transform> toRemove = new List<Transform>();
			foreach ( Transform child in transform ) {
				var coord = converter.World2Hex( child.position );
				var cell = child.GetComponent<Cell>();
				var isValid = tiles.ContainsKey( coord ) == false
						   && cell != null;
				if ( isValid ) {
					tiles.Add( coord, cell );
				}
				else {
					toRemove.Add( child );
				}
			}

			System.Action<Object> destroyFunc = Application.isPlaying ? (System.Action<Object>)Destroy : (System.Action<Object>)DestroyImmediate;
			foreach ( var t in toRemove ) {
				destroyFunc( t.gameObject );
			}
		}

		private Bounds CalculateBounds( IDictionary<CubeCoordinate, Cell> cells ) {
			var bounds = new Bounds();
			bounds.min = new Vector3( float.MaxValue, float.MaxValue, float.MaxValue );
			bounds.max = new Vector3( float.MinValue, float.MinValue, float.MinValue );

			foreach ( var cell in cells.Values ) {
				bounds.Encapsulate( cell.transform.localPosition );
			}

			bounds.Expand( cellRadius * 2 );
			return bounds;
		}

		private InputReceiver AddInputReceiver( Bounds bounds ) {
			var collider = gameObject.AddComponent<BoxCollider2D>();
			collider.size = bounds.size;
			collider.offset = bounds.center;

			return gameObject.AddComponent<InputReceiver>();
		}



		private CoordConverter _converter;
		public CoordConverter converter {
			get {
				if ( _converter == null ) {
					_converter = gameObject.GetOrAddComponent<CoordConverter>();
					_converter.Init( cellRadius, gap );
				}

				return _converter;
			}
		}
	}
}
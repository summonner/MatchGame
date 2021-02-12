using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace Summoner.MatchGame {
	public sealed class BoardEditMode {
		private static readonly GameObject template = Resources.Load<GameObject>( "Board/Tile" );
		private static readonly int boardLayer = LayerMask.NameToLayer( "Board" );

		private readonly BoardLayout target = null;
		private System.Action<CubeCoordinate> action = null;
		private IDictionary<CubeCoordinate, Cell> tiles = null;

		private CoordConverter converter => target.converter;
		public int tileCount => tiles.Count;

		public BoardEditMode( BoardLayout target ) {
			this.target = target;
			target.gameObject.layer = boardLayer;

			ReloadTiles();
		}

		private void ReloadTiles() {
			target.ReloadTiles( ref tiles );
		}

		private Tool lastTool;
		private bool _enable = false;
		public bool enable {
			get => _enable;

			set {
				if ( value == _enable ) {
					return;
				}

				action = null;
				_enable = value;
				if ( _enable ) {
					lastTool = Tools.current;
					SceneView.RepaintAll();
					Undo.undoRedoPerformed += ReloadTiles;
				}
				else {
					Tools.current = lastTool;
					Undo.undoRedoPerformed -= ReloadTiles;
				}
			}
		}

		private CubeCoordinate GetMousePosition() {
			var ray = HandleUtility.GUIPointToWorldRay( Event.current.mousePosition );
			var plane = new Plane( Vector3.forward, target.transform.position.z );
			if ( plane.Raycast( ray, out float enter ) ) {
				var p = ray.GetPoint( enter );
				return converter.World2Hex( p );
			}

			return new CubeCoordinate( 0, 0 );
		}

		public bool ProcessEvent() {
			var coord = GetMousePosition();
			TileCursor.Draw( converter.Hex2World( coord ) );

			switch ( Event.current.type ) {
				case EventType.MouseDown:
					action = OnClick( coord );
					goto case EventType.MouseDrag;

				case EventType.DragUpdated:
				case EventType.MouseDrag:
					if ( action == null ) {
						return false;
					}

					action( coord );
					return true;

				case EventType.MouseUp:
					action = null;
					return true;
			}
			return false;
		}

		private System.Action<CubeCoordinate> OnClick( CubeCoordinate coord ) {
			switch ( Event.current.button ) {
				case 0:
					if ( tiles.ContainsKey( coord ) ) {
						return RemoveTile;
					}
					else {
						return AddTile;
					}

				case 1:
					enable = false;
					break;
			}

			return null;
		}


		private void AddTile( CubeCoordinate coord ) {
			if ( tiles.ContainsKey( coord ) ) {
				return;
			}

			var instance = Object.Instantiate( template );
			var tile = instance.transform;

			tile.Reset( target.transform );
			tile.position = converter.Hex2World( coord );
			tile.name = coord.ToString();
			tile.gameObject.layer = boardLayer;
			tiles.Add( coord, tile.GetComponent<Cell>() );

			Undo.RegisterCreatedObjectUndo( instance, "Edit Board" );
		}

		private void RemoveTile( CubeCoordinate coord ) {
			if ( tiles.TryGetValue( coord, out var tile ) == false ) {
				return;
			}

			Undo.DestroyObjectImmediate( tile.gameObject );
			tiles.Remove( coord );
		}
	}
}
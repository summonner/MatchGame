using UnityEngine;
using System.Collections.Generic;
using Summoner.Util.Extension;

namespace Summoner.MatchGame {
	[SelectionBase]
	public class BoardGenerator : MonoBehaviour {
		[SerializeField] private GameObject template = null;
		[SerializeField] private float cellRadius = 0.5f;
		[SerializeField] private float gap = 0.02f;
		[SerializeField] private int size = 8;

		void Awake() {
			Debug.Assert( template != null );

			var converter = new CoordConverter( cellRadius, gap );
			var pivot = CreatePivot();
			Bounds bounds = new Bounds();

			foreach ( var position in TraversePoints() ) {
				var cell = SpawnCell( pivot );
				cell.name = position.ToString();
				cell.localPosition = converter.Hex2Board( position );

				bounds.Encapsulate( cell.localPosition );
			}

			bounds.Expand( cellRadius * 2 );
			var inputReceiver = AddInputReceiver( pivot, bounds );
			inputReceiver.converter = converter;

			Destroy( this );
		}

		private Transform CreatePivot() {
			var pivotObj = new GameObject( "Cells" );
			pivotObj.layer = gameObject.layer;

			var pivot = pivotObj.transform;
			pivot.Reset( transform );
			return pivot;
		}

		private IEnumerable<CubeCoordinate> TraversePoints() {
			for ( var x = 0; x < size; ++x ) {
				var offset = x / 2;
				for ( var y = 0 - offset; y < size - offset; ++y ) {
					yield return new CubeCoordinate( x, y );
				}
			}
		}

		private Transform SpawnCell( Transform parent ) {
			var cell = Instantiate( template, parent );
			return cell.transform;
		}

		private InputReceiver AddInputReceiver( Transform pivot, Bounds bounds ) {
			pivot.localPosition -= bounds.center;
			var pivotObj = pivot.gameObject;

			var collider = pivotObj.AddComponent<BoxCollider2D>();
			collider.size = bounds.size;
			collider.offset = bounds.center;

			return pivotObj.AddComponent<InputReceiver>();
		}
	}
}
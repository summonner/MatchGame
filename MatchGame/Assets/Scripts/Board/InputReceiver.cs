using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

namespace Summoner.MatchGame {
	public sealed class InputReceiver : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler {
		private CoordConverter converter;

		public delegate void OnDragDelegate( CubeCoordinate selected, CubeCoordinate direction );
		public event OnDragDelegate onDrag = delegate { };

		public delegate void OnClickDelegate( CubeCoordinate selected );
		public event OnClickDelegate onClick = delegate { };

		void Awake() {
			converter = GetComponent<CoordConverter>();
		}

		public void OnBeginDrag( PointerEventData eventData ) {
			var selected = converter.World2Hex( eventData.pointerPressRaycast.worldPosition );
			var drag = Vector3.Normalize( eventData.pointerCurrentRaycast.worldPosition - eventData.pointerPressRaycast.worldPosition );
			var direction = converter.Board2Hex( drag );
			Debug.Log( $"{selected}, {direction}" );
			onDrag( selected, direction );
		}

		public void OnDrag( PointerEventData eventData ) {
			// do nothing
			// to receive begin drag event
		}

		public void OnPointerClick( PointerEventData eventData ) {
			var selected = converter.World2Hex( eventData.pointerCurrentRaycast.worldPosition );
			Debug.Log( selected );
			onClick( selected );
		}
	}
}
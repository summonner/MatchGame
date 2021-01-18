using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

namespace Summoner.MatchGame {
	public class InputReceiver : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IDragHandler {
		public CoordConverter converter;

		public void OnBeginDrag( PointerEventData eventData ) {
			var selected = converter.World2Hex( eventData.pointerPressRaycast.worldPosition );
			var drag = Vector3.Normalize( eventData.pointerCurrentRaycast.worldPosition - eventData.pointerPressRaycast.worldPosition );
			var direction = converter.Board2Hex( drag );
			Debug.Log( $"{selected}, {direction}" );
		}

		public void OnDrag( PointerEventData eventData ) {
			// do nothing
			// to receive begin drag event
		}

		public void OnPointerDown( PointerEventData eventData ) {
			var selected = converter.World2Hex( eventData.pointerCurrentRaycast.worldPosition );
			Debug.Log( selected );
		}
	}
}
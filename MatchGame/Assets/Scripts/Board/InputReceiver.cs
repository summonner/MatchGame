using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

namespace Summoner.MatchGame {
	public class InputReceiver : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IDragHandler {
		[System.NonSerialized] public CoordConverter converter;

		public delegate void OnDragDelegate( CubeCoordinate selected, CubeCoordinate direction );
		public event OnDragDelegate onDrag = delegate { };

		public delegate void OnClickDelegate( CubeCoordinate selected );
		public event OnClickDelegate onClick = delegate { };

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

		public void OnPointerDown( PointerEventData eventData ) {
			var selected = converter.World2Hex( eventData.pointerCurrentRaycast.worldPosition );
			Debug.Log( selected );
			onClick( selected );
		}
	}
}
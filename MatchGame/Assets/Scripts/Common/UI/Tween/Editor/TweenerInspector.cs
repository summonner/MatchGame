﻿using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace Summoner.UI.Tween {
	[CustomEditor( typeof(TweenBase), true )]
	public class TweenerInspector : Editor {
		private float value = 0f;
		TweenBase tweener = null;

		private bool canPreview {
			get {
				return EditorApplication.isPlaying
					|| PrefabUtility.GetPrefabAssetType( target ) == PrefabAssetType.Regular;
			}
		}

		private void OnEnable() {
			value = 0;
			if ( canPreview == false ) {
				tweener = target as TweenBase;
			}
			else {
				tweener = null;
			}
		}

		private void OnDisable() {
			value = 0;
			if ( tweener == null ) {
				return;
			}

			if ( canPreview == false ) {
				tweener.value = 0;
			}
		}

		public override void OnInspectorGUI() {
			using ( new EnableScope( canPreview == false ) ) {
				value = EditorGUILayout.Slider( "Preview", value, 0, 1 );
				if ( canPreview == false ) {
					tweener.value = value;
				}

				using ( new EditorGUILayout.HorizontalScope() ) {
					if ( GUILayout.Button( "◀" ) == true ) {
						tweener.PlayReverse();
					}

					if ( GUILayout.Button( "■" ) == true ) {
						tweener.Stop();
					}

					if ( GUILayout.Button( "▶" ) == true ) {
						tweener.Play();
					}
				}
			}

			base.OnInspectorGUI();
		}
	}
}
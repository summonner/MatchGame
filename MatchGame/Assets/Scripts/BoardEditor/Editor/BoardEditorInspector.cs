using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace Summoner.MatchGame {
	[CustomEditor( typeof(BoardLayout) )]
	public sealed class BoardEditorInspector : Editor {
		private BoardEditMode editMode;
		private void OnEnable() {
			var layout = target as BoardLayout;
			editMode = new BoardEditMode( layout );
		}

		private void OnDisable() {
			editMode.enable = false;
		}

		public override void OnInspectorGUI() {
			base.OnInspectorGUI();

			EditorGUILayout.Space();
			editMode.enable = GUILayout.Toggle( editMode.enable, "Edit Tile", "button" );

			EditorGUILayout.LabelField( "numTiles", editMode.tileCount.ToString() );
		}

		private void OnSceneGUI() {
			if ( editMode.enable == false ) {
				return;
			}

			Tools.current = Tool.None;

			HandleUtility.AddDefaultControl( GUIUtility.GetControlID( FocusType.Passive ) );
			if ( editMode.ProcessEvent() ) {
				EditorUtility.SetDirty( target );
				Event.current.Use();
			}

			HandleUtility.Repaint();
		}

	}
}
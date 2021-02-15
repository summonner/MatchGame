using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using Summoner.EditorExtensions;

namespace Summoner.MatchGame {
	[CustomPropertyDrawer( typeof(SpawnTable.Record) )]
	public class SpawnTableRecordDrawer : PropertyDrawer {
		public override void OnGUI( Rect position, SerializedProperty property, GUIContent label ) {
			var value = property.FindPropertyRelative( "value" );
			var probability = property.FindPropertyRelative( "probability" );

			PropertyLayoutHelper layout = new PropertyLayoutHelper();
			layout.Begin();
			layout.Add( ( rect ) => { EditorGUI.PropertyField( rect, value, GUIContent.none ); } );
			layout.Add( ( rect ) => { EditorGUI.PropertyField( rect, probability, GUIContent.none ); } );
			layout.End();

			layout.Render( position );
		}
	}
}
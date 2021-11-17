using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer( typeof( GameAction ) )]
public class GameActionCustomPropertyDrawer : PropertyDrawer {
    public override float GetPropertyHeight( SerializedProperty property, GUIContent label ) {
        return EditorGUIUtility.singleLineHeight * 5 + 6;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        float totalHeight = 0;

        EditorGUI.BeginProperty( position, label, property );
        EditorGUI.PrefixLabel( position, label ); 

        totalHeight += 18;

        EditorGUI.indentLevel++;

        SerializedProperty gameActionBlockProperty = property.FindPropertyRelative("GameActionBlock");
        var gameActionBlockRect = new Rect( position.x, position.y + totalHeight, position.width, 16 );
        EditorGUI.PropertyField( gameActionBlockRect, gameActionBlockProperty );
        
        totalHeight += EditorGUI.GetPropertyHeight( gameActionBlockProperty, GUIContent.none );

        System.Type gameActionBlockType = gameActionBlockProperty.objectReferenceValue?.GetType();
        if(gameActionBlockType == typeof(InflictDamageAction)) {
            SerializedProperty statIdProperty = property.FindPropertyRelative("StatId");
            var statIdRect = new Rect( position.x, position.y + totalHeight, position.width, 16 );
            EditorGUI.PropertyField( statIdRect, statIdProperty );

            totalHeight += EditorGUI.GetPropertyHeight( statIdProperty, GUIContent.none );
        }
        else if(gameActionBlockType == typeof(PointVFXActionBlock)) {
            SerializedProperty vfxNameProperty = property.FindPropertyRelative("VFXName");
            var vfxNameRect = new Rect( position.x, position.y + totalHeight, position.width, 16 );
            EditorGUI.PropertyField( vfxNameRect, vfxNameProperty );

            totalHeight += EditorGUI.GetPropertyHeight( vfxNameProperty, GUIContent.none );

            SerializedProperty vfxPointIdProperty = property.FindPropertyRelative("VFXPointId");
            var vfxPointIdRect = new Rect( position.x, position.y + totalHeight, position.width, 16 );
            EditorGUI.PropertyField( vfxPointIdRect, vfxPointIdProperty );

            totalHeight += EditorGUI.GetPropertyHeight( vfxPointIdProperty, GUIContent.none );
        }
        else if(gameActionBlockType == typeof(LineVFXActionBlock)) {
            
        }
        else if(gameActionBlockType == typeof(UnitVFXActionBlock)) {
            
        }

        EditorGUI.indentLevel--;

        EditorGUI.EndProperty();
    }
}

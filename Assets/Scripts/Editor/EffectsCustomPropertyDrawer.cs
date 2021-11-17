// using System.Reflection;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEditor;

// [CustomPropertyDrawer( typeof( Effect ) )]
// public class EffectCustomPropertyDrawer : PropertyDrawer
// {
//     Dictionary<string, SerializedProperty> additionalPropertyDict = new Dictionary<string, SerializedProperty>();
//     float propertyHeight = 0;

//     public override float GetPropertyHeight (SerializedProperty property, GUIContent label) {
//         // float totalHeight = 18;

//         propertyHeight = EditorGUI.GetPropertyHeight( property, includeChildren: true );

//         foreach(KeyValuePair<string, SerializedProperty> p in additionalPropertyDict) {
//             propertyHeight += EditorGUI.GetPropertyHeight( p.Value );
//         }
         
//         return propertyHeight;
//     }

//     public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
//     {
//         float totalHeight = 0;

//         EditorGUI.BeginProperty( position, label, property );
//         EditorGUI.PrefixLabel( position, label );

//         EditorGUI.indentLevel++;

//         totalHeight += 18;

//         string name = "Id";
//         SerializedProperty idProperty = property.FindPropertyRelative(name);
//         var idRect = new Rect( position.x, position.y + totalHeight, position.width, GetPropertyHeight( idProperty, GUIContent.none ) );
//         EditorGUI.PropertyField( idRect, idProperty );

//         totalHeight += EditorGUI.GetPropertyHeight( idProperty, GUIContent.none );

//         name = "TargetFilter";
//         SerializedProperty targetFilterProperty = property.FindPropertyRelative(name);
//         var targetRect = new Rect( position.x, position.y + totalHeight, position.width, GetPropertyHeight(targetFilterProperty, GUIContent.none) );
//         EditorGUI.PropertyField( targetRect, targetFilterProperty );

//         totalHeight += EditorGUI.GetPropertyHeight( targetFilterProperty, GUIContent.none );

//         name = "GameActionBlock";
//         SerializedProperty GameActionBlockProperty = property.FindPropertyRelative(name);
//         var GameActionBlockRect = new Rect( position.x, position.y + totalHeight, position.width, GetPropertyHeight(GameActionBlockProperty, GUIContent.none) );
//         EditorGUI.ObjectField( GameActionBlockRect, GameActionBlockProperty, typeof(GameActionBlock) );

//         totalHeight += EditorGUI.GetPropertyHeight( GameActionBlockProperty, GUIContent.none );

//         System.Type GameActionBlockType = GameActionBlockProperty.objectReferenceValue.GetType();
        
//         if(GameActionBlockType == typeof(InflictDamageAction)) {
//             EditorGUI.indentLevel++;

//             name = "Damage";
//             SerializedObject GameActionBlockSerializedObject = new SerializedObject(GameActionBlockProperty.objectReferenceValue);
//             SerializedProperty damageProperty = GameActionBlockSerializedObject.FindProperty(name);
//             var damageRect = new Rect( position.x, position.y + totalHeight, position.width, GetPropertyHeight(damageProperty, GUIContent.none) );
//             EditorGUI.PropertyField( damageRect, damageProperty );

//             if(!additionalPropertyDict.ContainsKey(name)) {
//                 additionalPropertyDict[name] = damageProperty;
//                 propertyHeight += EditorGUI.GetPropertyHeight( damageProperty, GUIContent.none );
//                 Debug.Log(propertyHeight);
//             }

//             totalHeight += EditorGUI.GetPropertyHeight( damageProperty, GUIContent.none );

//             EditorGUI.indentLevel--;
//         }

//         EditorGUI.EndProperty();
//     }
// }

using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(TargetFilter))]
public class TargetFilterCustomPropertyDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUIUtility.singleLineHeight * 4 + 6;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);
        EditorGUI.PrefixLabel(position, label);

        EditorGUI.indentLevel++;

        var typeRect = new Rect(position.x, position.y + 18, position.width, 16);
        EditorGUI.PropertyField(typeRect, property.FindPropertyRelative("Type"));

        int type = property.FindPropertyRelative("Type").enumValueIndex;

        if (type == 2)
        {
            // Line Filter
            var initialPointRect = new Rect(position.x, position.y + 38, position.width, 16);
            EditorGUI.PropertyField(initialPointRect, property.FindPropertyRelative("InitialPointId"));

            var finalPointRect = new Rect(position.x, position.y + 56, position.width, 16);
            EditorGUI.PropertyField(finalPointRect, property.FindPropertyRelative("FinalPointId"));
        }
        else if (type == 3)
        {
            // AOE Filter
            var AOERect = new Rect(position.x, position.y + 38, position.width, 16);
            EditorGUI.PropertyField(AOERect, property.FindPropertyRelative("AOERadiusId"));

            var targetPointRect = new Rect(position.x, position.y + 56, position.width, 16);
            EditorGUI.PropertyField(targetPointRect, property.FindPropertyRelative("TargetCenterId"));
        }
        else if (type == 4)
        {
            // TargetsOfPreviousEffect
            var PreviousEffectRect = new Rect(position.x, position.y + 38, position.width, 16);
            EditorGUI.PropertyField(PreviousEffectRect, property.FindPropertyRelative("PreviousEffectId"));
        }
        else if (type == 6)
        {
            // Cone Filter
            var coneRange = new Rect(position.x, position.y + 38, position.width, 16);
            EditorGUI.PropertyField(coneRange, property.FindPropertyRelative("ConeRangeId"));

            var coneAngle = new Rect(position.x, position.y + 56, position.width, 16);
            EditorGUI.PropertyField(coneAngle, property.FindPropertyRelative("ConeAngleId"));
        }

        EditorGUI.indentLevel--;

        EditorGUI.EndProperty();
    }
}
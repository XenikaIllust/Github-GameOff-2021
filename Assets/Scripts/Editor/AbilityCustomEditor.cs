// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEditor;

// [CustomEditor(typeof(Ability))]
// public class AbilityCustomEditor : Editor {
//     public override void OnInspectorGUI()
//     {
//         EditorGUI.BeginChangeCheck();

//         Ability ability = (Ability) target;
//         SerializableDictionary<string, float> abilityStats = ability.AbilityStats;
        
//         // if(ability.InputType == AbilityType.TargetPoint) {
//         //     abilityStats["Cast Range"] = EditorGUILayout.FloatField("Cast Range", abilityStats["Cast Range"]);
//         // }
//         // else if(ability.InputType == AbilityType.TargetUnit) {
//         //     abilityStats["Cast Range"] = EditorGUILayout.FloatField("Cast Range", abilityStats["Cast Range"]);
//         // }
//         // else if(ability.InputType == AbilityType.TargetArea) {
//         //     abilityStats["Cast Range"] = EditorGUILayout.FloatField("Cast Range", abilityStats["Cast Range"]);
//         //     abilityStats["AOE Radius"] = EditorGUILayout.FloatField("AOE Radius", abilityStats["AOE Radius"]);
//         // }

//         serializedObject.ApplyModifiedProperties();
//         EditorGUI.EndChangeCheck();

//         base.OnInspectorGUI();
//     }
// }

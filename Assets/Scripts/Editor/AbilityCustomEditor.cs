using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Ability))]
public class AbilityCustomEditor : Editor {
    public override void OnInspectorGUI()
    {
        Ability ability = (Ability) target;
        Dictionary<string, float> abilityStats = ability.AbilityStats;

        if(!abilityStats.ContainsKey("Maximum Cast Distance")) {
            abilityStats.Add("Maximum Cast Distance", 0);
        }
        if(!abilityStats.ContainsKey("AOE Radius")) {
            abilityStats.Add("AOE Radius", 0);
        }
        
        if(ability.InputType == AbilityType.TargetPoint) {
            abilityStats["Maximum Cast Distance"] = EditorGUILayout.FloatField("Maximum Cast Distance", abilityStats["Maximum Cast Distance"]);
        }
        else if(ability.InputType == AbilityType.TargetUnit) {
            abilityStats["Maximum Cast Distance"] = EditorGUILayout.FloatField("Maximum Cast Distance", abilityStats["Maximum Cast Distance"]);
        }
        else if(ability.InputType == AbilityType.TargetArea) {
            abilityStats["Maximum Cast Distance"] = EditorGUILayout.FloatField("Maximum Cast Distance", abilityStats["Maximum Cast Distance"]);
            abilityStats["AOE Radius"] = EditorGUILayout.FloatField("AOE Radius", abilityStats["AOE Radius"]);
        }

        base.OnInspectorGUI();

        serializedObject.ApplyModifiedProperties();
    }
}

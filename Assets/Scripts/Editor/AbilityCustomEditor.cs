using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Ability))]
public class AbilityCustomEditor : Editor {
    public override void OnInspectorGUI()
    {
        EditorGUI.BeginChangeCheck();

        Ability ability = (Ability) target;
        Dictionary<string, float> abilityStats = ability.AbilityStats;

        AbilityStatsSerializableDictionary abilityStatsSerializable = new AbilityStatsSerializableDictionary();

        if(!abilityStatsSerializable.keys.Contains("Cast Range")) {
            abilityStatsSerializable.keys.Add("Cast Range");
            abilityStatsSerializable.values.Add(0);
        }
        if(!abilityStatsSerializable.keys.Contains("Cast Range")) {
            abilityStatsSerializable.keys.Add("AOE Radius");
            abilityStatsSerializable.values.Add(0);
        }
        
        if(ability.InputType == AbilityType.TargetPoint) {
            abilityStats["Cast Range"] = EditorGUILayout.FloatField("Cast Range", abilityStats["Cast Range"]);
        }
        else if(ability.InputType == AbilityType.TargetUnit) {
            abilityStats["Cast Range"] = EditorGUILayout.FloatField("Cast Range", abilityStats["Cast Range"]);
        }
        else if(ability.InputType == AbilityType.TargetArea) {
            abilityStats["Cast Range"] = EditorGUILayout.FloatField("Cast Range", abilityStats["Cast Range"]);
            abilityStats["AOE Radius"] = EditorGUILayout.FloatField("AOE Radius", abilityStats["AOE Radius"]);
        }

        serializedObject.ApplyModifiedProperties();
        EditorGUI.EndChangeCheck();

        base.OnInspectorGUI();
    }
}

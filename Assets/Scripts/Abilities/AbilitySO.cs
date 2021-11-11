using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ability", menuName = "Scriptable Objects/Ability/Ability SO")]
public class AbilitySO : ScriptableObject
{
    public float Duration;
    public float Cooldown;
    public Outcome[] Outcomes;
}

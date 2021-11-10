using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Data component that stores health-related information for a given object type.
/// If you want to modify the base health value of a type of unit, then use this.
/// </summary>
[CreateAssetMenu(fileName = "NewHealthConfig", menuName = "Scriptable Objects/Health/Health Config", order = 0)]
public class HealthConfigSO : ScriptableObject
{
    [SerializeField] private int initialHealth;
    public int InitialHealth => initialHealth;
}

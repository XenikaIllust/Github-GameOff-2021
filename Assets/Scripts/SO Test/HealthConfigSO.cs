using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewHealthConfig", menuName = "Test SO Data/Health Config SO", order = 0)]
public class HealthConfigSO : ScriptableObject
{
    [SerializeField] private int initialHealth;
    public int InitialHealth => initialHealth;
}

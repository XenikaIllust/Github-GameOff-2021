using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DummyTarget : MonoBehaviour
{
    [SerializeField] private HealthConfigSO healthConfigSO;
    private HealthSO healthSO;

    private void Awake()
    {
        // Initialize health values
        healthSO = ScriptableObject.CreateInstance<HealthSO>();
        healthSO.SetMaxHealth(healthConfigSO.InitialHealth);
        healthSO.SetCurrentHealth(healthConfigSO.InitialHealth);
    }

    private void ReactToAttack()
    {

    }
}

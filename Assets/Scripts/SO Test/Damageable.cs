using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// --------------
///   Damageable
/// --------------
/// 
/// A component for tracking health and damage.
/// Attach this script to whatever object you want to take damage.
/// </summary>
public class Damageable : MonoBehaviour
{
    [SerializeField] private HealthConfigSO healthConfigSO;
    public HealthConfigSO HealthConfig => healthConfigSO;

    [SerializeField] private HealthSO healthSO;
    public HealthSO Health => healthSO;

    private UnityEvent ReceivedHit;
    private UnityEvent ReceivedHeal;
}

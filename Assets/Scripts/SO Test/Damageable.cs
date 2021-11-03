using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Damageable : MonoBehaviour
{
    [SerializeField] private HealthConfigSO healthConfigSO;
    public HealthConfigSO HealthConfig => healthConfigSO;

    [SerializeField] private HealthSO healthSO;
    public HealthSO Health => healthSO;

    private UnityEvent ReceivedHit;
    private UnityEvent ReceivedHeal;
}

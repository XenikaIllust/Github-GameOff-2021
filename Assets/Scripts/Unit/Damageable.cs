using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// --------------
///   Damageable
/// --------------
/// 
/// A unit component that tracks and manages health and damage.
/// This component will be the one to react to attacks.
/// </summary>
public class Damageable : MonoBehaviour
{
    [SerializeField] private Unit unit; // The unit we're attached to.

    // Template data for a unit type's default health values
    [SerializeField] private HealthConfigSO healthConfigSO;
    public HealthConfigSO HealthConfig => healthConfigSO;

    // Interface that represents a unit's current health
    [SerializeField] private HealthSO healthSO;
    public HealthSO Health => healthSO;

    public bool isDead;

    private void Awake()
    {
        // Initialize health system
        if (healthSO != null)
        {
            healthSO = ScriptableObject.CreateInstance<HealthSO>();
            healthSO.SetMaxHealth(healthConfigSO.InitialHealth);
            healthSO.SetCurrentHealth(healthConfigSO.InitialHealth);
        }
    }

    /// <summary>
    /// Causes the Damageable component to receive damage.
    /// This method will be called by attacks in their respective OnTriggerEnter or OnCollisionEnter methods.
    /// </summary>
    /// <param name="damage">Amount of damage to receive.</param>
    public void ReceiveAttack(int damage)
    {
        // Ignore attack if the unit has already died.
        if (isDead)
            return;

        healthSO.TakeDamage(damage);

        // Notify other components of the results of the attack
        unit.UnitEvents.RaiseEvent("Unit_UpdateHealthUI", healthSO.CurrentHealth);
        unit.UnitEvents.RaiseEvent("Unit_Hit", this);

        // Check if the unit has died
        if (healthSO.CurrentHealth <= 0)
        {
            isDead = true;

            // Notify other components that the unit has died
            unit.UnitEvents.RaiseEvent("Unit_HasDied", this);
        }
    }

    /// <summary>
    /// For replenishing a unit's health.
    /// </summary>
    /// <param name="amount">Amount of health to recover.</param>
    public void RestoreHealth(int amount)
    {
        // Ignore healing if the unit has already died.
        if (isDead)
            return;

        healthSO.RestoreHealth(amount);

        unit.UnitEvents.RaiseEvent("Unit_UpdateHealthUI", healthSO.CurrentHealth);
    }

    /// <summary>
    /// Instantly kills the unit. Could be useful for specific scenarios.
    /// </summary>
    public void Kill()
    {
        ReceiveAttack(healthSO.CurrentHealth);
    }
}

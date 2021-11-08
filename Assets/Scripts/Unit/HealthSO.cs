using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Scriptable object type that manages the active components of a health mechanic.
/// As in, current health, taking damage, and recovering
/// </summary>
[CreateAssetMenu(fileName = "NewHealthSO", menuName = "Test SO Data/Health SO", order = 1)]
public class HealthSO : ScriptableObject
{
    [SerializeField] private int maxHealth;
    public int MaxHealth => maxHealth;

    [SerializeField] private int currentHealth;
    public int CurrentHealth => currentHealth;

    public void SetCurrentHealth(int newValue)
    {
        currentHealth = newValue;
    }

    public void SetMaxHealth(int newValue)
    {
        maxHealth = newValue;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
    }

    public void RestoreHealth(int amount)
    {
        currentHealth += amount;

        if (currentHealth > maxHealth)
            currentHealth = maxHealth;
    }
}

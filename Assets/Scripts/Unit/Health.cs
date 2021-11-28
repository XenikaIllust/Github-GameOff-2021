using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    private EventProcessor _unitEventHandler;
    private bool _isDead;
    public float health = 100f;
    public float maxHealth;

    private void Awake()
    {
        _unitEventHandler = GetComponent<UnitEventManager>().UnitEventHandler;
        _isDead = false;
        maxHealth = health;
    }

    private void OnEnable()
    {
        _unitEventHandler.StartListening("OnDamageTaken", OnDamageTaken);
        _unitEventHandler.StartListening("OnHealTaken", OnHealTaken);
    }

    private void OnDisable()
    {
        _unitEventHandler.StopListening("OnDamageTaken", OnDamageTaken);
        _unitEventHandler.StopListening("OnHealTaken", OnHealTaken);
    }

    private void OnDamageTaken(object damageValue)
    {
        ReduceHealth((float)damageValue);
        CheckDeathCondition();
        UpdateHealthBar();
    }

    private void OnHealTaken(object healValue)
    {
        IncreaseHealth((float)healValue);
    }

    private void ReduceHealth(float value)
    {
        if (value > float.Epsilon) health -= value;
        if (health <= float.Epsilon) health = float.Epsilon;
    }

    private void IncreaseHealth(float value)
    {
        if (value > float.Epsilon) health += value;
        if (health >= maxHealth) health = maxHealth;
    }

    private void CheckDeathCondition()
    {
        if (_isDead) return;

        if (health <= float.Epsilon)
        {
            _isDead = true;
            _unitEventHandler.RaiseEvent("OnDied", null);
            EventManager.RaiseEvent("OnUnitDied", gameObject);
        }
    }

    private void UpdateHealthBar()
    {
        float damageRate = (maxHealth - health) / maxHealth;
        _unitEventHandler.RaiseEvent("OnHealthChanged", damageRate);
    }
}
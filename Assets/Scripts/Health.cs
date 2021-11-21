using UnityEngine;

public class Health : MonoBehaviour
{
    private EventProcessor _unitEventHandler;
    public float health = 100f;
    private float maxHealth;

    private void Awake()
    {
        _unitEventHandler = GetComponent<UnitEventManager>().UnitEventHandler;
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
    }

    private void IncreaseHealth(float value)
    {
        if (value > float.Epsilon) health += value;
    }

    private void CheckDeathCondition()
    {
        if (health <= float.Epsilon)
        {
            _unitEventHandler.RaiseEvent("OnDied", null);
            EventManager.RaiseEvent("OnUnitDied", gameObject);
        }
    }

    private void UpdateHealthBar()
    {
        _unitEventHandler.RaiseEvent("OnUpdateHealth", (maxHealth - health) / maxHealth);
        Debug.Log($"{gameObject.name} health: {health}");
    }
}
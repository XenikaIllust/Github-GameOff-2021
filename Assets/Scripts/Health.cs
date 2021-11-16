using UnityEngine;

public class Health : MonoBehaviour
{
    private EventProcessor _unitEventHandler;
    public float health = 100f;

    private void Awake()
    {
        _unitEventHandler = GetComponent<UnitEventManager>().UnitEventHandler;
    }

    private void OnEnable()
    {
        _unitEventHandler.StartListening("OnDamageTaken", OnDamageTaken);
    }

    private void OnDamageTaken(object damageValue)
    {
        ReduceHealth((float)damageValue);
        CheckDeathCondition();
        UpdateHealthBar();
    }

    private void ReduceHealth(float value)
    {
        health -= value;
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
        Debug.Log($"{gameObject.name} health: {health}");
    }
}
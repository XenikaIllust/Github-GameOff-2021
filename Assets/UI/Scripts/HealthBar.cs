
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;

    public UnitEventManager unitEventManager;

    void OnEnable()
    {
        unitEventManager.UnitEventHandler.StartListening("OnHealthChanged", SetHealth);
    }

    void OnDisable()
    {
        unitEventManager.UnitEventHandler.StopListening("OnHealthChanged", SetHealth);
    }

    public void SetHealth(object param)
    {
        slider.value = slider.maxValue - (float)param * 100;
    }
}

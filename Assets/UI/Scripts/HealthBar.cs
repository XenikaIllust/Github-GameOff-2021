using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    private UnitEventManager _unitEventManager;

    private void Awake()
    {
        _unitEventManager = FindObjectOfType<PlayerAgent>().GetComponent<UnitEventManager>();
    }

    private void OnEnable()
    {
        _unitEventManager.UnitEventHandler.StartListening("OnHealthChanged", SetHealth);
    }

    private void OnDisable()
    {
        _unitEventManager.UnitEventHandler.StopListening("OnHealthChanged", SetHealth);
    }

    private void SetHealth(object param)
    {
        slider.value = slider.maxValue - (float)param * 100;
    }
}

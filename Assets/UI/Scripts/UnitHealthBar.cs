using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitHealthBar : MonoBehaviour
{
    private Slider slider;
    private UnitEventManager _unitEventManager;

    private void Awake()
    {
        slider = transform.GetComponent<Slider>();
        _unitEventManager = transform.GetComponentInParent<UnitEventManager>();
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

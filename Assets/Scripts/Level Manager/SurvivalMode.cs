using System;
using TMPro;
using UnityEngine;

public class SurvivalMode : MonoBehaviour
{
    [SerializeField] private GameObject popUp;
    [SerializeField] private GameObject timerObject;
    [SerializeField] private float timer;

    private TMP_Text _timerText;
    private bool _winCondition;

    private void Awake()
    {
        popUp.SetActive(false);
        _timerText = timerObject.GetComponentInChildren<TMP_Text>();
        _winCondition = false;
        UpdateText();
    }

    private void Update()
    {
        if (_winCondition) return;

        if (timer <= float.Epsilon)
        {
            _winCondition = true;
            popUp.SetActive(true);
            UpdateText();
        }
        else
        {
            timer -= Time.deltaTime;
            _timerText.text = TimeSpan.FromSeconds(Mathf.FloorToInt(timer)).ToString("g");
            UpdateText();
        }
    }

    private void UpdateText()
    {
        if (timer <= float.Epsilon)
        {
            _timerText.text = TimeSpan.FromSeconds(0).ToString("g");
        }
        else
        {
            _timerText.text = TimeSpan.FromSeconds(Mathf.FloorToInt(timer)).ToString("g");
        }
    }
}
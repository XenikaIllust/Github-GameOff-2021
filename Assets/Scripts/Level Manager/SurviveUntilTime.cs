using System;
using TMPro;
using UnityEngine;

public class SurviveUntilTime : MonoBehaviour
{
    [SerializeField] private GameObject popUp;
    [SerializeField] private GameObject timerObject;
    [SerializeField] private float timer;

    private TMP_Text _timerText;

    private void Awake()
    {
        popUp.SetActive(false);
        _timerText = timerObject.GetComponentInChildren<TMP_Text>();
    }

    private void Update()
    {
        if (timer <= float.Epsilon)
        {
            popUp.SetActive(true);
            _timerText.text = TimeSpan.FromSeconds(0).ToString("g");
        }
        else
        {
            timer -= Time.deltaTime;
            _timerText.text = TimeSpan.FromSeconds(Mathf.FloorToInt(timer)).ToString("g");
        }
    }
}
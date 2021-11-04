using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameStateTest : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;

    private void Awake()
    {
        EventManager.StartListening("GameStateChanged", GameStateChanged);
    }

    private void GameStateChanged(object o)
    {
        text.text = $"Current State: { o.ToString() }";
    }
}

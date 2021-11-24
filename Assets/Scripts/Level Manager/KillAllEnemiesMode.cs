using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class KillAllEnemiesMode : MonoBehaviour
{
    [SerializeField] private GameObject popUp;
    [SerializeField] private GameObject aliveEnemyCounterObject;

    private List<SwarmerAI> _listOfEnemies;
    private int _aliveEnemyCounter;
    private TMP_Text _aliveEnemyCounterText;

    private void Awake()
    {
        popUp.SetActive(false);
        _listOfEnemies = new List<SwarmerAI>(FindObjectsOfType<SwarmerAI>());
        _aliveEnemyCounter = _listOfEnemies.Count;
        _aliveEnemyCounterText = aliveEnemyCounterObject.GetComponentInChildren<TMP_Text>();
        UpdateText();
    }

    private void OnEnable()
    {
        EventManager.StartListening("OnUnitDied", OnUnitDied);
    }

    private void OnDisable()
    {
        EventManager.StopListening("OnUnitDied", OnUnitDied);
    }

    private void OnUnitDied(object unit)
    {
        var unitGameObject = (GameObject)unit;

        if (unitGameObject.GetComponent<SwarmerAI>() != null)
        {
            _aliveEnemyCounter -= 1;
            UpdateText();

            if (_aliveEnemyCounter == 0)
            {
                popUp.SetActive(true);
            }
        }
    }

    private void UpdateText()
    {
        _aliveEnemyCounterText.text = $"Alive Enemies: {_aliveEnemyCounter}";
    }
}
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AbilityManager : MonoBehaviour
{
    private Unit _playerUnit;
    [SerializeField] private List<Ability> currentAbilities;
    private List<Ability> _allAbilities;
    [SerializeField] private List<GameObject> currentAbilityPrefabs;
    private GameObject _newAbilityPrefab;

    private void Awake()
    {
        _allAbilities = new List<Ability>(Resources.LoadAll<Ability>("Abilities"));
    }

    private void OnEnable()
    {
        SceneManager.sceneUnloaded += OnSceneUnloaded;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneUnloaded += OnSceneUnloaded;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneUnloaded(Scene arg0)
    {
        EventManager.StopListening("OnPlayerSpawned", OnPlayerSpawned);
        ImportPlayerAbilities();
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        EventManager.StartListening("OnPlayerSpawned", OnPlayerSpawned);
    }

    private void OnPlayerSpawned(object unit)
    {
        _playerUnit = (Unit)unit;
        ExportPlayerAbilities();
    }

    private void ImportPlayerAbilities()
    {
        currentAbilities = _playerUnit.abilities;
    }

    private void ExportPlayerAbilities()
    {
        _playerUnit.abilities = currentAbilities;
    }

    private void ChangeAbilities(Ability oldAbility, Ability newAbility)
    {
        var index = currentAbilities.IndexOf(oldAbility);
        currentAbilities[index] = newAbility;
    }
}
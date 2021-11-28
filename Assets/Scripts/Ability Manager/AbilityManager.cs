using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AbilityManager : MonoBehaviour
{
    private Unit _playerUnit;
    public List<Ability> currentAbilities;
    private List<Ability> _allAbilities;
    public List<AbilityPrefab> currentAbilityPrefabs;
    public AbilityPrefab newAbilityPrefab;
    [Space] public Canvas canvas;
    public HorizontalLayoutGroup horizontalLayoutGroup;

    private void Awake()
    {
        _allAbilities = new List<Ability>(Resources.LoadAll<Ability>("Abilities"));
        ExportToPrefabs();
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
        EventManager.StopListening("OnGamePaused", OnGamePaused);
        EventManager.StopListening("OnGameResumed", OnGameResumed);
        EventManager.StopListening("OnPlayerSpawned", OnPlayerSpawned);
        ImportFromUnit();
        ExportToPrefabs();
        _playerUnit = null;
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        EventManager.StartListening("OnGamePaused", OnGamePaused);
        EventManager.StartListening("OnGameResumed", OnGameResumed);
        EventManager.StartListening("OnPlayerSpawned", OnPlayerSpawned);
    }

    private void OnGameResumed(object @null)
    {
        canvas.enabled = false;
    }

    private void OnGamePaused(object @null)
    {
        canvas.enabled = true;
    }

    private void OnPlayerSpawned(object unit)
    {
        _playerUnit = (Unit)unit;
        ExportToUnit();
        ExportToPrefabs();
    }

    private void ImportFromUnit()
    {
        if (_playerUnit == null) return;
        currentAbilities = _playerUnit.abilities;
    }

    public void ExportToUnit()
    {
        if (_playerUnit == null) return;
        _playerUnit.abilities = currentAbilities;
    }

    public void ImportFromPrefabs()
    {
        for (var i = 0; i < currentAbilities.Count; i++) currentAbilities[i] = currentAbilityPrefabs[i].ability;
    }

    private void ExportToPrefabs()
    {
        for (var i = 0; i < currentAbilityPrefabs.Count; i++) currentAbilityPrefabs[i].ability = currentAbilities[i];
    }

    public void AdjustUnitCooldownTimer(int index1, int index2)
    {
        if (_playerUnit == null) return;
        (_playerUnit.abilityCooldownList[index1], _playerUnit.abilityCooldownList[index2])
            = (_playerUnit.abilityCooldownList[index2], _playerUnit.abilityCooldownList[index1]);
    }
}
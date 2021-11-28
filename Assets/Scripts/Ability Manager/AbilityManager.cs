using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AbilityManager : MonoBehaviour
{
    [HideInInspector] public Unit playerUnit;
    public List<Ability> currentAbilities;
    private List<Ability> _allAbilities;
    public List<AbilityPrefab> currentAbilityPrefabs;
    public AbilityPrefab newAbilityPrefab;
    [Space] public Canvas canvas;
    public HorizontalLayoutGroup horizontalLayoutGroup;

    private void Awake()
    {
        _allAbilities = new List<Ability>(Resources.LoadAll<Ability>("Abilities"));
        while (currentAbilities.Count < 4)
        {
            var newAbility = _allAbilities[Random.Range(0, _allAbilities.Count)];
            if (currentAbilities.Contains(newAbility) == false) currentAbilities.Add(newAbility);
        }

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
        playerUnit = null;
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
        playerUnit = (Unit)unit;
        ExportToUnit();
        ExportToPrefabs();
    }

    private void ImportFromUnit()
    {
        if (playerUnit == null) return;
        currentAbilities = playerUnit.abilities;
    }

    public void ExportToUnit()
    {
        if (playerUnit == null) return;
        playerUnit.abilities = currentAbilities;
    }

    public void ImportFromPrefabs()
    {
        for (var i = 0; i < currentAbilities.Count; i++) currentAbilities[i] = currentAbilityPrefabs[i].ability;
    }

    private void ExportToPrefabs()
    {
        for (var i = 0; i < currentAbilityPrefabs.Count; i++) currentAbilityPrefabs[i].ability = currentAbilities[i];
        UpdateAbilityPrefabsUI();
    }

    public void AdjustUnitCooldownTimer(int index1, int index2)
    {
        if (playerUnit == null) return;
        (playerUnit.abilityCooldownList[index1], playerUnit.abilityCooldownList[index2])
            = (playerUnit.abilityCooldownList[index2], playerUnit.abilityCooldownList[index1]);
    }

    public void UpdateAbilityPrefabsUI()
    {
        foreach (var abilityPrefab in currentAbilityPrefabs)
        {
            abilityPrefab.abilityNameUI.text = abilityPrefab.ability.abilityName;
        }
    }
}
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class AbilityManager : MonoBehaviour
{
    [HideInInspector] public Unit playerUnit;
    public float dragDropRadius = 150;
    public List<Ability> currentAbilities;
    private List<Ability> _playerAbilityPool;
    public List<AbilityPrefab> currentAbilityPrefabs;
    public AbilityPrefab newAbilityPrefab;
    [Space] public Canvas canvas;
    public HorizontalLayoutGroup horizontalLayoutGroup;
    public float ability5Cooldown;
    private bool _newAbilityAvailable = true;

    private void Awake()
    {
        _playerAbilityPool = new List<Ability>(Resources.LoadAll<Ability>("Abilities/PlayerAbilities"));
        while (currentAbilities.Count < 4)
        {
            var newAbility = _playerAbilityPool[Random.Range(0, _playerAbilityPool.Count)];
            if (currentAbilities.Contains(newAbility) == false) currentAbilities.Add(newAbility);
        }

        ExportToPrefabs();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        EventManager.StartListening("OnGamePaused", OnGamePaused);
        EventManager.StartListening("OnGameResumed", OnGameResumed);
        EventManager.StartListening("OnPlayerSpawned", OnPlayerSpawned);
        EventManager.StartListening("OnUnitDied", OnUnitDied);
    }

    private void OnSceneUnloaded(Scene arg0)
    {
        EventManager.StopListening("OnGamePaused", OnGamePaused);
        EventManager.StopListening("OnGameResumed", OnGameResumed);
        EventManager.StopListening("OnPlayerSpawned", OnPlayerSpawned);
        EventManager.StopListening("OnUnitDied", OnUnitDied);
        ImportFromUnit();
        ExportToPrefabs();
        playerUnit = null;
    }

    private void OnGameResumed(object @null)
    {
        canvas.enabled = false;
        CleanUpNewAbility();
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

    private void OnUnitDied(object unitGameObject)
    {
        if (!_newAbilityAvailable) return;
        _newAbilityAvailable = false;

        var dropRate = ((GameObject)unitGameObject).GetComponent<Unit>().bountyDropRate;
        var roll = Random.Range(0f, 100f);

        if (roll < dropRate)
        {
            LearnNewAbility();
        }
    }

    private void LearnNewAbility()
    {
        FindObjectOfType<PlayerUI>().PauseGame();

        while (newAbilityPrefab.ability == null)
        {
            var newAbility = _playerAbilityPool[Random.Range(0, _playerAbilityPool.Count)];
            newAbilityPrefab.ability = currentAbilities.Contains(newAbility) == false ? newAbility : null;
        }

        newAbilityPrefab.gameObject.SetActive(true);
        UpdateAbilityPrefabsUI();
    }

    private void CleanUpNewAbility()
    {
        ability5Cooldown = 0;
        newAbilityPrefab.ability = null;
        newAbilityPrefab.gameObject.SetActive(false);
        _newAbilityAvailable = true;
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
        (playerUnit.abilityCooldownList[index1], playerUnit.abilityCooldownList[index2]) = (
            playerUnit.abilityCooldownList[index2], playerUnit.abilityCooldownList[index1]);
    }

    public void UpdateAbilityPrefabsUI()
    {
        var uiPrefabs = new List<AbilityPrefab>(currentAbilityPrefabs);
        if (newAbilityPrefab.ability != null) uiPrefabs.Add(newAbilityPrefab);

        foreach (var abilityPrefab in uiPrefabs)
        {
            abilityPrefab.abilityNameUI.text = abilityPrefab.ability.abilityName;
        }
    }
}

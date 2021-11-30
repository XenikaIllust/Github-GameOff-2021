using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class AbilityPrefab : MonoBehaviour
{
    private AbilityManager _abilityManager;
    public Ability ability;
    private int _index = -1;
    [Header("UI")] public TMP_Text abilityNameUI;
    public Image abilityImageUI;

    private void Awake()
    {
        _abilityManager = FindObjectOfType<AbilityManager>();
        _index = _abilityManager.currentAbilityPrefabs.IndexOf(this);
    }

    public void OnBeginDrag()
    {
        _abilityManager.currentGroup.enabled = false;
        _abilityManager.newGroup.enabled = false;
    }

    public void OnDrag()
    {
        transform.position = Mouse.current.position.ReadValue();
    }

    public void OnEndDrag()
    {
        var prefabs = _abilityManager.currentAbilityPrefabs;
        if (Vector2.Distance(transform.position, _abilityManager.newAbilityPrefab.transform.position)
            <= _abilityManager.dragDropRadius && _abilityManager.newAbilityPrefab.ability != null)
        {
            NewAbilityDragged();

            if (_index != -1)
            {
                (ability, _abilityManager.newAbilityPrefab.ability)
                    = (_abilityManager.newAbilityPrefab.ability, ability);

                (_abilityManager.playerUnit.abilityCooldownList[_index], _abilityManager.ability5Cooldown) = (
                    _abilityManager.ability5Cooldown, _abilityManager.playerUnit.abilityCooldownList[_index]);
            }
            else
            {
                for (var i = 0; i < prefabs.Count; i++)
                {
                    if (Vector2.Distance(transform.position, prefabs[i].transform.position)
                        <= _abilityManager.dragDropRadius)
                    {
                        (ability, prefabs[i].ability) = (prefabs[i].ability, ability);
                        (_abilityManager.ability5Cooldown, _abilityManager.playerUnit.abilityCooldownList[i]) = (
                            _abilityManager.playerUnit.abilityCooldownList[i], _abilityManager.ability5Cooldown);
                    }
                }
            }
        }
        else
        {
            for (var i = 0; i < prefabs.Count; i++)
            {
                if (Vector2.Distance(transform.position, prefabs[i].transform.position)
                    <= _abilityManager.dragDropRadius)
                {
                    (ability, prefabs[i].ability) = (prefabs[i].ability, ability);
                    _abilityManager.AdjustUnitCooldownTimer(i, _index);
                }
            }
        }

        _abilityManager.currentGroup.enabled = true;
        _abilityManager.newGroup.enabled = true;
        _abilityManager.ImportFromPrefabs();
        _abilityManager.ExportToUnit();
        _abilityManager.UpdateAbilityPrefabsUI();
    }

    private void NewAbilityDragged()
    {
        _abilityManager.topText.text = "RESUME GAME (ESC)\nTO CONFIRM CHANGES";
        _abilityManager.bottomText.text = "(UNLEARN THIS ABILITY)";
    }
}

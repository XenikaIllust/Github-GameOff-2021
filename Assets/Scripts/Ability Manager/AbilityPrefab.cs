using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class AbilityPrefab : MonoBehaviour
{
    private AbilityManager _abilityManager;
    public Ability ability;
    private int _index = -1;
    public Image abilityImageUI;
    private Transform _realParent;
    private int _realSiblingIndex;

    private void Awake()
    {
        _abilityManager = FindObjectOfType<AbilityManager>();
        _index = _abilityManager.currentAbilityPrefabs.IndexOf(this);
        _realParent = transform.parent;
        _realSiblingIndex = transform.GetSiblingIndex();
    }

    public void OnPointerEnter()
    {
        _abilityManager.lastClickedAbility = ability;
        _abilityManager.UpdateAbilityPrefabsUI();
    }

    public void OnBeginDrag()
    {
        _abilityManager.lastClickedAbility = ability;
        _abilityManager.UpdateAbilityPrefabsUI();
        _abilityManager.currentAbilityPanel.enabled = false;
        _abilityManager.newAbilityPanel.enabled = false;
        transform.SetParent(_abilityManager.mostFrontCanvas.transform);
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
            if (_index != -1)
            {
                (ability, _abilityManager.newAbilityPrefab.ability)
                    = (_abilityManager.newAbilityPrefab.ability, ability);

                (_abilityManager.playerUnit.abilityCooldownList[_index], _abilityManager.ability5Cooldown) = (
                    _abilityManager.ability5Cooldown, _abilityManager.playerUnit.abilityCooldownList[_index]);

                NewAbilitySwapped();
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

                        NewAbilitySwapped();
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

        transform.SetParent(_realParent);
        transform.SetSiblingIndex(_realSiblingIndex);
        _abilityManager.currentAbilityPanel.enabled = true;
        _abilityManager.newAbilityPanel.enabled = true;
        _abilityManager.ImportFromPrefabs();
        _abilityManager.ExportToUnit();
        _abilityManager.UpdateAbilityPrefabsUI();
    }

    private void NewAbilitySwapped()
    {
        _abilityManager.topText.text = "RESUME GAME (ESC)\nTO CONFIRM CHANGES";
        _abilityManager.bottomText.text = "(UNLEARN THIS ABILITY)";
    }
}

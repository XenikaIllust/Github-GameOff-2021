using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class AbilityPrefab : MonoBehaviour
{
    private AbilityManager _abilityManager;
    public Ability ability;
    private float _dropRadius;
    private int _index = -1;
    public TMP_Text abilityNameUI;

    private void Awake()
    {
        _abilityManager = FindObjectOfType<AbilityManager>();
        _index = _abilityManager.currentAbilityPrefabs.IndexOf(this);
    }

    private void OnEnable()
    {
        _dropRadius = _abilityManager.dragDropRadius;
    }

    private void OnDisable()
    {
        _dropRadius = -1;
    }

    public void OnBeginDrag()
    {
        _abilityManager.horizontalLayoutGroup.enabled = false;
    }

    public void OnDrag()
    {
        transform.position = Mouse.current.position.ReadValue();
    }

    public void OnEndDrag()
    {
        var prefabs = _abilityManager.currentAbilityPrefabs;
        if (Vector3.Distance(transform.position, _abilityManager.newAbilityPrefab.transform.position) <= _dropRadius
            && _abilityManager.newAbilityPrefab.ability != null)
        {
            if (_index != -1)
            {
                (ability, _abilityManager.newAbilityPrefab.ability)
                    = (_abilityManager.newAbilityPrefab.ability, ability);

                (_abilityManager.playerUnit.abilityCooldownList[_index], _abilityManager.ability5Cooldown)
                    = (_abilityManager.ability5Cooldown, _abilityManager.playerUnit.abilityCooldownList[_index]);
            }
            else
            {
                for (var i = 0; i < prefabs.Count; i++)
                {
                    if (Vector3.Distance(transform.position, prefabs[i].transform.position) <= _dropRadius)
                    {
                        (ability, prefabs[i].ability) = (prefabs[i].ability, ability);
                        (_abilityManager.ability5Cooldown, _abilityManager.playerUnit.abilityCooldownList[i])
                            = (_abilityManager.playerUnit.abilityCooldownList[i], _abilityManager.ability5Cooldown);
                    }
                }
            }
        }
        else
        {
            for (var i = 0; i < prefabs.Count; i++)
            {
                if (Vector3.Distance(transform.position, prefabs[i].transform.position) <= _dropRadius)
                {
                    (ability, prefabs[i].ability) = (prefabs[i].ability, ability);
                    _abilityManager.AdjustUnitCooldownTimer(i, _index);
                }
            }
        }

        _abilityManager.horizontalLayoutGroup.enabled = true;
        _abilityManager.ImportFromPrefabs();
        _abilityManager.ExportToUnit();
        _abilityManager.UpdateAbilityPrefabsUI();
    }
}

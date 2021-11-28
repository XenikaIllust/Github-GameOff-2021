using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class AbilityPrefab : MonoBehaviour
{
    private AbilityManager _abilityManager;
    public Ability ability;
    [SerializeField] private float dropRadius = 150;
    private int _index = -1;
    public TMP_Text abilityNameUI;
    private float _ability5Cooldown;

    private void Awake()
    {
        _abilityManager = FindObjectOfType<AbilityManager>();
        _index = _abilityManager.currentAbilityPrefabs.IndexOf(this);
    }

    private void OnDisable()
    {
        _ability5Cooldown = float.Epsilon;
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
        if (Vector3.Distance(transform.position, _abilityManager.newAbilityPrefab.transform.position) <= dropRadius)
        {
            if (_index != -1)
            {
                (ability, _abilityManager.newAbilityPrefab.ability)
                    = (_abilityManager.newAbilityPrefab.ability, ability);

                (_abilityManager.playerUnit.abilityCooldownList[_index], _ability5Cooldown)
                    = (_ability5Cooldown, _abilityManager.playerUnit.abilityCooldownList[_index]);
            }
        }
        else
        {
            var abilities = _abilityManager.currentAbilityPrefabs;
            for (var i = 0; i < abilities.Count; i++)
            {
                if (Vector3.Distance(transform.position, abilities[i].transform.position) <= dropRadius)
                {
                    (ability, abilities[i].ability) = (abilities[i].ability, ability);
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
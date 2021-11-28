using UnityEngine;
using UnityEngine.InputSystem;

public class AbilityPrefab : MonoBehaviour
{
    private AbilityManager _abilityManager;
    public Ability ability;
    [SerializeField] private float dropRadius = 100;
    [SerializeField] private int index;

    private void Awake()
    {
        _abilityManager = FindObjectOfType<AbilityManager>();
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
            (ability, _abilityManager.newAbilityPrefab.ability) = (_abilityManager.newAbilityPrefab.ability, ability);
        }
        else
        {
            var abilities = _abilityManager.currentAbilityPrefabs;
            for (var i = 0; i < abilities.Count; i++)
            {
                if (Vector3.Distance(transform.position, abilities[i].transform.position) <= dropRadius)
                {
                    (ability, abilities[i].ability) = (abilities[i].ability, ability);
                    _abilityManager.AdjustUnitCooldownTimer(i, index);
                }
            }
        }

        _abilityManager.horizontalLayoutGroup.enabled = true;
        _abilityManager.ImportFromPrefabs();
        _abilityManager.ExportToUnit();
    }
}
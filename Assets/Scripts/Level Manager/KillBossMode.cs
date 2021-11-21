using UnityEngine;

public class KillBossMode : MonoBehaviour
{
    [SerializeField] private GameObject popUp;
    [SerializeField] private GameObject bossObject;

    private void Awake()
    {
        popUp.SetActive(false);
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
        if (ReferenceEquals(unit, bossObject))
        {
            popUp.SetActive(true);
        }
    }
}
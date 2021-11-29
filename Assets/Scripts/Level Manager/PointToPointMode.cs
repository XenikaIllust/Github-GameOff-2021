using UnityEngine;

public class PointToPointMode : MonoBehaviour
{
    [SerializeField] private GameObject popUp;
    [SerializeField] private float triggerDistance = 1f;

    private void Awake()
    {
        popUp.SetActive(false);
    }

    private void OnEnable()
    {
        EventManager.StartListening("OnPlayerPositionChanged", OnPositionChanged);
    }

    private void OnDisable()
    {
        EventManager.StopListening("OnPlayerPositionChanged", OnPositionChanged);
    }

    private void OnPositionChanged(object position)
    {
        popUp.SetActive(Vector2.Distance(transform.position, (Vector2)position) < triggerDistance);
    }
}

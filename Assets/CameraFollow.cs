using DG.Tweening;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float cameraSpeed = 2f;
    
    private void OnEnable()
    {
        EventManager.StartListening("OnPlayerPositionChanged", OnPlayerPositionChanged);
    }

    private void OnDisable()
    {
        EventManager.StopListening("OnPlayerPositionChanged", OnPlayerPositionChanged);
    }

    private void OnPlayerPositionChanged(object destination)
    {
        transform.DOKill();
        transform.DOMove(new Vector3
            (((Vector3)destination).x,((Vector3)destination).y, transform.position.z), cameraSpeed)
            .SetSpeedBased().SetEase(Ease.Linear);
    }
}

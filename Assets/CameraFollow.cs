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

    private void OnPlayerPositionChanged(object player)
    {
        Follow((Vector3)player);
    }

    private void Follow(Vector3 player)
    {
        transform.DOKill();
        transform.DOMove(new Vector3
                (player.x, player.y, transform.position.z), cameraSpeed)
            .SetSpeedBased().SetEase(Ease.Linear);
    }
}
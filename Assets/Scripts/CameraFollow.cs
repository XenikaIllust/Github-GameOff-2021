using UnityEngine;
using DG.Tweening;

public class CameraFollow : MonoBehaviour
{
    public float cameraSpeed = 2f;
    [SerializeField]private Transform player;

    private void Update()
    {
        transform.DOKill();
        transform.DOMove(new Vector3 (player.position.x, player.position.y, transform.position.z),
                                        cameraSpeed)
                                        .SetSpeedBased().SetEase(Ease.Linear);
    }
}

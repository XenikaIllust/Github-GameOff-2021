using UnityEngine;
using DG.Tweening;

public class CameraFollow : MonoBehaviour
{
    public float cameraSpeed = 2f;
    [SerializeField] private Transform player;

    private void Awake()
    {
        player = FindObjectOfType<PlayerAgent>().GetComponent<Transform>();
    }

    private void Update()
    {
        transform.DOKill();

        var position = player.position;
        transform.DOMove(new Vector3(position.x, position.y, transform.position.z), cameraSpeed)
            .SetSpeedBased().SetEase(Ease.Linear);
    }
}

using DG.Tweening;
using UnityEngine;

public class RTSPlayerController2D : MonoBehaviour
{
    public float speed = 1f;

    private Camera _camera;
    private Transform _transform;
    private float _zPosition;

    private void Awake()
    {
        _camera = Camera.main;
        _transform = transform;
        _zPosition = _transform.position.z;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            _transform.DOKill();
            
            Vector3 point = CursorWorldPosition();
            _transform.DOMove(point, TrueSpeed(point));
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            _transform.DOKill();
        }
    }

    private Vector3 CursorWorldPosition()
    {
        Vector2 screenPosition = Input.mousePosition;
        Vector3 worldPosition = _camera.ScreenToWorldPoint(screenPosition);

        return new Vector3(worldPosition.x, worldPosition.y, _zPosition);
    }

    private float TrueSpeed(Vector3 newVector3)
    {
        return Vector3.Distance(_transform.position, newVector3) / speed;
    }
}

using DG.Tweening;
using UnityEngine;

public class RTSPlayerController2D : MonoBehaviour
{
    public float speed = 1f;
    public float autoClickInterval = 0.1f;

    private Camera _camera;
    private Transform _transform;
    private float _zPosition;
    private float _autoClickTimer;

    private void Awake()
    {
        _camera = Camera.main;
        _transform = transform;
        _zPosition = _transform.position.z;
    }

    private void Update()
    {
        // Right click
        if (Input.GetMouseButtonDown(1))
        {
            Move();
        }

        // Hold to auto-repeat right click
        if (Input.GetMouseButton(1))
        {
            _autoClickTimer += Time.deltaTime;

            if (_autoClickTimer >= autoClickInterval)
            {
                _autoClickTimer = float.Epsilon;
                Move();
            }
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            Stop();
        }
    }

    private void Move()
    {
        _transform.DOKill();

        Vector3 point = CursorWorldPosition();
        _transform.DOMove(point, TrueSpeed(point));
    }

    private void Stop()
    {
        _transform.DOKill();
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
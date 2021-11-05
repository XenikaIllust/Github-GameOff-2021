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
            Move(CursorWorldPosition());
        }
        else if (Input.GetMouseButton(1))
        {
            // Auto-repeat click when held
            _autoClickTimer += Time.deltaTime;

            if (_autoClickTimer >= autoClickInterval)
            {
                _autoClickTimer = float.Epsilon;
                Move(CursorWorldPosition());
            }
        }
        else if (Input.GetMouseButtonUp(1))
        {
            _autoClickTimer = float.Epsilon;
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            Stop();
        }
    }

    private void Move(Vector3 destination)
    {
        _transform.DOKill();
        _transform.DOMove(destination, TrueSpeed(destination)).SetEase(Ease.Linear);
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

    private float TrueSpeed(Vector3 destination)
    {
        return Vector3.Distance(_transform.position, destination) / speed;
    }
}
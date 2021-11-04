using DG.Tweening;
using UnityEngine;

public class RTSPlayerController2D : MonoBehaviour
{
    private Camera _camera;
    private Transform _transform;

    private void Awake()
    {
        _camera = Camera.main;
        _transform = gameObject.transform;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            _transform.DOMove(CursorWorldPosition(), 2f);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            _transform.DOPause();
        }
    }

    private Vector3 CursorWorldPosition()
    {
        Vector2 screenPosition = Input.mousePosition;
        Vector3 worldPosition = _camera.ScreenToWorldPoint(screenPosition);

        return new Vector3(worldPosition.x, worldPosition.y, 1f);
    }
}

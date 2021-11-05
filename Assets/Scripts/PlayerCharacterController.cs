using UnityEngine;

public class PlayerCharacterController : CharacterController
{
    public float autoClickInterval = 0.1f;

    private Camera _camera;
    private float _autoClickTimer;

    protected override void Awake()
    {
        base.Awake();
        _camera = Camera.main;
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

    private Vector3 CursorWorldPosition()
    {
        Vector2 screenPosition = Input.mousePosition;
        Vector3 worldPosition = _camera.ScreenToWorldPoint(screenPosition);

        return new Vector3(worldPosition.x, worldPosition.y, CharacterTransform.position.z);
    }
}
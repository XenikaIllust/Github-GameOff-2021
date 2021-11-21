using UnityEngine;
using UnityEngine.UI;

public class Waypoint : MonoBehaviour
{
    public Transform target;
    private RectTransform _rectTransform;
    private float _radius;

    private void Awake()
    {
        if (FindObjectOfType<PointToPointMode>() == null) Destroy(gameObject);

        _rectTransform = GetComponent<RectTransform>();
        _radius = _rectTransform.rect.height;
    }

    private void Update()
    {
        Point2Target();
        HideWaypoint();
        PlaceWaypointOnScreenEdge();
    }

    // Makes the waypoint point to the objective.
    private void Point2Target()
    {
        Vector2 position = _rectTransform.position;
        Vector2 targetPosition = Camera.main.WorldToScreenPoint(target.position);

        float angle = Mathf.Atan2(targetPosition.y - position.y, targetPosition.x - position.x);

        _rectTransform.eulerAngles = Vector3.forward * (angle * Mathf.Rad2Deg - 90f);
    }

    // If the objective is offscreen, then return true.
    private bool isTargetOffScreen()
    {
        Vector2 targetPosition = Camera.main.WorldToScreenPoint(target.position);

        return targetPosition.x <= 0 || targetPosition.x >= Screen.width || targetPosition.y <= 0 ||
               targetPosition.y >= Screen.height;
    }

    // If objective is on screen, hide the waypoint.
    // Only show the waypoint if it is onscreen.
    private void HideWaypoint()
    {
        Image waypointImg = GetComponent<Image>();
        waypointImg.enabled = isTargetOffScreen();
    }

    // Place the waypoint on the screen edge.
    private void PlaceWaypointOnScreenEdge()
    {
        Vector2 goalPosition = Camera.main.WorldToScreenPoint(target.position);

        goalPosition.x = Mathf.Clamp(goalPosition.x, _radius, Screen.width - _radius);
        goalPosition.y = Mathf.Clamp(goalPosition.y, _radius, Screen.height - _radius);

        _rectTransform.position = goalPosition;
    }
}
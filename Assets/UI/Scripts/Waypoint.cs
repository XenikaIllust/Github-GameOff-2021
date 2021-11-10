
using UnityEngine;
using UnityEngine.UI;

public class Waypoint : MonoBehaviour
{
    public Transform target;

    private RectTransform rectTransform;

    private float radius;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();

        radius = rectTransform.rect.height;
    }

    void Update()
    {
        Point2Target();
        
        HideWaypoint();

        PlaceWaypointOnScreenEdge();
    }

    // Makes the waypoint point to the objective.
    void Point2Target()
    {
        Vector2 position = rectTransform.position;
        Vector2 targetPosition = Camera.main.WorldToScreenPoint(target.position);

        float angle = Mathf.Atan2(targetPosition.y - position.y, targetPosition.x - position.x);

        rectTransform.eulerAngles = Vector3.forward * (angle * Mathf.Rad2Deg - 90f);
    }

    // If the objective is offscreen, then return true.
    bool isTargetOffScreen()
    {
        Vector2 targetPosition = Camera.main.WorldToScreenPoint(target.position);

        return targetPosition.x <= radius || targetPosition.x >= Screen.width-radius || targetPosition.y <= radius || targetPosition.y >= Screen.height-radius;
    }

    // If objective is on screen, hide the waypoint.
    // Only show the waypoint if it is onscreen.
    void HideWaypoint()
    {
        Image waypointImg = GetComponent<Image>();

        waypointImg.enabled = isTargetOffScreen();
    }

    // Place the waypoint on the screen edge.
    void PlaceWaypointOnScreenEdge()
    {
        Vector2 goalPosition = Camera.main.WorldToScreenPoint(target.position);
        
        goalPosition.x = Mathf.Clamp(goalPosition.x, radius, Screen.width-radius);
        goalPosition.y = Mathf.Clamp(goalPosition.y, radius, Screen.height-radius);

        rectTransform.position = goalPosition;
    }
}

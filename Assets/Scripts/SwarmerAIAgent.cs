using UnityEngine;

public class SwarmerAIAgent : Agent
{
    private void OnEnable()
    {
        EventManager.StartListening("OnPlayerPositionChanged", OnPlayerPositionChanged);
    }

    private void OnDisable()
    {
        EventManager.StopListening("OnPlayerPositionChanged", OnPlayerPositionChanged);
    }

    private void OnPlayerPositionChanged(object newPosition)
    {
        unitEventHandler.RaiseEvent("OnMoveOrderIssued", (Vector3) newPosition);
    }
}
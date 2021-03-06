using System.Linq;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class AbilityInputType
{
    public static bool hasPressedLeftClick;

    public static IEnumerator PointTargetInput(Ability ability, EventProcessor unitEventHandler)
    {
        if (ability.quickCast)
        {
            hasPressedLeftClick = true;
        }
        else
        {
            ChangeCursor();

            hasPressedLeftClick = false;
            yield return new WaitUntil(() => hasPressedLeftClick); // Wait until Left Click is pressed
        }

        // (TODO) Check if the target is valid (is above terrain for example)

        // Calculate mouse position
        Vector3 targetPoint = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        // Send Vector3 targetCoordinates as event param
        object targetCoordinates = targetPoint;
        unitEventHandler.RaiseEvent("OnAbilityInputSet", targetCoordinates);

        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto); // Change cursor back to default
    }

    public static IEnumerator UnitTargetInput(Ability ability, EventProcessor unitEventHandler)
    {
        if (ability.quickCast)
        {
            hasPressedLeftClick = true;
        }
        else
        {
            ChangeCursor();

            hasPressedLeftClick = false;
            yield return new WaitUntil(() => hasPressedLeftClick); // Wait until Left Click is pressed
        }

        string[] tags = { "Enemy" }; // PLACEHOLDER UNTIL A BETTER SOLUTION IS FOUND
        LayerMask enemyMask = LayerMask.GetMask("Enemy");

        // Get target and check that it's valid
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()),
            Vector2.zero, Mathf.Infinity, enemyMask);
        if (hit.collider != null)
        {
            Transform selection = hit.transform;

            if (tags.Contains(selection.tag)) // Check if its the target we want.
            {
                Unit selectedUnit = hit.collider.GetComponent<Unit>();

                // Send Unit target as event param
                object target = selectedUnit;
                unitEventHandler.RaiseEvent("OnAbilityInputSet", target);

                Debug.Log($"{selectedUnit} was selected");
            }
            else
            {
                Debug.Log($"Raycast hit {selection.name}, but it's not a valid target");
            }
        }
        else
        {
            Debug.Log("Raycast hit nothing! No valid Unit selected");
        }

        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto); // Change cursor back to default
    }

    public static IEnumerator AOETargetInput(Ability ability, EventProcessor unitEventHandler)
    {
        if (ability.quickCast)
        {
            hasPressedLeftClick = true;
        }
        else
        {
            ChangeCursor();

            hasPressedLeftClick = false;
            yield return new WaitUntil(() => hasPressedLeftClick); // Wait until Left Click is pressed
        }

        // (TODO) Check if the target is valid (is above terrain for example)

        // Calculate mouse position
        Vector3 centerPoint = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        // Send Vector3 centerCoordinates as event param
        object centerCoordinates = centerPoint;
        unitEventHandler.RaiseEvent("OnAbilityInputSet", centerCoordinates);

        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto); // Change cursor back to default
    }

    private static void ChangeCursor()
    {
        Texture2D cursorTexture = (Texture2D)Resources.Load("AbilityCursor");
        Vector2 hotSpot = new Vector2(24, 24); // The offset from the top left of the cursor to use as the target point
        Cursor.SetCursor(cursorTexture, hotSpot, CursorMode.Auto);
    }
}

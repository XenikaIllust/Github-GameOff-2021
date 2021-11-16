using System.Linq;
using UnityEngine;

static class AbilityInputType
{
	public static bool PointTargetInput()
	{
		// (TODO) Check if the target is valid (is above terrain for example)
		
		// Calculate mouse position
		Vector3 targetPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);

		// Send Vector3 targetCoordinates as event param
		object targetCoordinates = targetPoint;
		EventManager.RaiseEvent("OnAbilityInputSet", targetCoordinates);

		// Change cursor back to default
		Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);

		Debug.Log("Skill Shot at " + targetPoint);
		return true;
	}

	public static bool UnitTargetInput(/* string[] tags */)
	{
		string[] tags = { "Enemy" }; // PLACEHOLDER UNTIL A BETTER SOLUTION IS FOUND
		LayerMask enemyMask = LayerMask.GetMask("Enemy");

		// (TODO) Change cursor to selection cursor
		// Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);

		// Get target and check that it's valid
		RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), direction: Vector2.zero, distance: Mathf.Infinity, layerMask: enemyMask);
		if (hit.collider != null) 
		{
			Transform selection = hit.transform;
			if (tags.Contains(selection.tag)) // Check if its the target we want.
			{
				Unit selectedUnit = hit.collider.GetComponent<Unit>();
				Debug.Log(selection.gameObject.name);
				// Send Unit target as event param
				object target = selectedUnit;
				EventManager.RaiseEvent("OnAbilityInputSet", target);

				Debug.Log(selectedUnit + " was selected");
			}
			else Debug.Log("Raycast hit " + selection.name + ", but it's not a valid target");
		}
		else Debug.Log("Raycast hit nothing! No valid Unit selected");

		// Change cursor back to default
		Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);

		return true;
	}

	public static bool AOETargetInput(/*float radius*/)
	{
		float radius = 1.0f; // PLACEHOLDER UNTIL A BETTER SOLUTION IS FOUND

		// (TODO) Change cursor to selection cursor
		// Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);

		// (TODO) Check if the target is valid (is above terrain for example)

		// Calculate mouse position
		Vector3 centerPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);

		// Send Vector3 centerCoordinates as event param
		object centerCoordinates = centerPoint;
		EventManager.RaiseEvent("OnAbilityInputSet", centerCoordinates);

		// Change cursor back to default
		Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);

		Debug.Log("AOE fired at " + centerPoint + " with a radius of " + radius);
		return true;
	}

	public static bool NoTargetInput()
	{
		return true;
	}
}

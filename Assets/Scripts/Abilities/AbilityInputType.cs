using UnityEngine;

static class AbilityInputType
{
	// static private Texture2D cursorTexture = null;
	// static private Texture2D AreaTexture = null;

	public static bool PointTargetInput()
	{
		// Change cursor to selection cursor
		// Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);

		// (TODO) Check if the target is valid (is above terrain for example)
		
		// Calculate mouse position
		Vector2 screenPosition = Input.mousePosition;
		Vector3 targetPoint = Camera.main.ScreenToWorldPoint(screenPosition);

		// Send Vector3 targetCoordinates as event param
		object targetCoordinates = targetPoint;
		EventManager.RaiseEvent("OnAbilityInputSet", targetCoordinates);

		// Change cursor back to default
		// Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);

		Debug.Log("Skill Shot at " + targetPoint);
		return true;
	}

	public static bool UnitTargetInput()
	{
		// (TODO) Change cursor to selection cursor
		// Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);

		// (TODO) Get target and check that it's not null
		Vector2 screenPosition = Input.mousePosition;
		Unit selectedTarget = new Unit();

		// Send Unit target as event param
		object target = selectedTarget;
		EventManager.RaiseEvent("OnAbilityInputSet", target);

		// Change cursor back to default
		// Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);

		Debug.Log(selectedTarget + " was selected.");
		return true;
	}

	public static bool AOETargetInput(/*float radius*/)
	{
		float radius = 1.0f; // PLACEHOLDER UNTIL I FIND A BETTER SOLUTION

		// Change cursor to selection cursor
		// Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);

		// (TODO) Check if the target is valid (is above terrain for example)

		// Calculate mouse position
		Vector2 screenPosition = Input.mousePosition;
		Vector3 centerPoint = Camera.main.ScreenToWorldPoint(screenPosition);

		// Send Vector3 centerCoordinates as event param
		object centerCoordinates = centerPoint;
		EventManager.RaiseEvent("OnAbilityInputSet", centerCoordinates);

		// Change cursor back to default
		// Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);

		Debug.Log("AOE fired at " + centerPoint + " with a radius of " + radius);
		return true;
	}

	public static bool NoTargetInput()
	{
		return true;
	}
}

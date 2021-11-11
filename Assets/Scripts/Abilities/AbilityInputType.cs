using UnityEngine;

static class AbilityInputType
{
	public static bool PointTargetInput()
	{
		// (TODO) Change cursor to selection cursor

		// (TODO) Check if the target is valid (is above terrain for example)
		
		// Calculate mouse position
		Vector2 screenPosition = Input.mousePosition;
		Vector3 targetPoint = Camera.main.ScreenToWorldPoint(screenPosition);

		// Send Vector3 targetCoordinates as event param
		object targetCoordinates = targetPoint;
		EventManager.RaiseEvent("OnAbilityInputSet", targetCoordinates);

		// (TODO) Change cursor back to default

		Debug.Log("Skill Shot at " + targetPoint);
		return true;
	}

	public static bool UnitTargetInput()
	{
		// (TODO) Change cursor to selection cursor

		// (TODO) Get target and check that it's not null

		// (TODO) Send Unit target as event param

		// (TODO) Change cursor back to default
		Debug.Log("UnitTargetInput");
		return true;
	}

	public static bool AOETargetInput(/*float radius*/)
	{
		// (TODO) Change cursor to selection cursor

		// (TODO) Check if you are above valid terrain

		// (TODO) Calculate mouse position

		// (TODO) Send Vector3 centerCoordinates as event param

		// (TODO) Change cursor back to default
		Debug.Log("AOETargetInput");
		return true;
	}

	public static bool NoTargetInput()
	{
		return true;
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Definitions/Target Filter")]
public class TargetFilter : ScriptableObject
{
	// Target filters work by taking a set of potential targets and then by applying
	// a series of filters in turn to narrow down your targets until you're left
	// with who you want to target.

	[System.Flags] // https://docs.microsoft.com/en-us/dotnet/api/system.flagsattribute?view=netframework-4.8  
	public enum TargetRelationship
	{
		Hostile = 1 << 0,
		Neutral = 1 << 1,
		Friendly = 1 << 2,
	}

	public enum SelfInclusion
	{
		ByFilter,
		Always,
		Never,
	}
	// It's pretty common to make an exception of yourself, e.g. to target allies excluding yourself

	[Tooltip("Initial set(s) of characters to filter on based on their relationship to the ability executor")]
	public TargetRelationship Relationship;
	[Tooltip("Should we exclude the character executing the ability, always include them, or just leave it up to the selection filters?")]
	public SelfInclusion IncludeSelf;

	[Tooltip("Each filter is applied in turn")]
	public SelectionFilter[] SelectionFilters;
}

[System.Serializable]
public class SelectionFilter
{
	public enum FilterType
	{
		InRange,
		Random,
		Closest,
	}
	// Potential Additional Selection Types
	// Position Based: "In Arc", "Angular Closest", "In Line of Sight"
	// Character State Based:  "Has Status Effect", "Health Percentage"

	public FilterType Type;
	[Header("Type - Range")]
	[Tooltip("The range to be within")]
	public float Range;
	[Tooltip("Offset in local coordinates")]
	public Vector3 Offset;
	[Header("Type - Random & Closest")]
	public int Count = 1;
}

// Example: Closest hostile character that in within 5 units of a point 3 units in front of the ability executor.

// Relationship: Hostile
// IncludeSelf: ByFilter
// SelectionFilters[]
// Element 0:
//  Type: InRange
//  Range: 5
//  Offset: 0, 0, 3
// Element 1:
//  Type: Closest
//  Count: 1

using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    // Statistics
    public int totalRuns;

    // Current Progress
    public int currentStage;
    public int currentBattle;

    // Player's State
    public int health;
    public List<Ability> equippedAbilities;

    public string ToJson()
    {
        return JsonUtility.ToJson(this);
    }

    public void LoadFromJson(string saveFileContents)
    {
        JsonUtility.FromJsonOverwrite(saveFileContents, this);
    }
}

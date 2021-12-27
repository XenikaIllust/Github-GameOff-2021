using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameMode {
    SNEAKING,
    COMBAT,
    BOSS
}

[System.Serializable]
[CreateAssetMenu(menuName = "SectorMap/MapNode")]
public class MapNode : ScriptableObject
{
    public List<MapNode> next = new List<MapNode>();
    public GameMode gameMode;
    // GameModeProperties gameModeProperties;

}

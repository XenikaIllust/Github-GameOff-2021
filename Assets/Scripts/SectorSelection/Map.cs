using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "SectorMap/Map")]
public class Map : ScriptableObject
{
    public MapNode startNode;
    public MapNode endNode;

    bool isRandomlyGenerated;

    public List<MapNode> firstLevelNodes = new List<MapNode>();
    public List<MapNode> secondLevelNodes = new List<MapNode>();
    public List<MapNode> thirdLevelNodes = new List<MapNode>();
    public List<MapNode> fourthLevelNodes = new List<MapNode>();
    public List<MapNode> fifthLevelNodes = new List<MapNode>();
}

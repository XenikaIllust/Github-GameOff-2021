using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public class Tiles : MonoBehaviour
{
    public Tile tile;
    public Tile obstacle;
    public Tiles[] tiles;

    public Tilemap groundBaseTileMap;
    public Tilemap groundDetailTileMap;

    public Tilemap leve1TileMap;
    public Tilemap leve1DetailTileMap;

    public Tilemap collisionTileMap;

    private Vector3Int tilePosition = new Vector3Int(0, 0, 0);
    private int tileOffset = 1;

    public int width, height;
    public GameObject block;

    void Start()
    {
        GenerateMap();
    }

    private void GenerateMapForTilemap(Tilemap map, Tile tile, Vector3Int position)
    {
        map.SetTile(position, tile);
    }

    private void GenerateSingleLayerMap(Tiles usingTile)
    {
        GenerateMapForTilemap(groundBaseTileMap, usingTile.tile, tilePosition);//1

        if (usingTile.obstacle != null)//2
        {
            Vector3Int tilePositionObs = tilePosition;//3
            GenerateMapForTilemap(groundDetailTileMap, usingTile.obstacle, tilePositionObs);
            GenerateMapForTilemap(collisionTileMap, usingTile.obstacle, tilePositionObs);
        }
    }

    public void GenerateMap()
    {
        if (tiles.Length < 1)
        {
            Debug.Log("No Tiles");
            return;
        }
        foreach (var item in tiles)
        {
            for (int i = 0; i < width; i++)
            {
                tilePosition = new Vector3Int(tilePosition.x + tileOffset, tilePosition.y, tilePosition.z);//add offset
                GenerateSingleLayerMap(item);
            }
        }
    }
}

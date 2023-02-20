using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTileObject", menuName = "Map/TileObject")]
public class TileObject : ScriptableObject
{
    public enum TileType { Decor, Obstacle }

    public bool passable = true;
    public int width;
    public int height;
    public int movementCost = 0;
    public string nameString;
    public Transform prefab;
    public TileType tileType;

    public List<Vector2Int> GetGridPositionList(int x, int z)
    {
        List<Vector2Int> positionList = new List<Vector2Int>();
        for (int i = 0; i < width; i++)
            for (int j = 0; j < height; j++)
                positionList.Add(new Vector2Int(x + i, z + j));
        return positionList;
    }
}

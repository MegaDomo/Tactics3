using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTileObject", menuName = "Map/Block")]
public class TileObject : ScriptableObject
{
    public bool passable;
    public int width;
    public int height;
    public int movementCost;
    public string nameString;
    public Transform prefab;

    public List<Vector3Int> GetGridPositionList(Vector2Int offset)
    {
        return new List<Vector3Int>();
    }
}

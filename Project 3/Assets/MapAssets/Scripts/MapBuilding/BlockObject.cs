using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBlockObject", menuName = "Map/Block")]
public class BlockObject : ScriptableObject
{
    public enum BlockType { Neutral, Difficult, Cluster, Line}

    public bool passable = true;
    public int width;
    public int height;
    public int movementCost;
    public string nameString;
    public Transform prefab;
    public BlockType blockType;






















    public static Dir GetNextDir(Dir dir)
    {
        switch (dir)
        {
            default:
            case Dir.Down:
                return Dir.Left;
            case Dir.Left:
                return Dir.Up;
            case Dir.Up:
                return Dir.Right;
            case Dir.Right:
                return Dir.Down;
        }

    }

    public enum Dir
    {
        Up,
        Right,
        Down,
        Left
    }

    public int GetRotationAngle(Dir dir)
    {
        switch (dir)
        {
            default:
            case Dir.Down:
                return 0;
            case Dir.Left:
                return 90;
            case Dir.Up:
                return 180;
            case Dir.Right:
                return 270;
        }
    }

    public List<Vector2Int> GetGridPositionList(Vector2Int offset, Dir dir)
    {
        List<Vector2Int> positionList = new List<Vector2Int>();
        switch (dir)
        {
            default:
            case Dir.Down:
            case Dir.Up:
                for (int x = 0; x < width; x++)
                {
                    for (int z = 0; z < height; z++)
                    {
                        positionList.Add(offset + new Vector2Int(x, z));
                    }
                }
                break;
            case Dir.Left:
            case Dir.Right:
                for (int x = 0; x < height; x++)
                {
                    for (int z = 0; z < width; z++)
                    {
                        positionList.Add(offset + new Vector2Int(x, z));
                    }
                }
                break;
        }
        return positionList;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Data Structure
public class Grid<T>
{
    private int width;
    private int height;
    private int length;
    private float cellSize;
    private Vector3 origin;
    private T[,] gridArray;

    // Constructor
    public Grid(int size, int cellSize, Vector3 origin, Func<T> createGridObject)
    {
        width = size;
        height = size;
        this.origin = origin;
        this.cellSize = cellSize;

        gridArray = new T[width, height];

        for (int i = 0; i < width; i++)
            for (int j = 0; j < height; j++)
                gridArray[i, j] = createGridObject();

        bool debug = false;
        if (debug)
        {
            // Shows Grid in Space
            for (int x = 0; x < gridArray.GetLength(0); x++)
            {
                for (int z = 0; z < gridArray.GetLength(1); z++)
                {
                    Utils.CreateWorldText(GetWorldPosition(x, z) + new Vector3(cellSize, 0, cellSize) * 0.5f, x.ToString() + ", " + z.ToString(), 30, TextAnchor.MiddleCenter);
                    Debug.DrawLine(GetWorldPosition(x, z), GetWorldPosition(x, z + 1), Color.white, 100f);
                    Debug.DrawLine(GetWorldPosition(x, z), GetWorldPosition(x + 1, z), Color.white, 100f);
                }
            }
            Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);
            Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);
        }        
    }

    public T GetGridObject(int x, int z)
    {
        if (!isCoordinatesSafe(x, z))
            return default(T);

        return gridArray[x, z];
    }

    public Vector3 GetWorldPosition(int x, int z)
    {
        return new Vector3(x, 0, z) * cellSize;
    }

    public Vector3 GetWorldPosition(Node node)
    {
        return new Vector3(node.x, 0, node.z) * cellSize;
    }

    public Vector3 GetWorldPosition(int x, int y, int z)
    {
        return new Vector3(x, y, z) * cellSize;
    }

    public void GetXZ(Vector3 worldPosition, out int x, out int z)
    {
        x = Mathf.FloorToInt(worldPosition.x / cellSize);
        z = Mathf.FloorToInt(worldPosition.z / cellSize);
    }

    public void GetXYZ(Vector3 worldPosition, out int x, out int y, out int z)
    {
        x = Mathf.FloorToInt(worldPosition.x / cellSize);
        y = Mathf.FloorToInt(worldPosition.y / cellSize);
        z = Mathf.FloorToInt(worldPosition.z / cellSize);
    }

    public int GetSize()
    {
        return width;
    }

    public int GetWidth()
    {
        return width;
    }

    public int GetHeight()
    {
        return height;
    }

    public int GetLength()
    {
        return length;
    }

    public float GetCellSize()
    {
        return cellSize;
    }

    public void SetGridObject(Vector3 worldPosition, T newGridObject)
    {
        int x, z;
        GetXZ(worldPosition, out x, out z);
        SetGridObject(x, z, newGridObject);
    }

    public void SetGridObject(int x, int z, T newGridObject)
    {
        if (!isCoordinatesSafe(x, z))
            return;
        gridArray[x, z] = newGridObject;
    }

    public bool isCoordinatesSafe(int x, int z)
    {
        if (x < 0 || z < 0 || x >= width || z >= height)
            return false;

        return true;
    }
}

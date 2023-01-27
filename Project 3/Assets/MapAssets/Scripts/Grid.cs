using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid 
{
    private int width;
    private int height;
    private float cellSize;
    private Vector3 origin;
    private Node[,] gridArray;

    public Grid(int size, int cellSize, Vector3 origin)
    {
        width = size;
        height = size;
        this.origin = origin;
        this.cellSize = cellSize;

        gridArray = new Node[width, height];

        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int z = 0; z < gridArray.GetLength(1); z++)
            {
                Utils.CreateWorldText(new Vector3(0, 0, 0), gridArray[x, z].x.ToString() + ", " + gridArray[x, z].x.ToString(), 30);
            }
        }
    }

    public void AddBranchToRoot(Node branch, Node root)
    {
        root.AddEdge(branch);
    }
    public void AddBranchToRoot(int x, int z, Node branch)
    {
        gridArray[x, z].AddEdge(branch);
    }

    public Node GetNode(int x, int z)
    {
        return gridArray[x, z];
    }

    public int GetSize()
    {
        return width;
    }

    public Vector3 GetWorldPosition(int x, int z)
    {
        return new Vector3(x, 0, z) * cellSize;
    }

    public void SetNode(int x, int z, Node newNode)
    {
        if (!isCoordinatesSafe(x, z))
            return;
        gridArray[x, z] = newNode;
    }


    private bool isCoordinatesSafe(int x, int z)
    {
        if (x < 0 || z < 0 || x >= width || z >= height)
            return false;

        return true;
    }
}

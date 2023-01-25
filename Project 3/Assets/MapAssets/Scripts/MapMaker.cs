using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapMaker : MonoBehaviour
{
    [Header("Unity References")]
    public List<GameObject> nodes;
    public Transform startingPoint;
    public MapManager manager;

    [Header("Attributes")]
    [SerializeField] private Vector3 offset;
    [SerializeField] private int minMapSize;
    [SerializeField] private int maxMapSize;

    [HideInInspector] public Node[][] map;
    [HideInInspector] public int[][] graph;
    [HideInInspector] public List<Node> adj = new List<Node>();

    private int size;

    public void Setup()
    {
        // Initializers
        GetMapSize();
        CreateMap();
        CreateGraph();
        PlaceUnits();

        // When Finishes, Performs Handoff
        manager.map = map;
    }
    
    #region Create Map
    // Methods that Picks a random size for the map
    private void GetMapSize()
    {
        // Uses Random.Range to get a Random number
        size = Random.Range(minMapSize, maxMapSize);

        // Initializes the map
        map = new Node[size][];
        for (int i = 0; i < map.Length; i++)
            map[i] = new Node[size];

        // Initializes the Graph
        graph = new int[size * size][];
        for (int i = 0; i < graph.Length; i++)
            graph[i] = new int[size * size];
    }

    // Creates the Map
    private void CreateMap()
    {
        // Starting Point
        Vector3 rowPoint = startingPoint.position;

        // Creates Each Row
        for (int i = 0; i < map.Length; i++)
        {
            CreateRow(rowPoint, i);
            rowPoint += new Vector3(offset.z, 0, 0);
        }
    }

    private void CreateRow(Vector3 spawn, int row)
    {
        for (int i = 0; i < map.Length; i++)
        {
            GameObject temp;
            int z = Random.Range(0, 5);

            // Obstacle 20%
            if (z == 0)
                temp = nodes[1];
            // Swamp 40%
            else if (z == 1 || z == 2)
                temp = nodes[2];
            // Plains 40%
            else
                temp = nodes[0];

            // Methods That will determine which Node Prefab to make

            // Create the Node
            Node clone = Instantiate(temp, spawn, Quaternion.identity).GetComponent<Node>();
            spawn += offset;

            int x, y;

            x = row;
            y = i;

            // Store the Coordinantes
            StoreNodeInCoordinate(x, y, clone);
        }
    }

    // Stores the Node on the Coordinate Map
    private void StoreNodeInCoordinate(int x, int y, Node node)
    {
        map[x][y] = node;
    }
    #endregion

    #region Create Graph
    // Creates the Graph
    private void CreateGraph()
    {
        CreateEdgesInNodes();
        CreateAdjacencyMatrix();
    }

    private void CreateEdgesInNodes()
    {
        for (int i = 0; i < map.Length; i++)
        {
            for (int j = 0; j < map[i].Length; j++)
            {
                // Up
                if (CanAdd(i + 1, j))
                {
                    map[i][j].AddEdge(map[i + 1][j]);
                }
                // Right
                if (CanAdd(i, j + 1))
                {
                    map[i][j].AddEdge(map[i][j + 1]);
                }
                // Down
                if (CanAdd(i - 1, j))
                {
                    map[i][j].AddEdge(map[i - 1][j]);
                }
                // Left
                if (CanAdd(i, j - 1))
                {
                    map[i][j].AddEdge(map[i][j - 1]);
                }
            }
        }
    }

    private void CreateAdjacencyMatrix()
    {
        // Rows of the Map
        for (int i = 0; i < map.Length; i++)
        {
            // Columns of the Map
            for (int j = 0; j < map[i].Length; j++)
            {
                adj.Add(map[i][j]);
            }
        }
    }

    private bool CanAdd(int x, int y)
    {
        // Out of Bounds
        if (x < 0 || y < 0 || x >= size || y >= size)
            return false;
        
        return true;
    }

    private void PrintGraph()
    {
        string matrix = "";
        for (int i = 0; i < graph.Length; i++)
        {
            for (int j = 0; j < graph[i].Length; j++)
            {
                matrix += graph[i][j].ToString() + ", ";
            }
            matrix += "\n";
        }
        Debug.Log(matrix);
    }
    #endregion

    // Put the Units in the spawn Positions
    private void PlaceUnits()
    {
        for (int i = 0; i < BattleSystem.instance.units.Count; i++)
        {
            MapManager.instance.Place(BattleSystem.instance.units[i], map[i][i]);
        }
        
    }

    public Vector3 GetCoordinates(Node node)
    {
        // Base Case : No Data
        if (node == null)
            return new Vector3(-1, 0, -1);

        // Traversal
        for (int i = 0; i < map.Length; i++)
        {
            for (int j = 0; j < map[i].Length; j++)
            {
                // Compare
                if (map[i][j] == node)
                    return new Vector3(i, 0, j);
            }
        }

        // Base Case : Out of Bounds
        return new Vector3(-1, 0, -1);
    }

}

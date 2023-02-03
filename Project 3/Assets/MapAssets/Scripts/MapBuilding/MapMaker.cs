using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapMaker : MonoBehaviour
{
    [Header("Unity References")]
    public List<TileObject> tileObjects;
    public GameObject forecastTile;
    public Transform startingPoint;
    public MapManager manager;

    [Header("Attributes")]
    [SerializeField] private int cellSize;
    [SerializeField] private int minMapSize;
    [SerializeField] private int maxMapSize;

    [HideInInspector] public Grid<Node> map;

    private bool makeRandomMap;
    private int size;

    public void SetUp(bool makeRandomMap)
    {
        this.makeRandomMap = makeRandomMap;

        // Initializers
        size = Random.Range(minMapSize, maxMapSize);
        map = new Grid<Node>(size, cellSize, startingPoint.position, () => new Node());

        for (int i = 0; i < map.GetSize(); i++)
            for (int j = 0; j < map.GetSize(); j++)
                map.GetGridObject(i, j).SetCoordinates(i, j);

        if (makeRandomMap)
            CreateMap();

        if (!makeRandomMap)
            GetMap();

        // When Finishes, Performs Handoff
        manager.map = map;
    }
    
    #region Create Map
    // Methods that Picks a random size for the map
    private void CreateMap()
    {
        for (int i = 0; i < map.GetSize(); i++)
        {
            for (int j = 0; j < map.GetSize(); j++)
            {
                TileObject temp;
                int z = Random.Range(0, 5);

                // Obstacle 20%
                if (z == 0)
                    temp = tileObjects[2];
                // Swamp 40%
                else if (z == 1 || z == 2)
                    temp = tileObjects[0];
                // Plains 40%
                else
                    temp = tileObjects[1];


                Transform block = Instantiate(temp.prefab, map.GetWorldPosition(i, j), Quaternion.identity);
                ForecastTile tile = Instantiate(forecastTile, map.GetWorldPosition(i, j), Quaternion.identity).GetComponent<ForecastTile>();
                StoreDataInGrid(i, j, block, temp, tile);
            }
        }
    }

    private void StoreDataInGrid(int x, int z, Transform block, TileObject tileObject, ForecastTile tile)
    {
        Node node = new Node(x, z, map);
        node.SetTileObject(block, tileObject);
        node.SetForecastTile(tile);
        map.SetGridObject(x, z, node);
    }
    #endregion

    #region Get Map
    private void GetMap()
    {
        Tilemap tilemap = GameObject.FindGameObjectWithTag("Map").GetComponent<Tilemap>();
        
        for (int i = 0; i < map.GetSize(); i++)
        {
            for (int j = 0; j < map.GetSize(); j++)
            {
                // Tile Map uses x and y whereas our Grid<T> uses x and z
                Vector3Int vec = FindPositionFromTileMap(i, j);
                GameObject clone = tilemap.GetInstantiatedObject(vec);
                Node node = CreateNode(i, j, clone);
                map.SetGridObject(i, j, node);
            }
        }
    }

    private Vector3Int FindPositionFromTileMap(int x, int z)
    {
        Vector3 temp = map.GetWorldPosition(x, z);
        Vector3Int vec = new Vector3Int((int)temp.x + 5, (int)temp.z + 5);        
        return vec;
    }

    private Node CreateNode(int x, int z, GameObject clone)
    {
        Node node = new Node(x, z, map);
        
        TileObject tileObject = clone.GetComponent<Block>().tileObject;
        node.SetTileObject(clone.transform, tileObject);

        ForecastTile tile = Instantiate(forecastTile, map.GetWorldPosition(x, z), Quaternion.identity).GetComponent<ForecastTile>();
        node.SetForecastTile(tile);
        return node;
    }
    #endregion

    #region Utility Methods
    public Vector3 GetCoordinates(Node node)
    {
        // Base Case : No Data
        if (node == null)
            return new Vector3(-1, 0, -1);

        // Traversal
        for (int i = 0; i < map.GetSize(); i++)
        {
            for (int j = 0; j < map.GetSize(); j++)
            {
                // Compare
                if (map.GetGridObject(i, j) == node)
                    return new Vector3(i, 0, j);
            }
        }

        // Base Case : Out of Bounds
        return new Vector3(-1, 0, -1);
    }
    #endregion
}
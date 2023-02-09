using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapMaker : MonoBehaviour
{
    [Header("Unity References")]
    public List<BlockObject> blockObjects;
    public GameObject forecastTile;
    public Transform startingPoint;
    public MapManager manager;

    [Header("Attributes")]
    [SerializeField] private int stepSize = 5;
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
        manager.GetMapData(map, stepSize);
    }
    
    #region Create Map
    // Methods that Picks a random size for the map
    private void CreateMap()
    {
        for (int i = 0; i < map.GetSize(); i++)
        {
            for (int j = 0; j < map.GetSize(); j++)
            {
                BlockObject blockObject = GetRandomBlockObject();
                Transform block = Instantiate(blockObject.prefab, map.GetWorldPosition(i, j), Quaternion.identity);
                ForecastTile tile = Instantiate(forecastTile, map.GetWorldPosition(i, j), Quaternion.identity).GetComponent<ForecastTile>();
                StoreDataInGrid(i, j, block, blockObject, tile);
            }
        }
    }

    private void StoreDataInGrid(int x, int z, Transform block, BlockObject blockObject, ForecastTile tile)
    {
        Node node = new Node(x, z, map);
        node.SetBlockObject(block, blockObject);
        node.SetForecastTile(tile);
        map.SetGridObject(x, z, node);
    }

    private BlockObject GetRandomBlockObject()
    {
        BlockObject temp;
        int z = Random.Range(0, 5);

        // Obstacle 20%
        if (z == 0)
            temp = blockObjects[2];
        // Swamp 40%
        else if (z == 1 || z == 2)
            temp = blockObjects[0];
        // Plains 40%
        else
            temp = blockObjects[1];

        return temp;
    }
    #endregion

    #region Get Map
    private void GetMap()
    {
        Transform layer1 = GameObject.FindGameObjectWithTag("Ground").transform;
        Transform layer2 = GameObject.FindGameObjectWithTag("ObstacleDecor").transform;

        GetGroundLayer(layer1);
        GetObstacleLayer(layer2);
    }

    private void GetGroundLayer(Transform tilemap)
    {
        for (int x = 0; x < map.GetWidth(); x++)
        {
            for (int z = 0; z < map.GetHeight(); z++)
            {
                RaycastHit hit = GetRaycastData(x, z, LayerMask.GetMask("Ground"));
                if (hit.collider == null)
                    continue;
                CreateNode(x, z, hit);
            }
        }
    }

    private void CreateNode(int x, int z, RaycastHit hit)
    {
        // Coordinates
        int y = (int)hit.transform.position.y;
        Node node = new Node(x, y, z, map);

        // Block Object
        Transform block = hit.transform;        
        BlockObject blockObject = block.GetComponent<Block>().blockObject;
        node.SetBlockObject(block, blockObject);

        // Forecast Object
        float y2 = node.GetStandingPoint().y;
        Vector3 spawnPoint = map.GetWorldPosition(x, z) + new Vector3(0, y2, 0);
        ForecastTile tile = Instantiate(forecastTile, spawnPoint, Quaternion.identity).GetComponent<ForecastTile>();
        node.SetForecastTile(tile);

        map.SetGridObject(x, z, node);
    }

    private void GetObstacleLayer(Transform tilemap)
    {
        for (int x = 0; x < map.GetWidth(); x++)
        {
            for (int z = 0; z < map.GetHeight(); z++)
            {
                RaycastHit hit = GetRaycastData(x, z, LayerMask.GetMask("Obstacle"));
                if (hit.collider == null)
                        continue;
                AmendNode(x, z, hit);
            }
        }
    }

    private void AmendNode(int x, int z, RaycastHit hit)
    {
        Node nodeToUpdate = map.GetGridObject(x, z);
        TileObject tileObject = hit.collider.gameObject.GetComponent<Tile>().tileObject;
        nodeToUpdate.SetTileObject(tileObject);
    }

    private RaycastHit GetRaycastData(int x, int z, LayerMask mask)
    {
        Vector3 origin = map.GetWorldPosition(x, z) + new Vector3(map.GetCellSize(), -50, map.GetCellSize()) * 0.5f;
        Ray ray = new Ray(origin, Vector3.up);
        Physics.Raycast(ray, out RaycastHit hit, 1000f, mask);
        return hit;
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
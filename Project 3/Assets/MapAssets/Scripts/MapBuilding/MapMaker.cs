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


                Transform block = Instantiate(temp.prefab, map.GetWorldPosition(i, j), Quaternion.identity);
                ForecastTile tile = Instantiate(forecastTile, map.GetWorldPosition(i, j), Quaternion.identity).GetComponent<ForecastTile>();
                StoreDataInGrid(i, j, block, temp, tile);
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
        for (int i = 0; i < tilemap.childCount; i++)
        {
            map.GetXZ(tilemap.GetChild(i).position, out int x, out int z);
            Node node = CreateNode(x, z, tilemap.GetChild(i).gameObject);
            map.SetGridObject(x, z, node);
        }
    }

    private Node CreateNode(int x, int z, GameObject clone)
    {
        Node node = new Node(x, z, map);

        BlockObject blockObject = clone.GetComponent<Block>().blockObject;
        node.SetBlockObject(clone.transform, blockObject);

        ForecastTile tile = Instantiate(forecastTile, map.GetWorldPosition(x, z), Quaternion.identity).GetComponent<ForecastTile>();
        node.SetForecastTile(tile);
        return node;
    }

    private void GetObstacleLayer(Transform tilemap)
    {
        for (int x = 0; x < map.GetWidth(); x++)
        {
            for (int z = 0; z < map.GetHeight(); z++)
            {
                Vector3 origin = map.GetWorldPosition(x, z) + new Vector3(map.GetCellSize(), -5, map.GetCellSize()) * 0.5f;
                Ray ray = new Ray(origin, Vector3.up);
                Physics.Raycast(ray, out RaycastHit hit);

                if (hit.collider == null)
                    continue;
                if (hit.transform.gameObject.layer != LayerMask.GetMask("Obstacle"))
                    continue;

                Tile tile = hit.collider.GetComponent<Tile>();
                TileObject tileObject = tile.tileObject;
                if (tileObject.isDecor)
                    continue;

                Node nodeToUpdate = map.GetGridObject((int)hit.transform.position.x, (int)hit.transform.position.z);
                nodeToUpdate.SetTileObject(tileObject);
            }
        }
    }

    private void AmendNode(Node node, TileObject tileObject)
    {
        node.SetTileObject(tileObject);
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
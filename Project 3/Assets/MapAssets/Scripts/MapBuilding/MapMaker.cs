using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapMaker : MonoBehaviour
{
    [Header("Testing")]
    public List<TerrainKit> terrains;
    public int lowSaturationPercentage;
    public int mediumSaturationPercentage;
    public int highSaturationPercentage;
    public string whichTerrain;

    [Header("Unity References")]
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
    private List<int> saturation = new List<int>();

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
        {
            saturation.Add(lowSaturationPercentage);
            saturation.Add(mediumSaturationPercentage);
            saturation.Add(highSaturationPercentage);
            MapGeneration gen = new MapGeneration(whichTerrain, map, terrains, forecastTile, saturation);
            //CreateMap();
        }
            

        if (!makeRandomMap)
            GetMap();

        // When Finishes, Performs Handoff
        manager.SetMapData(map, stepSize);
    }

    #region Get Map
    private void GetMap()
    {
        GetGroundLayer();
        GetObstacleLayer();
    }

    private void GetGroundLayer()
    {
        bool debug = false;
        if (debug)
        {
            for (int x = 0; x < map.GetWidth(); x++)
            {
                for (int z = 0; z < map.GetHeight(); z++)
                {
                    Vector3 position = map.GetWorldPosition(x, z);
                    Debug.DrawLine(position, position + new Vector3(0, 100, 0), Color.red, 100f);
                }
            }
        }

        for (int x = 0; x < map.GetWidth(); x++)
        {
            for (int z = 0; z < map.GetHeight(); z++)
            {
                RaycastHit[] hits = GetRaycastData(x, z, LayerMask.GetMask("Ground"));
                if (hits.Length == 0)
                    continue;
                CreateNode(x, z, hits);
            }
        }
    }

    private void CreateNode(int x, int z, RaycastHit[] hits)
    {
        RaycastHit hit = hits[0];
        
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

    private void GetObstacleLayer()
    {
        bool debug = true;
        if (debug)
        {
            for (int x = 0; x < map.GetWidth(); x++)
            {
                for (int z = 0; z < map.GetHeight(); z++)
                {
                    Vector3 position = map.GetWorldPosition(x, z);
                    Debug.DrawLine(position, position + new Vector3(0, 100, 0), Color.red, 100f);
                }
            }
        }

        for (int x = 0; x < map.GetWidth(); x++)
        {
            for (int z = 0; z < map.GetHeight(); z++)
            {
                RaycastHit[] hits = GetRaycastData(x, z, LayerMask.GetMask("Obstacle"));
                if (hits.Length == 0)
                        continue;
                Debug.Log("Hit something apparently" + " : " + hits.Length);
                Debug.Log(hits[0].transform.gameObject.name + " : " + hits[1].transform.gameObject.name);
                AmendNodeWithTileObject(x, z, hits);
            }
        }
    }

    private void AmendNodeWithTileObject(int x, int z, RaycastHit[] hits)
    {
        RaycastHit hit = hits[0];
        if (hit.collider == null) return;
        Node nodeToUpdate = map.GetGridObject(x, z);
        TileObject tileObject = hit.collider.gameObject.GetComponent<Tile>().tileObject;
        Transform tile = hit.transform;
        nodeToUpdate.SetTileObject(tile, tileObject);
    }

    private RaycastHit[] GetRaycastData(int x, int z, LayerMask mask)
    {
        Vector3 origin = map.GetWorldPosition(x, z) + new Vector3(0, -50, 0);
        Ray ray = new Ray(origin, Vector3.up);
        
        RaycastHit[] hits = Physics.RaycastAll(ray, 1000f, mask);
        return hits;
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
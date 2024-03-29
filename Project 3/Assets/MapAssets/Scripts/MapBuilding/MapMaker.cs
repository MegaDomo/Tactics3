using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Utility ScriptableObject
[CreateAssetMenu(fileName = "NewMapMaker", menuName = "Managers/Map Maker")]
public class MapMaker : ScriptableObject
{
    [Header("Testing")]
    public List<TerrainKit> terrains;
    public int lowSaturationPercentage;
    public int mediumSaturationPercentage;
    public int highSaturationPercentage;
    public string whichTerrain;

    [Header("ScriptableObject References")]
    public GameMaster gameMaster;

    [Header("Unity References")]
    public GameObject forecastTile;

    [Header("Attributes")]
    [SerializeField] private int stepSize = 5;
    [SerializeField] private int cellSize;
    [SerializeField] private int minMapSize;
    [SerializeField] private int maxMapSize;

    private Grid<Node> map;
    private List<Unit> units = new List<Unit>();

    public Action<Grid<Node>, List<Unit>> mapMadeEvent;

    private int size;
    private List<int> saturation = new List<int>();

    public void OnEnable()
    {
        gameMaster.makeMapEvent += SetUp;
    }

    private void SetUp(Transform startingPoint, bool makeRandomMap)
    {
        // Initializers
        size = UnityEngine.Random.Range(minMapSize, maxMapSize);
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
        }
            
        if (!makeRandomMap)
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
            GetMap();
        }
        
        // When Finishes, Performs Handoff
        mapMadeEvent?.Invoke(map, units);
    }

    #region Get Map
    private void GetMap()
    {
        GetGroundLayer();
        GetObstacleLayer();
        GetSpawnPoints();
    }

    #region Ground Layer
    private void GetGroundLayer()
    {
        for (int x = 0; x < map.GetWidth(); x++)
        {
            for (int z = 0; z < map.GetHeight(); z++)
            {
                RaycastHit[] hits = GetRaycastData(x, z, LayerMask.GetMask("Ground"));
                if (hits.Length == 0)
                    continue;
                RaycastHit hit = FindTopBlock(hits);
                CreateNode(x, z, hit.transform);
            }
        }
    }

    private void CreateNode(int x, int z, Transform block)
    {
        int y = (int)block.position.y;
        Node node = new Node(x, y, z, map);
        CreateBlockObject(x, z, node, block);
        CreateForecastTile(x, z, node);
        map.SetGridObject(x, z, node);
    }

    private RaycastHit FindTopBlock(RaycastHit[] hits)
    {
        int y = 0;
        RaycastHit raycast = hits[0];
        foreach (RaycastHit hit in hits)
        {
            if (hit.transform.position.y > y)
            {
                y = (int)hit.transform.position.y;
                raycast = hit;
            }
        }
        return raycast;
    }

    private void CreateBlockObject(int x, int z, Node node, Transform block)
    {
        BlockObject blockObject = block.GetComponent<Block>().blockObject;
        node.SetBlockObject(block, blockObject);
    }

    private void CreateForecastTile(int x, int z, Node node)
    {
        float y2 = node.GetStandingPoint().y;
        Vector3 spawnPoint = map.GetWorldPosition(x, z) + new Vector3(0, y2, 0);
        ForecastTile tile = Instantiate(forecastTile, spawnPoint, Quaternion.identity).GetComponent<ForecastTile>();
        tile.SetMap(map);
        node.SetForecastTile(tile);
    }
    #endregion

    #region Obstacle & Decor
    private void GetObstacleLayer()
    {
        for (int x = 0; x < map.GetWidth(); x++)
        {
            for (int z = 0; z < map.GetHeight(); z++)
            {
                RaycastHit[] hits = GetRaycastData(x, z, LayerMask.GetMask("Obstacle"));
                if (hits.Length == 0)
                        continue;
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
    #endregion

    #region Spawning
    private void GetSpawnPoints()
    {
        List<Node> spawnPoints = new List<Node>();
        List<UnitObj> unitsToSpawn = new List<UnitObj>();
        GameObject[] spawnPointsObj = GameObject.FindGameObjectsWithTag("SpawnPoint");
        
        for (int i = 0; i < spawnPointsObj.Length; i++)
        {
            Vector3 position = spawnPointsObj[i].transform.position;
            SpawnPointBlock spb = spawnPointsObj[i].GetComponent<SpawnPointBlock>();

            map.GetXZ(position, out int x, out int z);
            Node node = map.GetGridObject(x, z);
            spawnPoints.Add(node);
            unitsToSpawn.Add(spb.unitObj);

            Destroy(spb.gameObject);
        }

        SpawnUnits(spawnPoints, unitsToSpawn);
    }

    public void SpawnUnits(List<Node> spawnPoints, List<UnitObj> unitsToSpawn)
    {
        List<Unit> unitsToPlace = InstantiateUnits(unitsToSpawn);
        PlaceUnits(spawnPoints, unitsToPlace);

        units = unitsToPlace;
    }

    private List<Unit> InstantiateUnits(List<UnitObj> unitsToSpawn)
    {
        List<Unit> unitsToPlace = new List<Unit>();
        for (int i = 0; i < unitsToSpawn.Count; i++)
        {
            UnitObj nextUnitObj = unitsToSpawn[i];

            GameObject clone = Instantiate(nextUnitObj.prefab);
            Unit unit = clone.GetComponent<Unit>();
            unit.Setup(map, nextUnitObj);

            if (unit.unitType == Unit.UnitType.Enemy)
                unit.SetAsEnemy();
            
            unitsToPlace.Add(unit);
        }
        return unitsToPlace;
    }

    private void PlaceUnits(List<Node> spawnPoints, List<Unit> unitsToPlace)
    {
        for (int i = 0; i < spawnPoints.Count; i++)
            gameMaster.Place(unitsToPlace[i], spawnPoints[i]);
    }
    #endregion

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
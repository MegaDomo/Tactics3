using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapMaker : MonoBehaviour
{
    [Header("Unity References")]
    public List<TileObject> blocks;
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
        {
            for (int j = 0; j < map.GetSize(); j++)
            {
                map.GetGridObject(i, j).x = i;
                map.GetGridObject(i, j).z = j;
            }
        }


        if (makeRandomMap)
            CreateMap();
        

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
                Transform temp;
                int z = Random.Range(0, 5);

                // Obstacle 20%
                if (z == 0)
                    temp = blocks[2].prefab;
                // Swamp 40%
                else if (z == 1 || z == 2)
                    temp = blocks[0].prefab;
                // Plains 40%
                else
                    temp = blocks[1].prefab;


                Transform block = Instantiate(temp, map.GetWorldPosition(i, j), Quaternion.identity);
                StoreDataInGrid(i, j, block);                
            }
        }
    }

    private void StoreDataInGrid(int x, int z, Transform block)
    {
        map.GetGridObject(x, z).SetBlock(block);
        Node node = block.GetComponent<Node>();
        node.SetNode(map, x, z);
        map.SetGridObject(x, z, node);
    }

    // Stores the Node on the Coordinate Map
    private void StoreNodeInCoordinate(int x, int y, Node node)
    {
        node.SetNode(map, x, y);
        map.SetGridObject(x, y, node);
    }
    #endregion

    // Put the Units in the spawn Positions
    private void PlaceUnits()
    {
        for (int i = 0; i < BattleSystem.instance.units.Count; i++)
        {
            MapManager.instance.Place(BattleSystem.instance.units[i], map.GetGridObject(i, i));
        }
    }

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

}

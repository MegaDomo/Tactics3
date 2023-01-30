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
    [SerializeField] private int cellSize;
    [SerializeField] private int minMapSize;
    [SerializeField] private int maxMapSize;
    [SerializeField] private Vector3 offset;

    [HideInInspector] public Grid<Node> map;

    private int size;

    public void SetUp()
    {
        // Initializers
        size = Random.Range(minMapSize, maxMapSize);
        map = new Grid<Node>(size, cellSize, startingPoint.position, () => new Node());
        CreateMap();
        PlaceUnits();

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

                Node clone = Instantiate(temp, map.GetWorldPosition(i, j), Quaternion.identity).GetComponent<Node>();
                StoreNodeInCoordinate(i, j, clone);
            }
        }
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

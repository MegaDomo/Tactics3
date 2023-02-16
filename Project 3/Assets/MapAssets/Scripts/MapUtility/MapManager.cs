using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    #region Singleton
    public static MapManager instance;

    void Awake()
    {
        instance = this;
    }
    #endregion

    [Header("Attributes")]
    public float pathingSpeed;

    [HideInInspector] public int stepSize;
    [HideInInspector] public Grid<Node> map;
    [HideInInspector] public Pathfinding pathing;

    public void SetUp()
    {
        pathing = new Pathfinding(map, stepSize);
    }

    public void GetMapData(Grid<Node> map, int stepSize)
    {
        this.map = map;
        this.stepSize = stepSize;
    }

    // Unit is unit to move, and node is destination
    public void Move(Unit selected, Node destination)
    {
        // Base Case : If Can't move return
        if (!destination.passable || !CanMove(selected, selected.node, destination))
            return;

        Node start = selected.node;

        List<Node> path = pathing.AStar(selected.node, destination);
        int pathCost = pathing.GetPathCost(path);

        // Moves the Player along the Queue
        StartCoroutine(TraversePath(pathCost, selected, path));
        
        // Updates the Nodes
        start.OnUnitExit();
        destination.OnUnitEnter();
    }

    public void MoveAsCloseAsPossible(Unit selected, Node destination)
    {
        // Base Case : If Can't move return
        if (!destination.passable)
            return;

        Node start = selected.node;

        List<Node> path = pathing.GetClosestPath(start, destination, selected);
        int pathCost = pathing.GetPathCost(path);
        Debug.Log("PathingCost; " + pathCost + " --- Count: " + path.Count);
        Utils.CreateWorldTextPopupOnGrid(path[path.Count - 1], 10f, "Closest Move", 30, map);
        // Moves the Player along the Queue
        StartCoroutine(TraversePath(pathCost, selected, path));

        // Updates the Nodes
        start.OnUnitExit();
        destination.OnUnitEnter();
    }

    IEnumerator TraversePath(int pathCost, Unit selected, List<Node> path)
    {
        // Moves Player
        for (int i = 1; i < path.Count; i++)
        {
            Place(selected, path[i]);
            yield return new WaitForSeconds(pathingSpeed);
        }
        selected.stats.moved += pathCost;
    }

    // ===== Utility Methods =====
    #region Utility Methods
    // This Method will be called by the Move Command, and it needs to be called by mouse hovering
    public bool CanMove(Unit selected, Node start, Node end)
    {
        // Objective Distance
        if (GetDistance(start, end) > MovementLeft(selected))
            return false;

        // Get Path
        int pathCost = pathing.GetPathCost(start, end);
        Debug.Log(pathCost);
        // No Path
        if (pathCost == -1)
            return false;
        
        // Path is too long
        if (pathCost > MovementLeft(selected))
            return false;

        return true;
    }

    // Doing the Math to get an integer worth of tiles moved    
    public int GetDistance(Node start, Node end)
    {
        // Gets Differences of destination - start point
        Vector3 dif = GetCoordinates(end) - GetCoordinates(start);

        // Add up total Movement along x and z axis
        return (int)Mathf.Abs(dif.x) + (int)Mathf.Abs(dif.z);
    }

    // Gets the Node the Player has moved to. Returns the respected Coordinates
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

    // Actually Moves the GameObject
    public void Place(Unit unit, Node destination)
    {
        Vector3 newPosition = destination.GetStandingPoint() + unit.offset;
        unit.gameObject.transform.position = newPosition;
        // This Sets Node and Unit data
        unit.node = destination;
        destination.unit = unit;
    }

    public int MovementLeft(Unit unit)
    {
        return unit.stats.movement - unit.stats.moved;
    }
    #endregion

}

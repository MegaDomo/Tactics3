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

    [HideInInspector] public Grid<Node> map;
    [HideInInspector] public Pathfinding pathing;

    public void SetUp()
    {
        pathing = new Pathfinding(map);
    }

    // Unit is unit to move, and node is destination
    public void Move(Unit selected, Node destination)
    {
        // Base Case : If Can't move return
        if (!destination.passable || !CanMove(selected, selected.node, destination))
            return;

        Node start = selected.node;

        // Moves the Player along the Queue
        StartCoroutine(MoveAlongPath(selected, destination));
        
        // Updates the Nodes
        start.OnUnitExit();
        destination.OnUnitEnter();
    }

    IEnumerator MoveAlongPath(Unit selected, Node end)
    {
        List<Node> path = pathing.AStar(selected.node, end);
        int pathCost = pathing.GetPathCost(path);
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
        if (GetDistance(start, end) > selected.stats.movement - selected.stats.moved)
            return false;

        // Get Path
        int pathCost = pathing.GetPathCost(start, end);
        Debug.Log(pathCost);
        // No Path
        if (pathCost == -1)
            return false;
        
        // Path is too long
        if (pathCost > selected.stats.movement - selected.stats.moved)
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
        // This Sets node Data in Unit Script
        unit.node = destination;
    }
    #endregion

}
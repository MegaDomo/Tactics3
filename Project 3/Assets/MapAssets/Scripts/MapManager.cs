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

    void Start()
    {
        
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

    #region PathFinding
    IEnumerator MoveAlongPath(Unit selected, Node end)
    {
        List<Node> path = AStar(selected, selected.node, end, out int pathCost);

        // This uses the Length of the Path in relation to how far the player can move
        // Moves Player
        for (int i = 0; i < path.Count; i++)
        {
            Place(selected, path[i]);
            yield return new WaitForSeconds(pathingSpeed);
        }
        selected.stats.moved += pathCost;
    }


    // ===== Breadth First / AStar / A* =====

    private List<Node> AStar(Unit selected, Node start, Node end, out int pathCost)
    {
        List<Node> path = new List<Node>();

        // Starts the Queue
        PriorityQueue <Node> frontier = new PriorityQueue<Node>();
        Dictionary<Node, Node> came_from = new Dictionary<Node, Node>();
        Dictionary<Node, int> cost_so_far = new Dictionary<Node, int>();
        frontier.Enqueue(start, 0);
        came_from.Add(start, null);
        cost_so_far.Add(start, 0);

        Node current;        
        // Performs the Search
        while (frontier.Count != 0)
        {
            current = frontier.Dequeue();

            // Early Exit : Are we there yet?
            if (current == end)
                break;

            // Adds Adjacent Edges
            foreach (Node next in current.edges)
            {
                // If Obstacle
                if (!next.passable)
                    continue;

                // Finds Cost of current path
                int newCost = cost_so_far[current] + current.movementCost;                
                if (!cost_so_far.ContainsKey(next) || newCost < cost_so_far[next])
                {
                    cost_so_far[next] = newCost + GetDistance(next, end);
                    frontier.Enqueue(next, newCost);
                    came_from[next] = current;
                }
            }
        }

        // Defines the Path, following the Breadcrumbs
        current = end;
        pathCost = 0;

        // If no Path found
        if (!came_from.ContainsKey(current))
            return path;
        
        while (current != start)
        {
            // Adds current node
            path.Add(current);            
            // Updates current node with next in Breadcrumb trail
            current = came_from[current];
            // Adds up Path Cost
            pathCost += current.movementCost;
        }        
        Debug.Log(pathCost);
        // Note : Will not include start Node
        path.Reverse();
        return path;
    }
    #endregion

    // ===== Utility Methods =====
    #region Utility Methods
    // This Method will be called by the Move Command, and it needs to be called by mouse hovering
    public bool CanMove(Unit selected, Node start, Node end)
    {
        // Objective Distance
        if (GetDistance(start, end) > selected.stats.movement - selected.stats.moved)
            return false;

        // Get Path        
        List<Node> path = AStar(selected, start, end, out int pathCost);
        
        // No Path
        if (path.Count == 0)
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

    // Actually Moves the GameObjectObject
    public void Place(Unit unit, Node destination)
    {
        // Moves the Player
        unit.gameObject.transform.position = destination.standingPoint.position + unit.offset;
        // This Sets node Data in Unit Script
        unit.node = destination;
    }
    #endregion

}

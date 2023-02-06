using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding
{
    Grid<Node> grid;

    public Pathfinding(Grid<Node> grid)
    {
        this.grid = grid;
    }

    #region Pathing
    // Finds the Path and also returns the pathCost
    public List<Node> AStar(Node start, Node end)
    {
        List<Node> path = new List<Node>();

        // Starts the Queue
        PriorityQueue<Node> frontier = new PriorityQueue<Node>();
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
            foreach (Node next in GetNeighbors(current))
            {
                // Finds Cost of current path
                int newCost = cost_so_far[current] + next.movementCost;
                if (!cost_so_far.ContainsKey(next) || newCost < cost_so_far[next])
                {
                    cost_so_far[next] = newCost;
                    int priority = newCost + GetDistance(next, end);
                    frontier.Enqueue(next, priority);
                    came_from[next] = current;
                }
            }
        }

        // Defines the Path, following the Breadcrumbs
        current = end;

        // If no Path found
        if (!came_from.ContainsKey(current))
            return path;

        while (current != start)
        {
            // Adds current node
            path.Add(current);
            // Updates current node with next in Breadcrumb trail
            current = came_from[current];
        }

        path.Add(current);
        path.Reverse();
        return path;
    }

    public List<Node> GetNeighbors(Node node)
    {
        List<Node> neighbors = new List<Node>();

        int x = node.x;
        int z = node.z;

        if (isSafe(x + 1, z) && isSafe(grid.GetGridObject(x + 1, z)))
            neighbors.Add(grid.GetGridObject(x + 1, z));
        if (isSafe(x, z + 1) && isSafe(grid.GetGridObject(x, z + 1)))
            neighbors.Add(grid.GetGridObject(x, z + 1));
        if (isSafe(x - 1, z) && isSafe(grid.GetGridObject(x - 1, z)))
            neighbors.Add(grid.GetGridObject(x - 1, z));
        if (isSafe(x, z - 1) && isSafe(grid.GetGridObject(x, z - 1)))
            neighbors.Add(grid.GetGridObject(x, z - 1));

        return neighbors;
    }
    private bool isSafe(int x, int z)
    {
        if (x < 0 || z < 0 || x >= grid.GetSize() || z >= grid.GetSize())
            return false;
        return true;
    }

    private bool isSafe(Node neighbor)
    {
        if (!neighbor.passable)
            return false;
        return true;
    }

    public int GetPathCost(List<Node> path)
    {
        int pathCost = 0;
        for (int i = 0; i < path.Count - 1; i++)
        {
            pathCost += path[i].movementCost;
        }
        return pathCost;
    }

    // Returns -1 if no path found
    public int GetPathCost(Node start, Node end)
    {
        List<Node> path = AStar(start, end);
        if (path.Count == 0)
            return -1;
        return GetPathCost(path);
    }
    #endregion

    #region Utility Methods
    public bool CanMove(Unit selected, Node start, Node end)
    {
        // Heuristic
        if (GetDistance(start, end) > selected.stats.movement - selected.stats.moved)
            return false;

        int pathCost = GetPathCost(start, end);
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
        for (int i = 0; i < grid.GetSize(); i++)
        {
            for (int j = 0; j < grid.GetSize(); j++)
            {
                // Compare
                if (grid.GetGridObject(i, j) == node)
                    return new Vector3(i, 0, j);
            }
        }

        // Base Case : Out of Bounds
        return new Vector3(-1, 0, -1);
    }
    #endregion
}

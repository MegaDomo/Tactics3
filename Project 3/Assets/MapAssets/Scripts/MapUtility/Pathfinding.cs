using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding
{
    int stepSize;
    private Grid<Node> grid;

    public Pathfinding(Grid<Node> grid, int stepSize)
    {
        this.grid = grid;
        this.stepSize = stepSize;
    }

    #region AStar Pathing
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
                if (Mathf.Abs(next.y - current.y) > stepSize)
                    continue;

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
        // Obstructed
        if (!neighbor.passable)
            return false;
        return true;
    }
    #endregion

    #region Path Variants
    public List<Node> GetClosestPath(Node start, Node end, Unit unit)
    {
        List<Node> path = AStar(start, end);
        int nodeCost = GetNodeCostFromMovement(path, MapManager.instance.MovementLeft(unit));
        path.RemoveRange(nodeCost, path.Count - nodeCost);

        return path;
    }

    public int GetPathCost(List<Node> path)
    {
        int pathCost = 0;
        foreach (Node node in path)
        {
            pathCost += node.movementCost;
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

    public int GetPathCostWithoutStart(List<Node> path)
    {
        int pathCost = 0;
        for (int i = 1; i < path.Count; i++)
        {
            pathCost += path[i].movementCost;
        }
        return pathCost;
    }

    public int GetPathCostWithoutStart(Node start, Node end)
    {
        List<Node> path = AStar(start, end);
        if (path.Count == 0)
            return -1;
        return GetPathCostWithoutStart(path);
    }

    // Note : Used for Getting Closest Path
    public int GetNodeCostFromMovement(List<Node> path, int movement)
    {
        int nodeCost = 0;
        int pathCost = 0;
        foreach (Node node in path)
        {
            nodeCost++;
            pathCost += node.movementCost;
            if (pathCost > movement)
                break;
            
        }
        return nodeCost;
    }
    #endregion

    // Methods to get various shapes of Nodes in the form of Lists
    #region Getting to know your neighbors
    public List<Node> GetDiamond(Node target, int radius)
    {
        List<Node> diamond = new List<Node>();
        List<Vector2Int> coor = GetDiamondCoordinateList(target, radius, 0);
        foreach (Vector2Int vector in coor)
        {
            if (isSafe(vector.x, vector.y))
                diamond.Add(grid.GetGridObject(vector.x, vector.y));
        }
        return diamond;
    }

    public List<Node> GetHollowDiamond(Node target, int radius, int minRadius)
    {
        List<Node> diamond = new List<Node>();
        List<Vector2Int> coor = GetDiamondCoordinateList(target, radius, minRadius);
        foreach (Vector2Int vector in coor)
        {
            if (isSafe(vector.x, vector.y))
                diamond.Add(grid.GetGridObject(vector.x, vector.y));
        }
        return diamond;
    }

    private List<Vector2Int> GetDiamondCoordinateList(Node origin, int maxRadius, int minRadius)
    {
        List<Vector2Int> coordinates = new List<Vector2Int>();
        int xOrigin = origin.x;
        int zOrigin = origin.z;
        for (int x = -maxRadius + xOrigin; x <= maxRadius + xOrigin; x++)
        {
            for (int z = -maxRadius + zOrigin; z <= maxRadius + zOrigin; z++)
            {
                int xDif = xOrigin - x;
                int zDif = zOrigin - z;
                int dis = Mathf.Abs(xDif) + Mathf.Abs(zDif);
                if (dis <= maxRadius && dis >= minRadius)
                    coordinates.Add(new Vector2Int(x, z));

            }
        }
        return coordinates;
    }

    public List<Node> GetSquare(int radius)
    {
        return new List<Node>();
    }

    public List<Node> GetPlus(int radius)
    {
        return new List<Node>();
    }

    public List<Node> GetLine(int radius)
    {
        return new List<Node>();
    }

    public List<Node> GetAllRoutes(Unit unit)
    {
        List<Node> allRoutes = new List<Node>();

        Queue<Node> frontier = new Queue<Node>();

        Node start = unit.node;
        Node current;

        int index = 0;
        frontier.Enqueue(start);
        while (frontier.Count != 0)
        {
            current = frontier.Dequeue();
            if (GetPathCost(start, current) > unit.MovementLeft())
                continue;
            allRoutes.Add(current);

            List<Node> neighbors = GetNeighbors(current);
            foreach (Node node in neighbors)
            {
                if (!frontier.Contains(node) && !allRoutes.Contains(node))
                    frontier.Enqueue(node);
            }
            index++;
            if (index > 500)
            {
                Debug.Log(frontier.Count + " Too Much");
                break;
            }
        }
        return allRoutes;
    }
    private bool IsSafe(int x, int z)
    {
        return grid.isCoordinatesSafe(x, z);
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

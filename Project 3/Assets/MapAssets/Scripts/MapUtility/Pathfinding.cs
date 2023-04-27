using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Pathfinding
{
    #region AStar Pathing
    // Finds the Path and also returns the pathCost
    public static List<Node> AStar(Grid<Node> grid, Node start, Node end)
    {
        List<Node> path = new List<Node>();
        float stepSize = grid.GetCellSize() / 2;
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
            foreach (Node next in GetPassibleNeighbors(grid, current))
            {
                if (Mathf.Abs(next.y - current.y) > stepSize)
                    continue;

                // Finds Cost of current path
                int newCost = cost_so_far[current] + next.movementCost;
                if (!cost_so_far.ContainsKey(next) || newCost < cost_so_far[next])
                {
                    cost_so_far[next] = newCost;
                    int priority = newCost + GetDistance(grid, next, end);
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

        path.Reverse();
        return path;
    }

    public static List<Node> AStarWithStart(Grid<Node> grid, Node start, Node end)
    {
        List<Node> path = new List<Node>();
        float stepSize = grid.GetCellSize() / 2;
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
            foreach (Node next in GetPassibleNeighbors(grid, current))
            {
                if (Mathf.Abs(next.y - current.y) > stepSize)
                    continue;

                // Finds Cost of current path
                int newCost = cost_so_far[current] + next.movementCost;
                if (!cost_so_far.ContainsKey(next) || newCost < cost_so_far[next])
                {
                    cost_so_far[next] = newCost;
                    int priority = newCost + GetDistance(grid, next, end);
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
        path.RemoveAt(path.Count - 1);
        return path;
    }
    #endregion

    #region Grid/Path Methods
    // Start of Grid/Path Methods Region
    #region Closest
    public static List<Node> GetClosestPath(Grid<Node> grid, Node start, Node end, Unit unit)
    {
        List<Node> path = AStarWithStart(grid, start, end);
        int nodeCost = GetNodeCostFromMovement(path, unit.MovementLeft());
        path.RemoveRange(nodeCost + 1, path.Count - nodeCost - 1);
        return path;
    }

    public static Node GetClosestPassibleNode(Grid<Node> grid, Node start, List<Node> destinations)
    {
        Node node = null;
        int closest = int.MaxValue;
        int nextCost;

        foreach (Node nextNode in destinations)
        {
            if (!IsSafePassible(nextNode))
                continue;
            nextCost = GetPathCost(grid, start, nextNode);

            if (nextCost < closest)
            {
                closest = nextCost;
                node = nextNode;
            }
        }

        return node;
    }

    public static Node GetClosestNode(Grid<Node> grid, Node start, List<Node> destinations)
    {
        Node node = null;
        int closest = int.MaxValue;
        int nextCost;

        foreach (Node nextNode in destinations)
        {
            nextCost = GetPathCost(grid, start, nextNode);

            if (nextCost < closest)
            {
                closest = nextCost;
                node = nextNode;
            }
        }

        return node;
    }

    public static Node GetClosestPassibleNode(Grid<Node> grid, Node start, Node destination)
    {
        Node node = null;
        List<Node> destinations = GetPassibleNeighbors(grid, destination);
        int closest = int.MaxValue;
        int nextCost;

        foreach (Node nextNode in destinations)
        {
            nextCost = GetPathCost(grid, start, nextNode);

            if (nextCost < closest)
            {
                closest = nextCost;
                node = nextNode;
            }
        }

        return node;
    }

    public static Node GetClosestNode(Grid<Node> grid, Node start, Node destination)
    {
        Node node = null;
        List<Node> destinations = GetNeighbors(grid, destination);
        int closest = int.MaxValue;
        int nextCost;

        foreach (Node nextNode in destinations)
        {
            nextCost = GetPathCost(grid, start, nextNode);

            if (nextCost < closest)
            {
                closest = nextCost;
                node = nextNode;
            }
        }

        return node;
    }
    #endregion

    #region PathCost
    public static int GetPathCost(List<Node> path)
    {
        int pathCost = 0;
        foreach (Node node in path)
        {
            pathCost += node.movementCost;
        }
        return pathCost;
    }
    
    // Returns -1 if no path found
    public static int GetPathCost(Grid<Node> grid, Node start, Node end)
    {
        List<Node> path = AStar(grid, start, end);
        if (path.Count == 0)
            return -1;
        return GetPathCost(path);
    }

    public static int GetPathCostWithStart(Grid<Node> grid, Node start, Node end)
    {
        List<Node> path = AStarWithStart(grid, start, end);
        if (path.Count == 0)
            return -1;
        return GetPathCost(path);
    }

    public static int GetPathCostWithoutStart(List<Node> path)
    {
        int pathCost = 0;
        for (int i = 1; i < path.Count; i++)
        {
            pathCost += path[i].movementCost;
        }
        return pathCost;
    }

    public static int GetPathCostWithoutStart(Grid<Node> grid, Node start, Node end)
    {
        List<Node> path = AStar(grid, start, end);
        if (path.Count == 0)
            return -1;
        return GetPathCostWithoutStart(path);
    }

    // Note : Used for Getting Closest Path
    public static int GetNodeCostFromMovement(List<Node> path, int movement)
    {
        int nodeCost = 0;
        int pathCost = 0;
        foreach (Node node in path)
        {
            pathCost += node.movementCost;
            if (pathCost > movement)
                break;
            nodeCost++;
        }
        return nodeCost;
    }
    #endregion
    // End of Grid/Path Methods Region
    #endregion

    // Methods to get various shapes of Nodes in the form of Lists
    #region Getting to know your neighbors
    public static List<Node> GetPassibleNeighbors(Grid<Node> grid, Node node)
    {
        List<Node> neighbors = new List<Node>();

        int x = node.x;
        int z = node.z;

        if (isSafe(grid, x + 1, z) && IsSafePassible(grid.GetGridObject(x + 1, z)))
            neighbors.Add(grid.GetGridObject(x + 1, z));
        if (isSafe(grid, x, z + 1) && IsSafePassible(grid.GetGridObject(x, z + 1)))
            neighbors.Add(grid.GetGridObject(x, z + 1));
        if (isSafe(grid, x - 1, z) && IsSafePassible(grid.GetGridObject(x - 1, z)))
            neighbors.Add(grid.GetGridObject(x - 1, z));
        if (isSafe(grid, x, z - 1) && IsSafePassible(grid.GetGridObject(x, z - 1)))
            neighbors.Add(grid.GetGridObject(x, z - 1));

        return neighbors;
    }

    public static List<Node> GetNeighbors(Grid<Node> grid, Node node)
    {
        List<Node> neighbors = new List<Node>();

        int x = node.x;
        int z = node.z;

        if (isSafe(grid, x + 1, z))
            neighbors.Add(grid.GetGridObject(x + 1, z));
        if (isSafe(grid, x, z + 1))
            neighbors.Add(grid.GetGridObject(x, z + 1));
        if (isSafe(grid, x - 1, z))
            neighbors.Add(grid.GetGridObject(x - 1, z));
        if (isSafe(grid, x, z - 1))
            neighbors.Add(grid.GetGridObject(x, z - 1));

        return neighbors;
    }

    public static List<Node> GetDiamond(Grid<Node> grid, Node target, int radius)
    {
        List<Node> diamond = new List<Node>();
        List<Vector2Int> coor = GetDiamondCoordinateList(target, radius, 0);
        foreach (Vector2Int vector in coor)
        {
            if (isSafe(grid, vector.x, vector.y))
                diamond.Add(grid.GetGridObject(vector.x, vector.y));
        }
        return diamond;
    }

    public static List<Node> GetHollowDiamond(Grid<Node> grid, Node target, int radius, int minRadius)
    {
        List<Node> diamond = new List<Node>();
        List<Vector2Int> coor = GetDiamondCoordinateList(target, radius, minRadius);
        foreach (Vector2Int vector in coor)
        {
            if (isSafe(grid, vector.x, vector.y))
                diamond.Add(grid.GetGridObject(vector.x, vector.y));
        }
        return diamond;
    }

    private static List<Vector2Int> GetDiamondCoordinateList(Node origin, int maxRadius, int minRadius)
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

    public static List<Node> GetSquare(Grid<Node> grid, int radius)
    {
        return new List<Node>();
    }

    public static List<Node> GetPlus(Grid<Node> grid, int radius)
    {
        return new List<Node>();
    }

    public static List<Node> GetLine(Grid<Node> grid, int radius)
    {
        return new List<Node>();
    }

    public static List<Node> GetAllRoutes(Grid<Node> grid, Unit unit)
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
            if (GetPathCost(grid, start, current) > unit.MovementLeft())
                continue;
            allRoutes.Add(current);

            List<Node> neighbors = GetPassibleNeighbors(grid, current);
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
    #endregion

    #region Utility Methods
    private static bool isSafe(Grid<Node> grid, int x, int z)
    {
        if (x < 0 || z < 0 || x >= grid.GetSize() || z >= grid.GetSize())
            return false;
        return true;
    }

    private static bool IsSafePassible(Node neighbor)
    {
        // Obstructed
        if (!neighbor.passable)
            return false;
        return true;
    }

    public static bool CanMove(Grid<Node> grid, Unit selected, Node start, Node end)
    {
        // Heuristic
        if (GetDistance(grid, start, end) > selected.stats.movement - selected.stats.moved)
            return false;

        int pathCost = GetPathCost(grid, start, end);
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
    public static int GetDistance(Grid<Node> grid, Node start, Node end)
    {
        // Gets Differences of destination - start point
        Vector3 dif = GetCoordinates(grid, end) - GetCoordinates(grid, start);

        // Add up total Movement along x and z axis
        return (int)Mathf.Abs(dif.x) + (int)Mathf.Abs(dif.z);
    }

    // Gets the Node the Player has moved to. Returns the respected Coordinates
    public static Vector3 GetCoordinates(Grid<Node> grid, Node node)
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

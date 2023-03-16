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

    private bool unitIsMoving = false;
    
    public void SetMapData(Grid<Node> map, int stepSize)
    {
        this.map = map;
        this.stepSize = stepSize;
    }

    #region Move Methods
    public List<Node> GetPath(Node start, Node end)
    {
        return Pathfinding.AStar(map, start, end);
    }

    public int GetPathCost(List<Node> path)
    {
        return Pathfinding.GetPathCost(path);
    }
    // Unit is unit to move, and node is destination
    public void Move(Unit selected, Node destination)
    {
        // Base Case : If Can't move return
        if (!CanMove(selected, selected.node, destination))
            return;

        Node start = selected.node;

        List<Node> path = Pathfinding.AStar(map, selected.node, destination);
        int pathCost = Pathfinding.GetPathCost(path);

        // Updates the Nodes
        start.OnUnitExit();
        destination.OnUnitEnter(selected);
    }

    public void MoveAsCloseAsPossible(Unit selected, Node destination)
    {
        if (unitIsMoving)
            return;
        if (!destination.passable)
            return;

        Node start = selected.node;

        List<Node> path = Pathfinding.GetClosestPath(map, start, destination, selected);
        int pathCost = Pathfinding.GetPathCost(path);

        if (path.Count <= 0)
        {
            Debug.Log("For " + selected.name + " Path was 0");
            return;
        }

        destination = path[path.Count - 1];
        Utils.CreateWorldTextPopupOnGrid(destination, 10f, "Closest Move", 30, map);

        // Moves the Player along the Queue
        unitIsMoving = true;
        StartCoroutine(TraversePath(pathCost, selected, path));

        // Updates the Nodes
        start.OnUnitExit();
        destination.OnUnitEnter(selected);
    }

    IEnumerator TraversePath(int pathCost, Unit selected, List<Node> path)
    {
        // Moves Player
        // Note : i = 1 because the path includes the Node the unit is already standing on;
        // therefore it is skipped
        for (int i = 0; i < path.Count; i++)
        {
            Place(selected, path[i]);
            yield return new WaitForSeconds(pathingSpeed);
        }
        selected.stats.moved += pathCost;

        unitIsMoving = false;
    }
    #endregion

    #region Getters & Setters
    public Grid<Node> GetMap()
    {
        return map;
    }
    #endregion

    #region Utility Methods
    // This Method will be called by the Move Command, and it needs to be called by mouse hovering
    public bool CanMove(Unit selected, Node start, Node end)
    {
        if (!end.passable)
            return false;

        // Quick Out : Objective Distance 
        if (GetDistance(start, end) > MovementLeft(selected))
        {
            Debug.Log("GetDistance");
            return false;
        }
            

        int pathCost = Pathfinding.GetPathCost(map, start, end);

        if (pathCost == -1)
            return false;
        
        // Path is too long
        if (pathCost > MovementLeft(selected))
        {
            Debug.Log("pathCost");
            return false;
        }

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

    public void TranslateBetweenNodes(Unit unit, Node destination)
    {
        Vector3 newPosition = destination.GetStandingPoint() + unit.offset;
    }

    public int MovementLeft(Unit unit)
    {
        return unit.stats.movement - unit.stats.moved;
    }
    #endregion
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    [Header("Unity References")]
    public Transform standingPoint;
    public GameObject vfx;

    [Header("Attributes")]
    [SerializeField] private Color def;
    [SerializeField] private Color withinReach;
    [SerializeField] private Color outOfReach;

    [HideInInspector] public List<Node> edges = new List<Node>();

    #region Node Stats
    [HideInInspector] public Unit unit;

    private Material mat;

    public int movementCost;
    public bool passable;


    
    #endregion

    #region Pathing Stats
    [HideInInspector] public int flag; // 0 is no flag, 1 is flag
    #endregion

    // Constructor
    public Node()
    {
        
    }

    void Awake()
    {
        // Sets Default Color
        mat = vfx.GetComponent<MeshRenderer>().material;
        mat.color = def;
    }

    // Called when a Unit enters this Node
    public void OnUnitEnter()
    {
        passable = false;
    }

    public void OnUnitExit()
    {
        passable = true;
    }

    public void UpdateValues()
    {

    }

    #region OnMouse Methods
    // Moves the Player On a Mouse Click
    void OnMouseDown()
    {
        // Gives the BattleSystem the Destination
        MapManager.instance.Move(this);
    }

    // When Mouse Enters Node
    void OnMouseEnter()
    {
        // Check if in Reach to determine green or red
        if (!passable)
        {
            mat.color = outOfReach;
            return;
        }
            
        // Can move to, Turn Green
        if (MapManager.instance.CanMove(MapManager.instance.selected.node, this))
            mat.color = withinReach;
        // Can't move to, Turn Red
        else
            mat.color = outOfReach;
    }

    // When Mouse Exits Node
    void OnMouseExit()
    {
        // Change back to default Color
        mat.color = def;
    }
    #endregion

    #region Setters
    public void SetIsPassable(bool value)
    {
        passable = value;
    }

    public void AddEdge(Node node)
    {
        // Already Has Node
        if (edges.Contains(node))
            return;
         
        // Adds Node
        edges.Add(node);
    }
    #endregion
}

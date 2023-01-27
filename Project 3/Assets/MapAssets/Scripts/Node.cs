using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    [Header("Unity References")]
    public Transform standingPoint;
    public GameObject vfx;

    [Header("Attributes")]
    public int movementCost;
    [SerializeField] private Color def;
    [SerializeField] private Color withinReach;
    [SerializeField] private Color outOfReach;

    [HideInInspector] public bool passable;
    [HideInInspector] public int x;
    [HideInInspector] public int y;    
    [HideInInspector] public Unit unit;
    [HideInInspector] public List<Node> edges = new List<Node>();

    private Material mat;

    // Constructor
    public Node(int x, int y)
    {
        this.x = x;
        this.y = y;
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
        // TODO : Inspect Nodes get details

        // Gives the BattleSystem the Destination
        PlayerTurn.instance.PlayerMove(this);
    }

    // When Mouse Enters Node
    void OnMouseEnter()
    {
        Highlight();
        // TODO : Tool tip for tile stats
    }

    private void Highlight()
    {
        // If players turn
        if (CombatState.state != BattleState.PLAYERTURN)
            return;

        // Check if in Reach to determine green or red
        if (!passable)
        {
            mat.color = outOfReach;
            return;
        }

        // Can move to, Turn Green
        Unit unit = PlayerTurn.instance.selected;
        if (MapManager.instance.CanMove(unit, unit.node, this))
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

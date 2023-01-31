using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    [Header("Unity References")]
    public TileObject tileObject;
    public Transform standingPoint;
    public GameObject vfx;

    [Header("Attributes")]
    public int movementCost;
    [SerializeField] private Color def;
    [SerializeField] private Color withinReach;
    [SerializeField] private Color outOfReach;

    public bool passable = true;

    private Material mat;

    // Constructor
    public Block()
    {
        // Debug Constructor
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

    public void SetIsPassable(bool value)
    {
        passable = value;
    }
    /*
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
    */
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForecastTile : MonoBehaviour
{
    [Header("Unity References")]
    [SerializeField] private MeshRenderer mesh;

    [Header("Attributes")]
    [SerializeField] private Color def;
    [SerializeField] private Color withinReach;
    [SerializeField] private Color outOfReach;

    [HideInInspector] public int x;
    [HideInInspector] public int z;
    [HideInInspector] public Node node;

    private Material mat;

    private void Awake()
    {
        mat = mesh.material;
        mat.color = def;
    }

    public void SetNode(Node node)
    {
        this.node = node;
    }

    #region OnMouse Methods
    // Moves the Player On a Mouse Click
    void OnMouseDown()
    {
        // TODO : Inspect Nodes get details
        // Gives the BattleSystem the Destination
        PlayerTurn.instance.PlayerMove(node);
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
        if (!node.passable)
        {
            mat.color = outOfReach;
            return;
        }

        // Can move to, Turn Green
        Unit unit = PlayerTurn.instance.selected;
        if (MapManager.instance.CanMove(unit, unit.node, node))
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
}

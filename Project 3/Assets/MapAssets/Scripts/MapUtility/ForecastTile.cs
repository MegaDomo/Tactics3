using UnityEngine;
using UnityEngine.EventSystems;

public class ForecastTile : MonoBehaviour
{
    [Header("Unity References")]
    [SerializeField] private MeshRenderer mesh;

    [Header("Attributes")]
    [SerializeField] private Color def;
    [SerializeField] private Color selected;
    [SerializeField] private Color withinReach;
    [SerializeField] private Color outOfReach;

    [HideInInspector] public int x;
    [HideInInspector] public int z;
    [HideInInspector] public Node node;

    private bool isSelected;

    private Material mat;
    private PlayerTurn player;
    private Grid<Node> map;

    private void Awake()
    {
        mat = mesh.material;
        HideTile();
    }

    public void Highlight()
    {
        // If players turn
        if (CombatState.state != BattleState.PLAYERTURN && player.actionState != ActionState.ChooseNode)
            return;

        // Check if in Reach to determine green or red
        if (!node.passable)
        {
            TileOutOfReach();
            return;
        }

        // Can move to, Turn Green
        Unit unit = player.GetSelected();
        if (Pathfinding.CanMove(map, unit, unit.node, node))
            TileInReach();
        // Can't move to, Turn Red
        else
            TileOutOfReach();
    }

    public void SelectedThisTile()
    {
        isSelected = true;
        TileSelected();
    }

    public void DeselectedThisTile()
    {
        isSelected = false;
        HideTile();
    }

    #region Color Methods
    public void TileInReach()
    {
        mat.color = withinReach;
    }

    public void TileOutOfReach()
    {
        mat.color = outOfReach;
    }

    public void TileSelected()
    {
        mat.color = selected;
    }

    public void HideTile()
    {
        mat.color = def;
    }
    #endregion

    #region Getters & Setters
    public Node GetNode()
    {
        return node;
    }

    public void SetNode(Node node)
    {
        this.node = node;
    }

    public void SetMap(Grid<Node> map)
    {
        this.map = map;
    }
    #endregion
}
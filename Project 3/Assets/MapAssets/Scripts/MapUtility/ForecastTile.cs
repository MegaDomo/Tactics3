using UnityEngine;
using UnityEngine.EventSystems;

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
    private PlayerTurn player;

    private void Awake()
    {
        mat = mesh.material;
        mat.color = def;
    }

    private void Start()
    {
        player = PlayerTurn.instance;
    }

    private void Update()
    {/*
        if (player.actionState == ActionState.ChoosingAction)
            if (player.GetTargetedNode() == node)
                mat.color = withinReach;*/
    }

    #region OnMouse Methods
    // Moves the Player On a Mouse Click
    void OnMouseDown()
    {
        // TODO : Inspect Nodes get details
        // Gives the BattleSystem the Destination
        //PlayerTurn.instance.PlayerMove(node);
    }

    // When Mouse Enters Node
    void OnMouseEnter()
    {
        Highlight();
        // TODO : Tool tip for tile stats
    }

    private void Highlight()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;
        // If players turn
        if (CombatState.state != BattleState.PLAYERTURN && player.actionState != ActionState.MoveAction)
            return;

        // Check if in Reach to determine green or red
        if (!node.passable)
        {
            mat.color = outOfReach;
            return;
        }

        // Can move to, Turn Green
        Unit unit = PlayerTurn.instance.GetSelected();
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
        HideTile();
    }
    #endregion

    public void HideTile()
    {
        mat.color = def;
    }

    #region Getters & Setters
    public Node GetNode()
    {
        return node;
    }
    public void SetNode(Node node)
    {
        this.node = node;
    }
    #endregion
}

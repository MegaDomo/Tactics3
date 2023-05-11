using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class NodeClicker : MonoBehaviour
{
    [Header("Scriptable Objects References")]
    public GameMaster gameMaster;
    public PlayerTurn playerTurn;

    [Header("Unity References")]
    public Transform nodeSelector;
    public NodeHighlighter highlighter;

    private ForecastTile selectedTile;
    private ForecastTile previousTile;

    private Ability playerAbility;
    private Node playerDestination;
    private Node targetNode;

    private void OnEnable()
    {
        playerTurn.choseAbilityEvent += AbilitySubscriber;
    }

    private void OnDisable()
    {
        playerTurn.choseAbilityEvent -= AbilitySubscriber;
    }

    #region Update - Checks
    void Update()
    {
        CheckForDeselection();
        
        CheckForNodeSelection();

        CheckForTargetSelection();
    }

    private void CheckForDeselection()
    {
        if (CombatState.state != BattleState.PLAYERTURN)
            return;

        if (Input.GetMouseButtonDown(1))
        {
            if (selectedTile != null) selectedTile.HideTile();
            playerTurn.ClearNode();
            playerTurn.ClearAbiliity();
            HideSelector();
        }
    }

    private void CheckForNodeSelection()
    {
        if (CombatState.state != BattleState.PLAYERTURN)
            return;

        RaycastHit forecastHit = GetClickData(LayerMask.GetMask("ForecastTile"));
        if (forecastHit.transform == null)
            return;

        if (playerTurn.actionState == ActionState.ChoosingAction)
        {
            ClickedOnDestination(forecastHit);
            return;
        }
    }

    private void CheckForTargetSelection()
    {
        if (CombatState.state != BattleState.PLAYERTURN)
            return;

        if (playerTurn.actionState != ActionState.ChoosingTarget)
            return;

        if (playerAbility == null)
            return;

        if (playerAbility.targetType == Ability.TargetType.StandingSingleTarget)
        {
            StandingSingleTarget();
            return;
        }

        if (playerAbility.targetType == Ability.TargetType.DirectedSingleTarget)
        {
            DirectedSingleTarget();
            return;
        }

        if (playerAbility.targetType == Ability.TargetType.StandingAoe)
        {
            StandingAoe();
            return;
        }

        if (playerAbility.targetType == Ability.TargetType.DirectedAoe)
        {
            DirectedAoe();
            return;
        }
    }
    #endregion

    #region Ability Types
    private void StandingSingleTarget()
    {
        //HighlightAbilityRange(playerDestination, playerAbility);

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit clickHit = GetClickData(LayerMask.GetMask("ForecastTile"));
            SetTargetNode(clickHit);
            ClickedOnTarget(clickHit);
        }
    }

    private void DirectedSingleTarget()
    {
        //HighlightAbilityRange(playerDestination, playerAbility);

        RaycastHit hoverHit = GetMouseHoverData(LayerMask.GetMask("ForecastTile"));
        if (hoverHit.transform == null)
            return;

        SetTargetNode(hoverHit);
        HighlightAbility(targetNode);
        if (Input.GetMouseButtonDown(0))
        {
            ClickedOnTarget(hoverHit);
        }
    }

    private void StandingAoe()
    {
        //HighlightAbility(playerDestination, playerAbility);

        if (Input.GetMouseButtonDown(0))
        {
            SetTargetNode(playerDestination);
            ClickedOnNode(targetNode);
        }
    }

    private void DirectedAoe()
    {
        //HighlightAbilityRange(playerDestination, playerAbility);

        RaycastHit hoverHit = GetMouseHoverData(LayerMask.GetMask("ForecastTile"));
        if (hoverHit.transform == null)
            return;

        SetTargetNode(hoverHit);
        HighlightAbility(targetNode, playerAbility);
        if (Input.GetMouseButtonDown(0))
        {
            if (!CheckInRange(targetNode))
            {
                Debug.Log("Not in Ability Range on Target/Node Selection");
                return;
            }
            ClickedOnNode(targetNode);
        }
    }
    #endregion

    #region Selection Helper Methods
    private void ClickedOnDestination(RaycastHit forecastHit)
    {
        ForecastTile newTile = forecastHit.transform.GetComponent<ForecastTile>();

        Node item = newTile.node;
        Unit selected = playerTurn.GetSelected();
        Grid<Node> map = gameMaster.GetMap();

        List<Node> routes = Pathfinding.GetAllRoutes(map, selected);
        if (!routes.Contains(item))
        {
            Debug.Log("Out of Range");
            return;
            // Maybe turn something red
        }


        if (newTile != selectedTile)
        {
            if (selectedTile == null)
                selectedTile = newTile;

            selectedTile.SetState(ForecastTile.ForecastState.Hidden);
            selectedTile = newTile;
        }
        SelectTile();
    }

    private void SelectTile()
    {
        Node node = selectedTile.GetNode();
        playerTurn.ChooseNode(node);
        selectedTile.SetState(ForecastTile.ForecastState.Selected);
        MoveSelector(node);
    }

    private void ClickedOnTarget(RaycastHit forecastHit)
    {
        ForecastTile forecastTile = forecastHit.transform.GetComponent<ForecastTile>();
        Node node = forecastTile.node;

        if (node.unit == null)
        {
            Debug.Log("No Unit on RaycastHit for SingleTarget Ability");
            return;
        }

        if (!CheckInRange(node))
        {
            Debug.Log("Not in Ability Range on Target/Node Selection");
            return;
        }

        playerTurn.ChooseTargetNode(node);
        playerTurn.UseAbility();
    }

    private void ClickedOnNode(Node node)
    {
        playerTurn.ChooseTargetNode(node);
        playerTurn.UseAbility();
    }

    private bool CheckInRange(Node node)
    {
        if (playerAbility == null)
            return false;

        int dis = Pathfinding.GetDistance(GameMaster.map, playerDestination, node);
        
        if (dis >= playerAbility.minRange && dis <= playerAbility.maxRange)
            return true;
        else
            return false;
    }
    #endregion

    #region Event Subscribers
    private void AbilitySubscriber(Node destination, Ability ability)
    {
        playerAbility = ability;
        playerDestination = destination;
    }

    private void ClearAbilitySubscriber()
    {
        playerAbility = null;
        playerDestination = null;
    }
    #endregion

    #region Utility
    private void MoveSelector(Node node)
    {
        nodeSelector.gameObject.SetActive(true);
        nodeSelector.position = node.GetStandingPoint();
    }

    private void HideSelector()
    {
        nodeSelector.gameObject.SetActive(false);
    }

    private void SetTargetNode(RaycastHit forecastHit)
    {
        Node node = forecastHit.transform.GetComponent<ForecastTile>().node;
        if (targetNode != node)
            highlighter.HidePossibleRoutes();
        targetNode = node;
    }

    private void SetTargetNode(Node node)
    {
        if (targetNode != node)
            highlighter.HidePossibleRoutes();
        targetNode = node;
    }

    private void HighlightAbility(Node node, Ability ability)
    {
        List<Node> nodes = ability.GetAreaTargeting(node);
        highlighter.Highlight(nodes, ForecastTile.ForecastState.AbilityForecast);
    }

    private void HighlightAbility(Node node)
    {
        List<Node> nodes = new List<Node>();
        nodes.Add(node);
        highlighter.Highlight(nodes, ForecastTile.ForecastState.AbilityForecast);
    }

    private void HighlightAbilityRange(Node node, Ability ability)
    {
        List<Node> nodes = ability.GetAbilityRange(node);
        highlighter.Highlight(nodes, ForecastTile.ForecastState.AbilityRange);
    }

    // Input Methods Used in Update()
    private RaycastHit GetClickData(LayerMask mask)
    {
        RaycastHit hit = new RaycastHit();
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out hit, 1000f, mask);
            if (hit.transform == null)
                Debug.Log("No Hit on Click");
        }
        return hit;
    }

    private RaycastHit GetMouseHoverData(LayerMask mask)
    {
        RaycastHit hit = new RaycastHit();
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out hit, 1000f, mask);
        }
        return hit;
    }
    #endregion
}

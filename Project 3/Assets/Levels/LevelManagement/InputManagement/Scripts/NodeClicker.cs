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

        Node node = GetClickData(LayerMask.GetMask("ForecastTile"));
        if (node == null)
            return;

        if (playerTurn.actionState == ActionState.ChoosingAction)
        {
            ClickedOnDestination(node);
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

        if (playerAbility.targetType == Ability.TargetType.SingleTarget)
        {
            SingleTarget();
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

        if (playerAbility.targetType == Ability.TargetType.DirectionalAOE)
        {
            //DirectionalAOE();
            return;
        }
    }
    #endregion

    #region Ability Types
    private void SingleTarget()
    {
        HighlightAbilityRange(playerDestination, playerAbility);

        Node hoverNode = GetMouseHoverData(LayerMask.GetMask("ForecastTile"));
        if (hoverNode == null)
            return;

        SetTargetNode(hoverNode);
        HighlightAbility(targetNode);
        if (Input.GetMouseButtonDown(0))
        {
            if (playerAbility.isUnitTarget)
                ClickedOnTarget(targetNode);
            else
                ClickedOnNode(targetNode);
        }
    }

    private void StandingAoe()
    {
        HighlightAbility(playerDestination, playerAbility);

        if (Input.GetMouseButtonDown(0))
        {
            SetTargetNode(playerDestination);
            ClickedOnNode(targetNode);
        }
    }

    private void DirectedAoe()
    {
        HighlightAbilityRange(playerDestination, playerAbility);

        Node hoverNode = GetMouseHoverData(LayerMask.GetMask("ForecastTile"));
        if (hoverNode == null)
            return;

        SetTargetNode(hoverNode);
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

    private void DirectionalAOE()
    {
        return;
        Node hoverNode = GetMouseHoverData(LayerMask.GetMask("ForecastTile"));
        if (hoverNode == null)
            return;

        SetTargetNode(hoverNode);

        Node direction = GetDirectionalNode();

        HighlightAbility(targetNode, playerAbility);
    }
    #endregion

    #region Selection Helper Methods
    private void ClickedOnDestination(Node node)
    {
        ForecastTile newTile = node.forecastTile;

        Unit selected = playerTurn.GetSelected();
        Grid<Node> map = gameMaster.GetMap();

        List<Node> routes = Pathfinding.GetAllRoutes(map, selected);
        if (!routes.Contains(node))
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

    private void ClickedOnTarget(Node node)
    {
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

    private void HighlightAbility(Node node)
    {
        List<Node> nodes = new List<Node>();
        nodes.Add(node);
        highlighter.Highlight(nodes, ForecastTile.ForecastState.AbilityForecast);
    }

    private void HighlightAbility(Node node, Ability ability)
    {
        List<Node> nodes = ability.GetAreaTargeting(node);
        highlighter.Highlight(nodes, ForecastTile.ForecastState.AbilityForecast);
    }

    private void HighlightAbility(Node node, Ability ability, Vector3 direction)
    {
        List<Node> nodes = ability.GetAreaTargeting(node);
        highlighter.Highlight(nodes, ForecastTile.ForecastState.AbilityForecast);
    }

    private void HighlightAbilityRange(Node node, Ability ability)
    {
        List<Node> nodes = ability.GetAbilityRange(node);
        highlighter.Highlight(nodes, ForecastTile.ForecastState.AbilityRange);
    }

    // Input Methods Used in Update()
    private Node GetClickData(LayerMask mask)
    {
        RaycastHit hit = new RaycastHit();
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out hit, 1000f, mask);
            if (hit.transform == null)
            {
                Debug.Log("No Hit on Click");
                return null;
            }
        }
        if (hit.transform == null)
            return null;
        return hit.transform.GetComponent<ForecastTile>().GetNode();
    }

    private Node GetMouseHoverData(LayerMask mask)
    {
        RaycastHit hit = new RaycastHit();
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out hit, 1000f, mask);
        }
        if (hit.transform == null)
            return null;
        return hit.transform.GetComponent<ForecastTile>().GetNode();
    }

    private Node GetDirectionalNode()
    {
        Vector3 direction = Pathfinding.GetDirection(playerDestination, targetNode);
        
        int x = playerDestination.x + (int)direction.x;
        int z = playerDestination.z + (int)direction.z;

        return GameMaster.map.GetGridObject(x, z);
        /*
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.z))
        {
            if (direction.x > 0)
                adjNode.x += 1;
            else
                adjNode.x += -1;
        }
        else if (Mathf.Abs(direction.x) < Mathf.Abs(direction.z))
        {
            if (direction.z > 0)
                adjNode.z += 1;
            else
                adjNode.z += -1;
        }
        else
        {
            if (direction.x > 0)
                adjNode.x += 1;
            else
                adjNode.x += -1;

            if (direction.z > 0)
                adjNode.z += 1;
            else
                adjNode.z += -1;
        }
        */

    }
    #endregion
}

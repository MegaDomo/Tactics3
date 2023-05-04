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

    private ForecastTile selectedTile;
    private ForecastTile previousTile;

    #region Update - Checks
    void Update()
    {
        CheckForDeselection();
        if (playerTurn.actionState == ActionState.ChoosingAction)
            return;
        //HighlightNode();
        CheckForNodeSelection();
    }
    
    private void CheckForNodeSelection()
    {
        // Selected Node to move to
        if (CombatState.state == BattleState.PLAYERTURN && playerTurn.actionState == ActionState.ChooseNode)
            ClickOnNode();

        // After Selecting Action, Now Selecting Target
        if (CombatState.state == BattleState.PLAYERTURN && playerTurn.actionState == ActionState.ChoosingTarget)
            ClickOnTarget();
    }
    #endregion

    private void CheckForDeselection()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (selectedTile != null) selectedTile.HideTile();
            playerTurn.ClearNode();
            HideSelector();
        }
    }

    #region Choosing A Node
    private void ClickOnNode()
    {
        RaycastHit forecastHit = GetClickData(LayerMask.GetMask("ForecastTile"));
        if (forecastHit.transform != null)
        {
            ClickedOnForecastTile(forecastHit);
            return;
        }
    }

    private void ClickedOnForecastTile(RaycastHit forecastHit)
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

            selectedTile.DeselectedThisTile();
            selectedTile = newTile;
            selectedTile.SelectedThisTile();

            Node node = selectedTile.GetNode();
            playerTurn.ChooseNode(node);
            MoveSelector(node);
        }
        else
        {
            Node node = selectedTile.GetNode();
            playerTurn.ChooseNode(node);
            MoveSelector(node);
        }
    }
    #endregion

    #region Choosing Target
    private void ClickOnTarget()
    {
        RaycastHit targetHit = GetClickData(LayerMask.GetMask("Ground"));
        if (targetHit.transform != null)
        {
            
            return;
        }
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
    #endregion
}

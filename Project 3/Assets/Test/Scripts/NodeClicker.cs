using UnityEngine;
using UnityEngine.EventSystems;

public class NodeClicker : MonoBehaviour
{
    [Header("Scriptable Objects References")]
    public PlayerTurn player;

    [Header("Unity References")]
    public Transform nodeSelector;

    private ForecastTile selectedTile;
    private ForecastTile previousTile;

    void Update()
    {
        CheckForDeselection();
        if (player.actionState == ActionState.ChoosingAction)
            return;
        //HighlightNode();
        CheckForNodeSelection();
    }
    
    private void CheckForNodeSelection()
    {
        if (CombatState.state != BattleState.PLAYERTURN && player.actionState != ActionState.ChooseNode)
            return;
        // TODO : incorporate clicking on enemies to quickly hit enemies
        /*RaycastHit unitHit = GetClickData(LayerMask.GetMask("Enemy"));
        if (unitHit.transform != null)
        {
            ClickedOnUnit(unitHit);
            return;
        }*/
        
        RaycastHit forecastHit = GetClickData(LayerMask.GetMask("ForecastTile"));
        if (forecastHit.transform != null)
        {
            ClickedOnForecastTile(forecastHit);
            return;
        }

    }

    private void CheckForDeselection()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (selectedTile != null) selectedTile.HideTile();
            player.ClearNode();
            HideSelector();
        }
    }

    private void ClickedOnForecastTile(RaycastHit forecastHit)
    {
        ForecastTile newTile = forecastHit.transform.GetComponent<ForecastTile>();
        if (newTile != selectedTile)
        {
            if (selectedTile == null)
                selectedTile = newTile;

            selectedTile.DeselectedThisTile();
            selectedTile = newTile;
            selectedTile.SelectedThisTile();

            Node node = selectedTile.GetNode();
            player.ChooseNode(node);
            MoveSelector(node);
        }
    }

    private void ClickedOnUnit(RaycastHit unitHit)
    {

    }



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

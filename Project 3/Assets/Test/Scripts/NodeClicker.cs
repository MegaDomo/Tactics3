using UnityEngine;
using UnityEngine.EventSystems;

public class NodeClicker : MonoBehaviour
{
    [Header("Unity References")]
    public Transform nodeSelector;
    public PlayerTurn player;

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
    
    private void HighlightNode()
    {
        RaycastHit hit = GetHoverData(LayerMask.GetMask("ForecastTile"));
        if (hit.transform == null) return;

        ForecastTile fTile = hit.transform.GetComponent<ForecastTile>();
        if (previousTile == null)
        {
            previousTile = fTile;
        }
        if (previousTile != fTile && previousTile != selectedTile)
        {
            previousTile.HideTile();
            previousTile = fTile;
        }

        MoveSelector(fTile.node);
        fTile.Highlight();
    }

    private void MoveSelector(Node node)
    {
        nodeSelector.gameObject.SetActive(true);
        nodeSelector.position = node.GetStandingPoint();
    }

    private void HideSelector()
    {
        nodeSelector.gameObject.SetActive(false);
    }

    private void CheckForNodeSelection()
    {
        if (CombatState.state != BattleState.PLAYERTURN && player.actionState != ActionState.ChooseNode)
            return;

        RaycastHit hit = GetClickData();
        if (hit.transform == null)
            return;

        if (hit.transform.gameObject.tag != "ForecastTile")
            return;

        ForecastTile newTile = hit.transform.GetComponent<ForecastTile>();
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

    private void CheckForDeselection()
    {
        if (Input.GetMouseButtonDown(1))
        {
            selectedTile.HideTile();
            player.ClearNode();
            HideSelector();
        }
    }

    // Input Methods Used in Update()
    private RaycastHit GetClickData()
    {
        RaycastHit hit = new RaycastHit();
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out hit, 1000f);
            if (hit.transform == null)
                Debug.Log("No Hit on Click");
        }
        return hit;
    }

    private RaycastHit GetHoverData(LayerMask mask)
    {
        RaycastHit hit = new RaycastHit();
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out hit, 1000f, mask);
            if (hit.transform == null)
                Debug.Log("No Hit on Click");
        }
        return hit;
    }
}

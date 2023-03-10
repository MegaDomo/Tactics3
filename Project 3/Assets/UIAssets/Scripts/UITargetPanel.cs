using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UITargetPanel : MonoBehaviour
{
    [Header("Unity References")]
    public UIManager uiManager;
    public List<Button> targetButtons;

    private PlayerTurn player;
    private Unit selected;
    private List<Unit> targets;
    
    private void OnEnable()
    {
        if (player == null)
        {
            player = PlayerTurn.instance;
            return;
        }
        selected = player.GetSelected();
        SetTargets();
        TurnOffRemainingButtons();
    }

    private void SetTargets()
    {
        FindTargets();

        for (int i = 0; i < targets.Count; i++)
            ConfigureButtons(i, targetButtons[i], targets[i]);
    }

    private void FindTargets()
    {
        Node destination = player.GetDestination();
        List<Node> adjNodes = MapManager.instance.pathing.GetNeighbors(destination);

        foreach (Node node in adjNodes)
        {
            if (node.unit != null)
                targets.Add(node.unit);
        }        
    }

    private void ConfigureButtons(int index, Button button, Unit target)
    {
        TextMeshProUGUI text = button.transform.GetChild(index).GetComponent<TextMeshProUGUI>();
        text.SetText(target.name);
    }

    private void TurnOffRemainingButtons()
    {
        for (int i = targets.Count - 1; i < targetButtons.Count; i++)
            targetButtons[i].gameObject.SetActive(false);
    }

    private void CloseTargetPanel()
    {
        player.WeaponStrike();
        uiManager.SetActiveTargetPanel(false);
    }

    public void SetActiveTargetPanel(bool value)
    {
        gameObject.SetActive(value);
    }

    #region Event Handlers
    public void Target1Button()
    {
        player.SetTarget(targets[0]);
        CloseTargetPanel();
    }
    public void Target2Button()
    {
        player.SetTarget(targets[1]);
        CloseTargetPanel();
    }
    public void Target3Button()
    {
        player.SetTarget(targets[2]);
        CloseTargetPanel();
    }
    public void Target4Button()
    {
        player.SetTarget(targets[3]);
        CloseTargetPanel();
    }
    #endregion
}

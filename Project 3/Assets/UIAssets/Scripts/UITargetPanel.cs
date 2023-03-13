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

    #region Finds Targets
    public void Setup()
    {
        if (player == null)
            player = PlayerTurn.instance;

        selected = player.GetSelected();
        SetTargets();
        TurnOffRemainingButtons();
    }

    private void SetTargets()
    {
        FindTargets();

        for (int i = 0; i < targets.Count; i++)
            ConfigureButtons(targetButtons[i], targets[i]);
    }

    private void FindTargets()
    {
        Node destination = player.GetDestination();
        List<Node> adjNodes = Pathfinding.GetNeighbors(MapManager.instance.GetMap(), destination);

        targets = new List<Unit>();
        foreach (Node node in adjNodes)
        {
            Unit unit = node.unit;
            if (unit == null)
                continue;
            if (unit.unitType != Unit.UnitType.Enemy)
                continue;
            targets.Add(node.unit);
        }
    }

    private void ConfigureButtons(Button button, Unit target)
    {
        TextMeshProUGUI text = button.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        text.SetText(target.name);
    }

    private void TurnOffRemainingButtons()
    {
        for (int i = targets.Count; i < targetButtons.Count; i++)
            targetButtons[i].gameObject.SetActive(false);
    }
    #endregion

    #region Setters
    public void SetActiveTargetPanel(bool value)
    {
        gameObject.SetActive(value);
    }
    #endregion

    #region Event Handlers
    public void Target1Button()
    {
        player.SetTarget(targets[0]);
        WeaponStrike();
    }
    public void Target2Button()
    {
        player.SetTarget(targets[1]);
        WeaponStrike();
    }
    public void Target3Button()
    {
        player.SetTarget(targets[2]);
        WeaponStrike();
    }
    public void Target4Button()
    {
        player.SetTarget(targets[3]);
        WeaponStrike();
    }
    #endregion

    #region Utility
    private void WeaponStrike()
    {
        player.WeaponStrike();
    }
    #endregion
}

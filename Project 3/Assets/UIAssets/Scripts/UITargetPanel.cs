using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UITargetPanel : MonoBehaviour
{
    [Header("Scriptable Object References")]
    public GameMaster gameMaster;
    public PlayerTurn playerTurn;

    [Header("Unity References")]
    public List<Button> targetButtons;

    private Unit selected;
    private List<Unit> targets;

    #region Finds Targets
    public void Setup()
    {
        selected = playerTurn.GetSelected();
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
        Node destination = playerTurn.GetDestination();
        List<Node> adjNodes = Pathfinding.GetNeighbors(gameMaster.GetMap(), destination);

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
        playerTurn.SetTarget(targets[0]);
        WeaponStrike();
    }
    public void Target2Button()
    {
        playerTurn.SetTarget(targets[1]);
        WeaponStrike();
    }
    public void Target3Button()
    {
        playerTurn.SetTarget(targets[2]);
        WeaponStrike();
    }
    public void Target4Button()
    {
        playerTurn.SetTarget(targets[3]);
        WeaponStrike();
    }
    #endregion

    #region Utility
    private void WeaponStrike()
    {
        playerTurn.WeaponStrike();
    }
    #endregion
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TargetPanel : MonoBehaviour
{
    [Header("Unity References")]
    public GameObject targetPanel;
    public List<Button> targetButtons;

    private PlayerTurn player;
    private Unit selected;
    private List<Unit> targets;

    private void Start()
    {
        player = PlayerTurn.instance;
    }

    private void OnEnable()
    {
        selected = player.GetSelected();
        SetTargets();
    }

    private void SetTargets()
    {
        FindTargets();

        for (int i = 0; i < targets.Count; i++)
            ConfigureButtons(i, targetButtons[i], targets[i]);

        TurnOffRemainingButtons();
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
        gameObject.SetActive(false);
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

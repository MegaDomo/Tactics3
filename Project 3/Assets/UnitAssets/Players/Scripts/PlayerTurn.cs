using System;
using UnityEngine;

public enum ActionState { ChooseNode, ChoosingAction, CannotChoose }

[CreateAssetMenu(fileName = "NewPlayerTurnManager", menuName = "Managers/Player Turn Manager")]
public class PlayerTurn : ScriptableObject
{
    [Header("Scriptable Object References")]
    public BattleSystem battleSystem;

    [Header("Unity References")]
    public NodeHighlighter nodeHighlighter;
    public UIManager ui;

    [HideInInspector] public ActionState actionState;

    public Action selectedNodeEvent;
    public Action endTurnEvent;

    private Unit selected;
    private Unit target;
    private Node destination;

    private void OnEnable()
    {
        battleSystem.playerTurnEvent += StartTurn;
        
    }

    #region ActionStateMethods
    public void StartTurn(Unit unit)
    {
        actionState = ActionState.ChooseNode;
        SetSelected(unit);
        nodeHighlighter.Highlight(selected);
    }

    public void EndTurn()
    {
        actionState = ActionState.CannotChoose;
        nodeHighlighter.Unhighlight();
        ClearValues();
        endTurnEvent.Invoke();
    }

    public void ChooseNode(Node node)
    {
        destination = node;
        actionState = ActionState.ChoosingAction;
        selectedNodeEvent.Invoke();
        nodeHighlighter.Unhighlight();
    }

    public void ClearNode()
    {
        destination = null;
        actionState = ActionState.ChooseNode;
        ui.CloseAllPanels();
        nodeHighlighter.Highlight(selected);
    }

    public void ChooseTarget(Unit target)
    {
        this.target = target;
        actionState = ActionState.ChoosingAction;
        nodeHighlighter.Unhighlight();
    }
    
    #endregion

    #region Player Functions
    public void PlayerMove()
    {
        if (CombatState.state != BattleState.PLAYERTURN)
            return;
        selected.Move(destination);
    }

    public void WeaponStrike()
    {
        if (CombatState.state != BattleState.PLAYERTURN)
            return;

        if (target == null || destination == null || selected == null)
        {
            Debug.Log("Something was Null during WeaponStrike." + "\nTarget: "
                      + target + "\nDestination: " + destination + "\nSelected: " + selected);
            return;
        }

        if (selected.IsMoving())
            return;

        selected.SetIsAttacking(true);
        selected.Move(destination);
    }
    #endregion

    #region Utility
    public void DealDamage()
    {
        target.TakeDamage(selected, selected.equippedWeapon);
    }

    private void ClearValues()
    {
        selected = null;
        target = null;
        destination = null;
    }
    #endregion

    #region Getters & Setters
    public Unit GetSelected()
    {
        return selected;
    }
    public void SetSelected(Unit _selected)
    {
        selected = _selected;
    }

    public Unit GetTarget()
    {
        return target;
    }

    public void SetTarget(Unit target)
    {
        this.target = target;
    }

    public Node GetDestination()
    {
        return destination;
    }

    public void SetDestination(Node node)
    {
        destination = node;
    }
    #endregion
}

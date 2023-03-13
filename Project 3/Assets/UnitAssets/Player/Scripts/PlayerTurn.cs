using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ActionState { ChooseNode, ChoosingAction, CannotChoose }

public class PlayerTurn : MonoBehaviour
{
    #region Singleton
    public static PlayerTurn instance;

    private void Awake()
    {
        instance = this;
    }
    #endregion

    [Header("Unity References")]
    public NodeHighlighter nodeHighlighter;
    public UIManager ui;

    [HideInInspector] public ActionState actionState;

    private Unit selected;
    private Unit target;
    private Node destination;

    private bool isMoving;
    private bool isAttacking;

    #region ActionStateMethods
    public void StartTurn()
    {
        actionState = ActionState.ChooseNode;
        nodeHighlighter.Highlight(selected);
    }

    public void EndTurn()
    {
        actionState = ActionState.CannotChoose;
        nodeHighlighter.Unhighlight();
        ClearValues();
        BattleSystem.instance.GetNextInitiative();
    }

    public void ChooseNode(Node node)
    {
        destination = node;
        actionState = ActionState.ChoosingAction;
        ui.SetActiveHotBar(true);
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

        if (isMoving)
            return;

        SetIsAttacking(true);
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
        isMoving = false;
        isAttacking = false;
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

    public bool IsMoving()
    {
        return isMoving;
    }

    public void SetIsMoving(bool value)
    {
        isMoving = value;
    }

    public bool IsAttacking()
    {
        return isAttacking;
    }

    public void SetIsAttacking(bool value)
    {
        isAttacking = value;
    }
    #endregion
}

using System;
using System.Collections.Generic;
using UnityEngine;

public enum ActionState { ChoosingAction, ChoosingTarget, CannotChoose }

[CreateAssetMenu(fileName = "NewPlayerTurnManager", menuName = "Managers/Player Turn Manager")]
public class PlayerTurn : ScriptableObject
{
    [Header("Scriptable Object References")]
    public BattleSystem battleSystem;

    [HideInInspector] public ActionState actionState;

    // Player Events
    public Action endTurnEvent;

    // Node Events
    public Action<Unit> selectedNodeEvent;
    public Action<Unit> deselectedNodeEvent;
    public Action<Node, Ability> choseAbilityEvent;
    public Action clearAbilityEvent;
    public Action<Node, Ability> targetChosenEvent;
    public Action clearTargetChosenEvent;

    private Unit selected;
    private Unit target;
    private Node destination;
    private Node nodeTarget;
    private Ability chosenAbility;
    
    private void OnEnable()
    {
        battleSystem.playerTurnEvent += StartTurn;
    }

    private void OnDisable()
    {
        battleSystem.playerTurnEvent -= StartTurn;
    }

    #region ActionStateMethods
    public void StartTurn(Unit unit)
    {
        actionState = ActionState.ChoosingAction;
        if (unit == null) Debug.Log("No Player Unit is Selected");
        SetSelected(unit);
        destination = unit.node;
    }

    public void EndTurn()
    {
        actionState = ActionState.CannotChoose;
        ClearValues();
        endTurnEvent?.Invoke();
    }

    public void ChooseNode(Node node)
    {
        destination = node;
        selectedNodeEvent?.Invoke(selected);
    }

    public void ClearNode()
    {
        destination = selected.node;
        deselectedNodeEvent?.Invoke(selected);
    }

    public void ChooseAbility(Ability ability)
    {
        chosenAbility = ability;
        actionState = ActionState.ChoosingTarget;
        choseAbilityEvent?.Invoke(destination, chosenAbility);
    }

    public void ClearAbiliity()
    {
        chosenAbility = null;
        actionState = ActionState.ChoosingAction;
        clearAbilityEvent?.Invoke();
    }

    public bool HaveAbility()
    {
        return chosenAbility != null;
    }

    public void ChooseTarget(Unit target)
    {
        this.target = target;
    }

    public void CleartTarget()
    {
        target = null;
    }

    public void ChooseTargetNode(Node node)
    {
        nodeTarget = node;
        targetChosenEvent?.Invoke(nodeTarget, chosenAbility);
    }

    public void ClearTargetNode()
    {
        nodeTarget = null;
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

    public void UseAbility()
    {
        if (CombatState.state != BattleState.PLAYERTURN)
            return;
        selected.MoveAndUseAbility(destination, chosenAbility);
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

    public Node GetTargetNode()
    {
        return nodeTarget;
    }

    public Node GetDestination()
    {
        return destination;
    }

    public void SetDestination(Node node)
    {
        destination = node;
    }

    public Ability GetChosenAbility()
    {
        return chosenAbility;
    }

    public void SetChosenAbility(Ability ability)
    {
        chosenAbility = ability;
    }
    #endregion
}

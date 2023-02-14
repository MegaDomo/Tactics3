using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ActionState { MoveAction, ChoosingAction, CannotChoose }

public class PlayerTurn : MonoBehaviour
{
    #region Singleton
    public static PlayerTurn instance;

    private void Awake()
    {
        instance = this;
    }
    #endregion

    private Unit selected;
    private Unit targeted;
    private Node targetNode;

    public ActionState actionState;

    #region ActionStateMethods
    public void StartTurn()
    {
        actionState = ActionState.MoveAction;
    }

    public void EndTurn()
    {
        actionState = ActionState.CannotChoose;
    }

    public void ChooseNode(Node node)
    {
        targetNode = node;
        actionState = ActionState.ChoosingAction;
    }

    public void ClearNode()
    {
        targetNode = null;
        actionState = ActionState.MoveAction;
    }
    
    #endregion

    #region Player Functions
    public void PlayerMove()
    {
        // If players turn
        if (CombatState.state != BattleState.PLAYERTURN)
            return;
        if (selected == null) Debug.Log("Selected");
        if (targetNode == null) Debug.Log("Node");
        MapManager.instance.Move(selected, targetNode);
    }
    #endregion

    #region Getters & Setters
    public Unit GetSelected()
    {
        return selected;
    }
    public Unit GetTargeted()
    {
        return targeted;
    }


    public Node GetTargetedNode()
    {
        return targetNode;
    }

    public void SetSelected(Unit _selected)
    {
        selected = _selected;
    }

    public void SetTargeted(Unit _targeted)
    {
        targeted = _targeted;
    }

    public void SetTargetedNode(Node _node)
    {
        targetNode = _node;
    }
    #endregion
}

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

    [HideInInspector] public ActionState actionState;

    private Unit selected;
    private Unit targeted;
    private Node targetNode;

    private bool isMoving;

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
    }

    public void ChooseNode(Node node)
    {
        targetNode = node;
        actionState = ActionState.ChoosingAction;
        nodeHighlighter.Unhighlight();
    }

    public void ClearNode()
    {
        targetNode = null;
        actionState = ActionState.ChooseNode;
        nodeHighlighter.Highlight(selected);
    }
    
    #endregion

    #region Player Functions
    public void PlayerMove()
    {
        if (CombatState.state != BattleState.PLAYERTURN)
            return;
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

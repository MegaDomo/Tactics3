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
    public void PlayerMove(Node destination)
    {
        // If players turn
        if (CombatState.state != BattleState.PLAYERTURN)
            return;

        // TODO : Open up a Something / confirm window, then the Event handler of the "Yes" Button will call the Method Below
        MapManager.instance.Move(selected, destination);
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

    public void SetTargeted(Unit _targeted)
    {
        targeted = _targeted;
    }
    #endregion
}

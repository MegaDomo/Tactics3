using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTurn : MonoBehaviour
{
    #region Singleton
    public static PlayerTurn instance;

    private void Awake()
    {
        instance = this;
    }
    #endregion

    [HideInInspector] public Unit selected;

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



    #region Setters
    public void SetSelected(Unit _selected)
    {
        selected = _selected;
    }
    #endregion
}

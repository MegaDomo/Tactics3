using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST}
public static class CombatState
{
    public static BattleState state;

    #region Setters
    public static void MakeStateStart()
    {
        state = BattleState.START;
    }
    public static void MakeStatePlayerTurn()
    {
        state = BattleState.PLAYERTURN;
        Debug.Log("Player Turn");
    }
    public static void MakeStateEnemyTurn()
    {
        state = BattleState.ENEMYTURN;
        Debug.Log("Enemy Turn");
    }
    public static void MakeStateWon()
    {
        state = BattleState.WON;
    }
    public static void MakeStateLost()
    {
        state = BattleState.LOST;
    }
    #endregion
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// In Control of the whole enemy team
public class EnemyAI : MonoBehaviour
{
    #region Singleton
    public static EnemyAI instance;

    private void Awake()
    {
        instance = this;
    }
    #endregion

    private Unit selected;

    private bool isMoving;
    private bool isAttacking;

    public IEnumerator StartTurn()
    {
        Behavior bhvr = selected.behavior;
        bhvr.self = selected;
        bhvr.TakeTurn();

        yield return new WaitForSeconds(1f);

        // Change this \/
        EndTurn();
    }

    public void EndTurn()
    {
        BattleSystem.instance.GetNextInitiative();
    }

    #region Setters
    public void SetSelected(Unit _selected)
    {
        selected = _selected;
    }
    #endregion
}

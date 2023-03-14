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
    private Behavior behavior;
    private bool isMoving;
    private bool isAttacking;

    public IEnumerator StartTurn()
    {
        behavior = selected.behavior;
        behavior.self = selected;
        behavior.TakeTurn();

        yield return new WaitForSeconds(1f);
    }

    public void EndTurn()
    {
        BattleSystem.instance.GetNextInitiative();
    }

    #region Utility
    public void DealDamage()
    {
        if (behavior.target == null)
            return;
        behavior.target.TakeDamage(selected, selected.equippedWeapon);
    }
    #endregion
    #region Setters
    public void SetSelected(Unit _selected)
    {
        selected = _selected;
    }
    #endregion
}

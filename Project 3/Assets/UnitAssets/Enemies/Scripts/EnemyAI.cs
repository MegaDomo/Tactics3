using System;
using UnityEngine;

// In Control of the whole enemy team
[CreateAssetMenu(fileName = "NewEnemyAI", menuName = "Managers/Enemy AI")]
public class EnemyAI : ScriptableObject
{
    [Header("Scriptable Object References")]
    public BattleSystem battleSystem;

    private Unit selected;
    private Behavior behavior;
    private bool isMoving;
    private bool isAttacking;

    public Action endTurnEvent;

    private void OnEnable()
    {
        battleSystem.enemyTurnEvent += StartTurn;    
    }

    public void StartTurn(Unit unit)
    {
        selected = unit;
        behavior = selected.behavior;
        behavior.TakeTurn();
    }

    public void EndTurn()
    {
        endTurnEvent.Invoke();
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

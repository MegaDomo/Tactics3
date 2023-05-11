using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAnimationEventHandler : MonoBehaviour
{
    [Header("Scriptable Objects References")]
    public PlayerTurn playerTurn;
    public EnemyAI enemyAI;

    [Header("Unity References")]
    public Unit parentUnitScript;

    public void BasicAttack()
    {
        if (parentUnitScript.unitType == Unit.UnitType.Player)
            playerTurn.DamageCall();
        if (parentUnitScript.unitType == Unit.UnitType.Enemy)
            return;
    }
}

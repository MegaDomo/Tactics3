using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAnimationEventHandler : MonoBehaviour
{
    [Header("Unity References")]
    public Unit parentUnitScript;

    public void BasicAttack()
    {
        if (parentUnitScript.unitType == Unit.UnitType.Player)
            PlayerTurn.instance.DealDamage();
        if (parentUnitScript.unitType == Unit.UnitType.Enemy)
            EnemyAI.instance.DealDamage();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitBehavior
{
    [HideInInspector] public Behavior behavior;

    private Unit unit;

    public UnitBehavior(Unit unit)
    {
        this.unit = unit;
        SetBehavior();
        SetSelf();
    }

    private void SetSelf()
    {
        if (unit.unitType == Unit.UnitType.Enemy)
        {
            unit.gameObject.tag = "Enemy";
            behavior.self = unit;
        }

        if (unit.unitType == Unit.UnitType.Player)
        {
            unit.gameObject.tag = "Player";
        }
    }

    private void SetBehavior()
    {
        MapManager map = MapManager.instance;

        if (unit.enemyObj.behaviorType == EnemyObject.BehaviorType.Attacker)
            behavior = new Attacker(map);

        if (unit.enemyObj.behaviorType == EnemyObject.BehaviorType.Killer)
            behavior = new Killer();
        // TODO : Add other Behaviors and compensate their constructors
    }
}

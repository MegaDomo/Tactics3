using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitBehavior
{
    private Unit unit;
    private Behavior behavior;
    private EnemyObject.BehaviorType behaviorType;

    public UnitBehavior(Unit unit, EnemyObject enemyObject)
    {
        this.unit = unit;
        behaviorType = enemyObject.behaviorType;

        SetBehavior();
        behavior.self = unit;
    }

    private void SetBehavior()
    {
        MapManager map = MapManager.instance;

        if (behaviorType == EnemyObject.BehaviorType.Attacker)
            behavior = new Attacker(map, unit);

        if (behaviorType == EnemyObject.BehaviorType.Killer)
            behavior = new Killer();
        // TODO : Add other Behaviors and compensate their constructors
    }

    public Behavior GetBehavior()
    {
        return behavior;
    }
}

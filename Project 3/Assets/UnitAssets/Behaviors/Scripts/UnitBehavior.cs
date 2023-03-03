using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitBehavior : MonoBehaviour
{
    public enum BehaviorType { Attacker, Killer }

    [Header("Attributes")]
    public BehaviorType behaviorType;

    [HideInInspector] public Behavior behavior;

    private Unit unit;

    private void Start()
    {
        unit = GetComponent<Unit>();
        SetBehavior();
        SetSelf();
    }

    private void SetSelf()
    {
        if (unit.unitType == Unit.UnitType.Enemy)
        {
            gameObject.tag = "Enemy";
            behavior.self = unit;
        }

        if (unit.unitType == Unit.UnitType.Player)
        {
            gameObject.tag = "Player";
        }
    }

    private void SetBehavior()
    {
        MapManager map = MapManager.instance;

        if (behaviorType == BehaviorType.Attacker)
            behavior = new Attacker(map);

        if (behaviorType == BehaviorType.Killer)
            behavior = new Killer();
        // TODO : Add other Behaviors and compensate their constructors
    }
}

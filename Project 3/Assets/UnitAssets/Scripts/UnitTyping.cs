using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This Class is for the Inspector to Configure a unit
public class UnitTyping : MonoBehaviour
{
    
    public Unit.UnitType unitType;

    // If Player


    // If Enemy
    public EnemyObject enemyObject;

    // ETC

    private void Start()
    {
        Unit unit = GetComponent<Unit>();
        unit.unitType = unitType;
        unit.enemyObj = enemyObject;
    }
}

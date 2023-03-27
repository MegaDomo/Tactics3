using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Which Enemy")]
    public EnemyObject enemyObject;

    [HideInInspector] public Behavior behavior;

    public void SetupEnemy(Unit unit)
    {
        EnemyObject enemyObject = GetComponent<Enemy>().enemyObject;
        if (enemyObject == null)
        {
            Debug.Log("No Enemy Data to Set for: " + name);
            return;
        }

        gameObject.tag = "Enemy";
        UnitBehavior unitBehavior = new UnitBehavior(unit, enemyObject);
        behavior = unitBehavior.GetBehavior();
    }

    #region Getters & Setters
    public EnemyObject GetEnemyObject()
    {
        return enemyObject;
    }

    public void SetEnemyObject(EnemyObject obj)
    {
        enemyObject = obj;
    }

    public Unit GetTauntedPlayer()
    {
        return behavior.GetTauntedPlayer();
    }

    public void SetTauntedPlayer(Unit tauntedPlayer)
    {
        behavior.SetTauntedPlayer(tauntedPlayer);
    }

    public void ClearTauntedPlayer()
    {
        behavior.ClearTauntedPlayer();
    }
    #endregion
}



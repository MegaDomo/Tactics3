using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemy", menuName = "Managers/Enemy")]
public class Enemy : ScriptableObject
{
    [Header("Scriptable Object References")]
    public GameMaster gameMaster;

    [Header("Which Enemy")]
    public EnemyObject enemyObject;

    [HideInInspector] public Behavior behavior;

    public void SetupEnemy(Unit unit)
    {
        if (enemyObject == null)
        {
            Debug.Log("No Enemy Data to Set for: " + name);
            return;
        }

        UnitBehavior unitBehavior = new UnitBehavior(unit, gameMaster.GetMap(), enemyObject);
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



using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Manager ScriptableObject
[CreateAssetMenu(fileName = "NewBattleSystem", menuName = "Managers/Battle System")]
public class BattleSystem : ScriptableObject
{
    [Header("Scriptable Object References")]
    public GameMaster gameMaster;
    public SpawnManager spawnManager;
    public PlayerTurn playerTurn;
    public EnemyAI enemyAI;

    [HideInInspector] public List<Unit> units;
    [HideInInspector] public List<Unit> players;
    [HideInInspector] public List<Unit> enemies;
    [HideInInspector] public Queue<Unit> initiative = new Queue<Unit>();

    public Action<Unit> playerTurnEvent;
    public Action<Unit> enemyTurnEvent;

    public Action victoryEvent;
    public Action defeatEvent;

    public bool isInCombat;

    private void OnEnable()
    {
        gameMaster.startCombatEvent += Setup;
        playerTurn.endTurnEvent += GetNextInitiative;
        enemyAI.endTurnEvent += GetNextInitiative;
    }

    private void OnDisable()
    {
        gameMaster.startCombatEvent -= Setup;
        playerTurn.endTurnEvent -= GetNextInitiative;
        enemyAI.endTurnEvent -= GetNextInitiative;
    }

    public void Setup(List<Unit> units)
    {
        this.units = units;
        SetPlayersAndEnemies(units);
        QueueUp();
        StartBattle();
    }

    public void StartBattle()
    {
        isInCombat = true;
        GetNextInitiative();
    }

    public void QueueUp()
    {
        // TODO : Incorporate current speed stat
        // Enqueues
        for (int i = 0; i < units.Count; i++)
            initiative.Enqueue(units[i]);
    }

    public void GetNextInitiative()
    {
        // TODO : Check if one Side has been wiped
        // Extra Script (Maybe) that listens for Unit OnDeath Events
        // Uses Comparative Based Search to remove from list, upon empty list fire
        // respective Event

        if (!isInCombat)
            return;

        Unit next;
        if (!initiative.TryPeek(out next))
            return;

        next = initiative.Dequeue();

        next.StartTurn();

        // Gives the Unit to the Respected AI
        switch (next.unitType)
        {
            case Unit.UnitType.Player:
                CombatState.MakeStatePlayerTurn();
                playerTurnEvent?.Invoke(next);
                break;
            case Unit.UnitType.Enemy:
                CombatState.MakeStateEnemyTurn();
                enemyTurnEvent?.Invoke(next);
                break;
            default:
                break;
        }

        // Check if the queue ran out
        if (initiative.Count == 0)
            QueueUp();
    }



    #region Utility
    public void SetPlayersAndEnemies(List<Unit> units)
    {
        players = new List<Unit>();
        enemies = new List<Unit>();
        foreach (Unit unit in units)
        {
            if (unit.unitType == Unit.UnitType.Player)
                players.Add(unit);
            if (unit.unitType == Unit.UnitType.Enemy)
                enemies.Add(unit);
        }
    }

    public void Victory()
    {
        isInCombat = false;
        victoryEvent.Invoke();
    }

    public void Defeat()
    {
        defeatEvent.Invoke();
    }
    #endregion

    #region Getters & Setters
    public List<Unit> GetPlayers()
    {
        return players;
    }

    public List<Unit> GetEnemies()
    {
        return enemies;
    }
    #endregion
}

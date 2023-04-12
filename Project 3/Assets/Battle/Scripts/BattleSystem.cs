using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Manager ScriptableObject
[CreateAssetMenu(fileName = "NewBattleSystem", menuName = "Managers/Battle System")]
public class BattleSystem : ScriptableObject
{
    [Header("Scriptable Object References")]
    public GameMaster gameMaster;
    public PlayerTurn playerTurn;
    public EnemyAI enemyAI;

    [HideInInspector] public List<Unit> units;
    [HideInInspector] public List<Unit> players;
    [HideInInspector] public List<Unit> enemies;
    [HideInInspector] public Queue<Unit> initiative = new Queue<Unit>();

    public void SetUp()
    {
        QueueUp();
        StartBattle();
    }

    public void StartBattle()
    {
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
                playerTurn.SetSelected(next);
                playerTurn.StartTurn();
                break;
            case Unit.UnitType.Enemy:
                CombatState.MakeStateEnemyTurn();
                enemyAI.SetSelected(next);
                enemyAI.StartTurn();
                break;
            default:
                break;
        }

        // Check if the queue ran out
        if (initiative.Count == 0)
            QueueUp();
    }

    public void SetPlayersAndEnemies(List<Unit> players, List<Unit> enemies)
    {
        this.players = players;
        this.enemies = enemies;
        units = new List<Unit>();
        
        foreach (Unit unit in players)
            units.Add(unit);
        foreach (Unit unit in enemies)
            units.Add(unit);
    }

    #region Getters & Setters
    public List<Unit> GetPlayers()
    {
        return players;
    }
    #endregion
}

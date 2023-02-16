using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Determines whose going in Initiative
public class BattleSystem : MonoBehaviour
{
    #region Singleton
    public static BattleSystem instance;

    void Awake()
    {
        instance = this;
    }
    #endregion

    [Header("Unity References")]
    public MapMaker maker;
    public MapManager mapManager;
    public SpawnManager spawner;

    [Header("Fill Players")]
    public List<Unit> units;
    public Queue<Unit> initiative = new Queue<Unit>();

    [HideInInspector] public List<Unit> players;
    [HideInInspector] public List<Unit> enemies;

    public void SetUp()
    {
        GetPlayersAndEnemies();
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
        switch (next.type)
        {
            case "Player":
                CombatState.MakeStatePlayerTurn();
                PlayerTurn.instance.SetSelected(next);
                PlayerTurn.instance.StartTurn();
                break;
            case "Enemy":
                CombatState.MakeStateEnemyTurn();
                EnemyAI.instance.SetSelected(next);
                StartCoroutine(EnemyAI.instance.StartTurn());
                break;
            default:
                break;
        }

        // Check if the queue ran out
        if (initiative.Count == 0)
            QueueUp();
    }

    private void GetPlayersAndEnemies()
    {
        players = new List<Unit>();
        enemies = new List<Unit>();

        foreach (Unit unit in units)
        {
            if (unit.type == "Player")
                players.Add(unit);
            if (unit.type == "Enemy")
                enemies.Add(unit);
        }
    }
}

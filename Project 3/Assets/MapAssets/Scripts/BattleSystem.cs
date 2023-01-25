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
    [Header("Fill Players")]
    public List<Unit> units;
    public Queue<Unit> initiative = new Queue<Unit>();

    // TODO : called different
    void Start()
    {
        // Creates the Map
        // TODO : Determine if Static Map or RNG Map
        maker.Setup();

        // Spawn Unit on the Map : When units are spawned add to list
        // TODO :

        // Get Initiatives
        QueueUp();

        // Start the Battle
        StartBattle();
    }

    private void SpawnUnits()
    {
        //MapManager.instance.Place(units[2], MapManager.instance.map[4][4]);
        // TODO : Call 2 Scripts/Methods
        // - is the player handler - which is holding onto the loudouts that the player chose for their units
        // it then creates those units maybe in a Create Unit Script
        // - Enemy Spawn Manager/ Creater, maybe pulls from a pool of units and adds them to the list
    }


    private void QueueUp()
    {
        // TODO : Incorporate current speed stat
        // Enqueues
        for (int i = 0; i < units.Count; i++)
            initiative.Enqueue(units[i]);
    }

    public void GetNextInitiative()
    {
        Unit next = initiative.Dequeue();

        // Gives the Unit to the Respected AI
        switch (next.type)
        {
            case "Player":
                CombatState.MakeStatePlayerTurn();
                PlayerTurn.instance.SetSelected(next);
                break;
            case "Enemy":
                CombatState.MakeStateEnemyTurn();
                EnemyAI.instance.SetSelected(next);
                StartCoroutine(EnemyAI.instance.StartTurn());
                break;
            default:
                break;
        }

        next.StartTurn();

        // Check if the queue ran out
        if (initiative.Count == 0)
            QueueUp();
    }


    private void StartBattle()
    {
        GetNextInitiative();
    }
}

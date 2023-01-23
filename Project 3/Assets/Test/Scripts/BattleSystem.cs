using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSystem : MonoBehaviour
{
    #region Singleton
    public static BattleSystem instance;

    void Awake()
    {
        instance = this;
    }
    #endregion

    [Header("Fill Players")]
    public List<Unit> players;
    public Queue<Unit> initiative = new Queue<Unit>();

    void Start()
    {
        QueueUp();
        StartBattle();
    }

    private void StartBattle()
    {
        GetNextInitiative();
    }

    private void QueueUp()
    {
        // Enqueues
        for (int i = 0; i < players.Count; i++)
            initiative.Enqueue(players[i]);
    }

    public void GetNextInitiative()
    {
        Unit next = initiative.Dequeue();
        next.StartTurn();
        MapManager.instance.selected = next;

        if (initiative.Count == 0)
            QueueUp();
    }
}

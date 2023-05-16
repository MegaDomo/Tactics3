using System;
using System.Collections.Generic;
using UnityEngine;

// Utility/*Manager ScriptableObject
[CreateAssetMenu(fileName = "NewSpawnManager", menuName = "Managers/Spawn Manager")]
public class SpawnManager : ScriptableObject
{
    [Header("Scriptable Objects References")]
    public GameMaster gameMaster;
    public BattleSystem battleSystem;

    [Header("Scene Object References")]
    public List<GameObject> playerSpawnPoints;
    public List<GameObject> enemySpawnPoints;

    public Action<List<Unit>> finishedSpawningEvent;

    private List<UnitObj> unitsToSpawn;

    private void Awake()
    {
        
    }

    private void SpawnUnits()
    {
        for (int i = 0; i < playerSpawnPoints.Count; i++)
        {
            SpawnPointBlock spb = playerSpawnPoints[i].GetComponent<SpawnPointBlock>();
            unitsToSpawn.Add(spb.unitObj);
            Destroy(spb.gameObject);
        }

        for (int i = 0; i < enemySpawnPoints.Count; i++)
        {
            SpawnPointBlock spb = enemySpawnPoints[i].GetComponent<SpawnPointBlock>();
            unitsToSpawn.Add(spb.unitObj);
            Destroy(spb.gameObject);
        }
    }
}

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

    [Header("Debugging: Unit Prefabs to Spawn")]
    public GameObject playerPrefab;
    public GameObject enemyPrefab;

    public Action<List<Unit>> finishedSpawningEvent;

    private Grid<Node> map;

}

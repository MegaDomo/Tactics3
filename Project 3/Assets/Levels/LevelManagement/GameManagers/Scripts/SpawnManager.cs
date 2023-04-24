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

    private void OnEnable()
    {
    }

    public void SpawnUnits(Grid<Node> map, List<Node> spawnPoints, List<Unit> unitsToSpawn)
    {
        this.map = map;

        InstantiateUnits(unitsToSpawn);
        PlaceUnits(spawnPoints, unitsToSpawn);

        finishedSpawningEvent?.Invoke(unitsToSpawn);
    }

    private void InstantiateUnits(List<Unit> unitsToSpawn)
    {
        for (int i = 0; i < unitsToSpawn.Count; i++)
        {
            Unit nextUnit = unitsToSpawn[i];

            GameObject clone = Instantiate(nextUnit.prefab);
            UnitMovement movementComponent = clone.GetComponent<UnitMovement>();
            nextUnit.Setup(map, movementComponent);

            if (nextUnit.unitType == Unit.UnitType.Enemy)
                nextUnit.SetAsEnemy();
        }
    }

    private void PlaceUnits(List<Node> spawnPoints, List<Unit> unitsToSpawn)
    {
        for (int i = 0; i < spawnPoints.Count; i++)
            gameMaster.Place(unitsToSpawn[i], spawnPoints[i]);
    }

}

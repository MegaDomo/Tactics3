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

    public Action finishedSpawningEvent;

    private Grid<Node> map;

    private void OnEnable()
    {
        gameMaster.spawnUnitsEvent += SpawnUnits;    
    }

    public void SpawnUnits(Grid<Node> map, List<Unit> players, List<Unit> enemies)
    {
        this.map = map;
        SpawnPlayers(players);
        SpawnEnemies(enemies);
        finishedSpawningEvent.Invoke();
    }

    private void SpawnPlayers(List<Unit> players)
    {
        foreach (Unit player in players)
        {
            GameObject clone = Instantiate(playerPrefab);
            UnitMovement movementComponent = clone.GetComponent<UnitMovement>();
            player.Setup(map, movementComponent);
        }
        PlacePlayers(players);
    }

    private void SpawnEnemies(List<Unit> enemies)
    {
        foreach (Unit enemy in enemies)
        {
            GameObject clone = Instantiate(enemyPrefab);
            UnitMovement movementComponent = clone.GetComponent<UnitMovement>();
            enemy.Setup(map, movementComponent);
        }
        PlaceEnemies(enemies);
    }

    private void PlacePlayers(List<Unit> players)
    {
        Node origin = map.GetGridObject(0, 0);
        List<Node> spawnPoints = Pathfinding.GetDiamond(map, origin, 3);

        for (int i = 0; i < players.Count; i++)
            gameMaster.Place(players[i], spawnPoints[i]);
    }

    private void PlaceEnemies(List<Unit> enemies)
    {
        Node origin = map.GetGridObject((int)map.GetSize() / 2, (int)map.GetSize() / 2);
        List<Node> spawnPoints = Pathfinding.GetDiamond(map, origin, 3);

        for (int i = 0; i < enemies.Count; i++)
            gameMaster.Place(enemies[i], spawnPoints[i]);
    }

    /*
    public void SpawnUnits(List<Unit> playerObjs, int numOfEnemies)
    {
        InstantiatePlayerUnits(playerObjs);
        InstantiateEnemyUnits(numOfEnemies);
    }

    public void PlaceUnits(Grid<Node> map)
    {
        if (players.Count == 0 && enemies.Count == 0)
        {
            Debug.Log("No Units either Lists");
            return;
        }
        
        this.map = map;

        HandlePlayerSpawnPoints(players);
        HandleEnemySpawnPoints(enemies);
    }

    #region Player Spawn Methods
    public void HandlePlayerSpawnPoints(List<Unit> players) // 3 - 6
    {
        Node parent = GetPlayerParentSpawnLocation();
        List<Node> adjNodes = GetSpawnLocations(parent, players.Count);

        SpawnList(adjNodes, players);
    }

    public Node GetPlayerParentSpawnLocation()
    {
        return map.GetGridObject(0, Random.Range(0, map.GetSize()));
        // Hard part
        // Randoms w/ Weights
    }

    public void InstantiatePlayerUnits(List<Unit> playerObjs)
    {
        if (playerObjs.Count == 0)
            return;

        players = CreatePlayerUnits(playerObjs, playerPrefab);
    }
    #endregion

    #region Enemy Spawn Methods
    public void HandleEnemySpawnPoints(List<Unit> enemies)
    {
        Node parent = GetEnemyParentSpawnLocation();
        List<Node> adjNodes = GetSpawnLocations(parent, enemies.Count);

        SpawnList(adjNodes, enemies);
    }

    private Node GetEnemyParentSpawnLocation()
    {
        return map.GetGridObject(Random.Range(0, map.GetSize()), Random.Range(0, map.GetSize()));
    }

    public void InstantiateEnemyUnits(int num)
    {
        if (num == 0)
            return;

        enemies = CreateEnemyUnits(num, enemyPrefab);
    }
    #endregion

    #region Basic Spawn Mathods
    public List<Node> GetSpawnLocations(Node parent, int numOfUnits)
    {
        List<Node> adjNodes = new List<Node>();
        adjNodes.Add(parent);
        Node current;
        int index = -1;
        while (adjNodes.Count < numOfUnits)
        {
            index++;
            current = adjNodes[index];
            foreach (Node node in Pathfinding.GetPassibleNeighbors(gameMaster.GetMap(), current))
            {
                if (adjNodes.Count >= numOfUnits)
                    break;
                    
                if (adjNodes.Contains(node))
                    continue;
                adjNodes.Add(node);
            }
        }

        return adjNodes;
    }

    private void SpawnList(List<Node> spawnPoints, List<Unit> unitsToSpawn)
    {
        for (int i = 0; i < unitsToSpawn.Count; i++)
            Spawn(unitsToSpawn[i], spawnPoints[i]);
    }

    public void Spawn(Unit unit, Node spawnPoint)
    {
        unit.node = spawnPoint;
        spawnPoint.OnUnitEnter(unit);
        gameMaster.Place(unit, spawnPoint);
    }
    #endregion

    #region Utility
    private void SplitPlayersAndEnemies(List<Unit> units)
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

    private List<Unit> CreatePlayerUnits(List<Unit> playerObjs, GameObject prefab)
    {
        List<Unit> tempUnits = new List<Unit>();
        foreach (Unit unit in playerObjs)
        {
            GameObject clone = Instantiate(prefab);
            Unit cloneUnit = clone.GetComponent<Unit>();
            cloneUnit.SetAsPlayer(unit);
            tempUnits.Add(cloneUnit);
        }
        return tempUnits;
    }

    private List<Unit> CreateEnemyUnits(int numToCreate, GameObject prefab)
    {
        List<Unit> tempUnits = new List<Unit>();
        for (int i = 0; i < numToCreate; i++)
        {
            GameObject clone = Instantiate(prefab);
            Unit cloneUnit = clone.GetComponent<Unit>();
            cloneUnit.SetAsEnemy();
            tempUnits.Add(cloneUnit);
        }
        return tempUnits;
    }

    private void DestroyUnits(List<Unit> units)
    {
        foreach (Unit unit in units)
            Destroy(unit.unitMovement.gameObject);
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

    public List<Unit> GetUnits()
    {
        List<Unit> units = new List<Unit>();

        foreach (Unit unit in players)
            units.Add(unit);
        foreach (Unit unit in enemies)
            units.Add(unit);

        return units;
    }

    public void SetUnitsToSpawn(List<Unit> units)
    {
        SplitPlayersAndEnemies(units);
    }
    #endregion
    */
}

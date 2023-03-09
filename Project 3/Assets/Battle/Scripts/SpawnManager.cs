using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [Header("Unity References")]
    public List<Unit> players;
    public List<Unit> enemies;

    [Header("Debugging: Unit Prefabs to Spawn")]
    public GameObject playerPrefab;
    public GameObject enemyPrefab;

    private Grid<Node> map;

    public void SpawnUnits(Grid<Node> map, List<Unit> units)
    {
        if (units.Count == 0)
        {
            Debug.Log("No Units in List");
            return;
        }
        // TODO : Instantiate the Units()
        //InstantiatePlayerUnits(players);
        //InstantiateEnemyUnits(enemies);

        BattleSystem.instance.SetPlayersAndEnemies(players, enemies);

        this.map = map;

        HandlePlayerSpawnPoints(players);
        HandleEnemySpawnPoints(enemies);


        // TODO : Call 2 Scripts/Methods
        // - is the player handler - which is holding onto the loudouts that the player chose for their units
        // it then creates those units maybe in a Create Unit Script
        // - Enemy Spawn Manager/ Creater, maybe pulls from a pool of units and adds them to the list
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
            foreach (Node node in MapManager.instance.pathing.GetNeighbors(current))
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
        MapManager.instance.Place(unit, spawnPoint);
    }
    #endregion

    #region Utility
    public void SetPlayersAndEnemies(List<Unit> players, List<Unit> enemies)
    {
        this.players = players;
        this.enemies = enemies;
    }

    public void InstantiatePlayerUnits(List<Unit> units)
    {
        if (units.Count == 0)
            return;

        List<Unit> tempUnits = new List<Unit>();
        foreach (Unit unit in units)
        {
            GameObject clone = Instantiate(playerPrefab);
            tempUnits.Add(clone.GetComponent<Unit>());
        }

        foreach (Unit unit in units)
            Destroy(unit.gameObject);
        
        players = tempUnits;
        Debug.Log(players[0].name);
    }

    public void InstantiateEnemyUnits(List<Unit> units)
    {
        if (units.Count == 0)
            return;

        List<Unit> tempUnits = new List<Unit>();
        foreach (Unit unit in units)
        {
            GameObject clone = Instantiate(enemyPrefab);
            tempUnits.Add(clone.GetComponent<Unit>());
        }

        foreach (Unit unit in units)
            Destroy(unit.gameObject);

        enemies = tempUnits;
    }
    #endregion
}

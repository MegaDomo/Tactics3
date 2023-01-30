using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    Grid<Node> map;

    List<Unit> players = new List<Unit>();
    List<Unit> enemies = new List<Unit>();

    public void SpawnUnits(Grid<Node> map, List<Unit> units)
    {
        // TODO : Instantiate the Units()

        this.map = map;

        SplitUnits(units, players, enemies);

        HandlePlayerSpawnPoints(players);
        HandleEnemySpawnPoints(enemies);


        // TODO : Call 2 Scripts/Methods
        // - is the player handler - which is holding onto the loudouts that the player chose for their units
        // it then creates those units maybe in a Create Unit Script
        // - Enemy Spawn Manager/ Creater, maybe pulls from a pool of units and adds them to the list
    }

    private void SplitUnits(List<Unit> units, List<Unit> players, List<Unit> enemies)
    {
        foreach (Unit unit in units)
        {
            if (unit.type == "Player")
            {
                players.Add(unit);
            }

            if (unit.type == "Enemy")
            {
                enemies.Add(unit);
            }
        }
    }

    public void HandlePlayerSpawnPoints(List<Unit> players) // 3 - 6
    {
        Node parent = GetPlayerParentSpawnLocation();
        List<Node> adjNodes = GetAdjacentSpawnLocation(parent, players.Count);

        SpawnList(adjNodes, players);
    }

    public void HandleEnemySpawnPoints(List<Unit> enemies)
    {
        Node parent = GetEnemyParentSpawnLocation();
        List<Node> adjNodes = GetAdjacentSpawnLocation(parent, players.Count);

        SpawnList(adjNodes, enemies);
    }

    public Node GetPlayerParentSpawnLocation()
    {
        return map.GetGridObject(0, Random.Range(0, map.GetSize()));
        // Hard part
        // Randoms w/ Weights
    }

    private Node GetEnemyParentSpawnLocation()
    {
        return map.GetGridObject(Random.Range(0, map.GetSize()), Random.Range(0, map.GetSize()));
    }

    public List<Node> GetAdjacentSpawnLocation(Node parent, int numOfUnits)
    {
        List<Node> adjNodes = new List<Node>();

        adjNodes.Add(parent);

        foreach (Node node in MapManager.instance.pathing.GetNeighbors(parent))
        {
            adjNodes.Add(node);
        }

/*
        int index = 0;
        Node current = parent;


        while (adjNodes.Count < numOfUnits)
        {
            if (index > parent.edges.Count)
            {
                current = parent.edges[]
            }
            adjNodes.Add(parent.edges[index]);
            index++;
        }*/
        return adjNodes;
    }

    private void SpawnList(List<Node> spawnPoints, List<Unit> unitsToSpawn)
    {
        int index;
        if (spawnPoints.Count < unitsToSpawn.Count)
            index = spawnPoints.Count;
        else
            index = unitsToSpawn.Count;
        for (int i = 0; i < index; i++)
        {
            Spawn(unitsToSpawn[i], spawnPoints[i]);
        }
    }

    public void Spawn(Unit unit, Node spawnPoint)
    {
        MapManager.instance.Place(unit, spawnPoint);
    }
}

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
        List<Node> adjNodes = GetSpawnLocations(parent, players.Count);
        
        SpawnList(adjNodes, players);
    }

    public void HandleEnemySpawnPoints(List<Unit> enemies)
    {
        Node parent = GetEnemyParentSpawnLocation();
        List<Node> adjNodes = GetSpawnLocations(parent, enemies.Count);
        
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
            //Debug.Log(MapManager.instance.pathing.GetNeighbors(current).Count);
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

    private bool idk()
    {
        return false;
    }

    private void SpawnList(List<Node> spawnPoints, List<Unit> unitsToSpawn)
    {
        for (int i = 0; i < unitsToSpawn.Count; i++)
        {
            Spawn(unitsToSpawn[i], spawnPoints[i]);
        }
            
    }

    public void Spawn(Unit unit, Node spawnPoint)
    {
        MapManager.instance.Place(unit, spawnPoint);
    }
}

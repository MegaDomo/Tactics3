using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    #region Singleton
    public static GameMaster instance;

    private void Awake()
    {
        instance = this;
    }
    #endregion

    [Header("Players")]
    public List<Player> players;

    [Header("Level Editor: Settings of Next Run")]
    public bool makeRandomMap;
    public bool spawnUnits;
    public int numOfEnemies;
    public bool haveCombat;
    public bool editLevelOnRunTime;

    [Header("References")]
    public MapMaker maker;
    public MapManager mapManager;
    public BattleSystem battleSystem;
    public SpawnManager spawner;

    private void Start()
    {

        // Creates the Map
        // TODO : Determine if Static Map or RNG Map
        maker.SetUp(makeRandomMap);

        // Spawn Unit on the Map : When units are spawned add to list
        if (spawnUnits)
        {
            spawner.SpawnUnits(players, numOfEnemies);
            spawner.PlaceUnits(maker.map);
        }

        if (haveCombat)
        {
            List<Unit> players = spawner.GetPlayers();
            List<Unit> enemies = spawner.GetEnemies();
            battleSystem.SetPlayersAndEnemies(players, enemies);
            battleSystem.SetUp();
        }
    }
}

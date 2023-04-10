using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "NewGameMaster", menuName = "Managers/GameMaster")]
public class GameMaster : ScriptableObject
{
    [Header("Players")]
    public List<Player> players;

    [Header("Level Editor: Settings of Next Run")]
    public bool makeRandomMap;
    public bool spawnUnits;
    public bool haveCombat;
    public int numOfEnemies;

    [System.NonSerialized] public Grid<Node> map;
    [System.NonSerialized] public UnityEvent<Transform, bool> makeMapEvent;

    private BattleSystem battleSystem;
    private SpawnManager spawner;

    private void OnEnable()
    {
        makeMapEvent = new UnityEvent<Transform, bool>();
        SceneManager.activeSceneChanged += LoadLevel;
    }

    private void LoadLevel(Scene current, Scene next)
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
            return;

        return;

        Transform startingPoint = GameObject.FindGameObjectWithTag("MapStartingPoint").transform;
        makeMapEvent.Invoke(startingPoint, makeRandomMap);

        if (spawnUnits)
        {
            spawner.SpawnUnits(players, numOfEnemies);
            ///spawner.PlaceUnits(maker.map);
        }

        if (haveCombat)
        {
            List<Unit> players = spawner.GetPlayers();
            List<Unit> enemies = spawner.GetEnemies();
            battleSystem.SetPlayersAndEnemies(players, enemies);
            battleSystem.SetUp();
        }
    }

    #region Getters & Setters
    public Grid<Node> GetMap()
    {
        return map;
    }

    public void SetMap(Grid<Node> map)
    {
        this.map = map;
    }
    #endregion
}

/*
public class GameMaster : MonoBehaviour
{
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
        maker.SetUp(makeRandomMap);

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
}*/

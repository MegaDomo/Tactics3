using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

// Manager ScriptableObject
[CreateAssetMenu(fileName = "NewGameMaster", menuName = "Managers/GameMaster")]
public class GameMaster : ScriptableObject
{
    [Header("Players")]
    public List<Player> players;

    [Header("Scriptable Object References")]
    public MapMaker mapMaker;
    public BattleSystem battleSystem;
    public SpawnManager spawner;

    [Header("Level Editor: Settings of Next Run")]
    public bool makeRandomMap;
    public bool spawnUnits;
    public bool haveCombat;
    public int numOfEnemies;

    [HideInInspector] public Grid<Node> map;
    [HideInInspector] public UnityEvent<Transform, bool> makeMapEvent;

    private void OnEnable()
    {
        mapMaker.mapMadeEvent += MapEventSubscriber;

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

    #region Events & Subscribers
    private void MapEventSubscriber(Grid<Node> map)
    {
        this.map = map;
    }
    #endregion

    #region Utility Methods
    public void Place(Unit unit, Node destination)
    {
        Vector3 newPosition = destination.GetStandingPoint() + unit.offset;
        unit.gameObject.transform.position = newPosition;

        // This Sets Node and Unit data
        unit.node = destination;
        destination.unit = unit;
    }
    #endregion

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

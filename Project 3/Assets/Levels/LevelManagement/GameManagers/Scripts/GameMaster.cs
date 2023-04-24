using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Manager ScriptableObject
[CreateAssetMenu(fileName = "NewGameMaster", menuName = "Managers/GameMaster")]
public class GameMaster : ScriptableObject
{
    [Header("Players")]
    public List<Unit> players;
    public List<Unit> enemies;

    [Header("Scriptable Object References")]
    public LevelManager levelManager;
    public MapMaker mapMaker;
    public BattleSystem battleSystem;

    [Header("Level Editor: Settings of Next Run")]
    public bool makeRandomMap;
    public bool spawnUnits;
    public bool haveCombat;
    public int numOfEnemies;

    public Action<Transform, bool> makeMapEvent;
    public Action<Grid<Node>, List<Unit>, List<Unit>, Dictionary<Node, Unit>> spawnUnitsEvent;
    public Action<List<Unit>, List<Unit>> startCombatEvent;
    public Action<Dialogue> startDialogueEvent;

    private Grid<Node> map;
    private Dictionary<Node, Unit> spawnPoints;
    private string levelToLoad;

    private void OnEnable()
    {
        mapMaker.mapMadeEvent += MapEventSubscriber;
        battleSystem.victoryEvent += Victory;
        SceneManager.activeSceneChanged += LoadLevel;
    }

    public void LoadLevel(Scene current, Scene next)
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
            return;

        Transform startingPoint = GameObject.FindGameObjectWithTag("MapStartingPoint").transform;
        makeMapEvent.Invoke(startingPoint, makeRandomMap);

        if (spawnUnits)
            spawnUnitsEvent.Invoke(map, players, enemies, spawnPoints);

        Level level = levelManager.GetLevel(levelToLoad);
        if (level.isTherePreDialogue)
            startDialogueEvent.Invoke(level.preCombatDialogue);
    }

    #region Events & Subscribers
    private void MapEventSubscriber(Grid<Node> map, Dictionary<Node, Unit> spawnPoints)
    {
        this.spawnPoints = spawnPoints;
        this.map = map;
    }
    #endregion

    #region Utility Methods
    public void Place(Unit unit, Node destination)
    {
        Vector3 newPosition = destination.GetStandingPoint() + unit.unitMovement.offset;
        unit.SetPosition(newPosition);

        // This Sets Node and Unit data
        unit.node = destination;
        destination.unit = unit;
    }

    public void StartCobmat()
    {
        startCombatEvent.Invoke(players, enemies);
    }

    public void Victory()
    {
        Level level = levelManager.GetLevel(levelToLoad);
        if (level.isTherePostDialogue)
            startDialogueEvent.Invoke(level.postCombatDialogue);
            
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

    public string GetLevelToLoad()
    {
        return levelToLoad;
    }

    public void SetLevelToLoad(string levelToLoad)
    {
        this.levelToLoad = levelToLoad;
    }
    #endregion
}

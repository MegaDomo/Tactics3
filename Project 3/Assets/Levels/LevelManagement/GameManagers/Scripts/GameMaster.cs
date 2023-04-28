using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Manager ScriptableObject
[CreateAssetMenu(fileName = "NewGameMaster", menuName = "Managers/GameMaster")]
public class GameMaster : ScriptableObject
{
    [Header("Scriptable Object References")]
    public LevelManager levelManager;
    public MapMaker mapMaker;
    public BattleSystem battleSystem;

    [Header("Level Editor: Settings of Next Run")]
    public bool skipDialogue;
    public bool makeRandomMap;
    public bool spawnUnits;
    public bool haveCombat;

    public Action<Transform, bool> makeMapEvent;
    public Action<Grid<Node>, List<Node>, List<Unit>> spawnUnitsEvent;
    public Action<List<Unit>> startCombatEvent;
    public Action<Dialogue> startDialogueEvent;

    private Grid<Node> map;
    private List<Unit> units = new List<Unit>();
    private string levelToLoad;

    private void OnEnable()
    {
        mapMaker.mapMadeEvent += MapEventSubscriber;
        battleSystem.victoryEvent += Victory;
        SceneManager.activeSceneChanged += LoadLevel;
    }

    private void OnDisable()
    {
        mapMaker.mapMadeEvent -= MapEventSubscriber;
        battleSystem.victoryEvent -= Victory;
        SceneManager.activeSceneChanged -= LoadLevel;
    }

    public void LoadLevel(Scene current, Scene next)
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
            return;

        Transform startingPoint = GameObject.FindGameObjectWithTag("MapStartingPoint").transform;
        makeMapEvent.Invoke(startingPoint, makeRandomMap);

        Level level = levelManager.GetLevel(levelToLoad);
        if (level.isTherePreDialogue && !skipDialogue)
            startDialogueEvent?.Invoke(level.preCombatDialogue);
        else
            StartCobmat();
    }

    #region Events & Subscribers
    private void MapEventSubscriber(Grid<Node> map, List<Unit> units)
    {
        this.units = units;
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
        startCombatEvent?.Invoke(units);
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

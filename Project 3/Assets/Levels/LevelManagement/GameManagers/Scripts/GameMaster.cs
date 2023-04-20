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

    private Grid<Node> map;

    public Action<Transform, bool> makeMapEvent;
    public Action<Grid<Node>, List<Unit>, List<Unit>> spawnUnitsEvent;

    private string levelToLoad;

    private void OnEnable()
    {
        mapMaker.mapMadeEvent += MapEventSubscriber;
        SceneManager.activeSceneChanged += LoadLevel;
    }

    public void LoadLevel(Scene current, Scene next)
    {
        levelManager.LoadLevel(levelToLoad);
        //if (SceneManager.GetActiveScene().buildIndex == 0)
        //    return;
        
        Transform startingPoint = GameObject.FindGameObjectWithTag("MapStartingPoint").transform;
        makeMapEvent.Invoke(startingPoint, makeRandomMap);

        if (spawnUnits)
            spawnUnitsEvent.Invoke(map, players, enemies);
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
        Vector3 newPosition = destination.GetStandingPoint() + unit.unitMovement.offset;
        unit.SetPosition(newPosition);

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

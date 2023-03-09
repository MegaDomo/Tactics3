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

    [Header("Level Editor: Settings of Next Run")]
    public bool makeRandomMap;
    public bool spawnUnits;
    public bool haveCombat;
    public bool editLevelOnRunTime;

    [Header("References")]
    public MapMaker maker;
    public MapManager mapManager;
    public BattleSystem battleSystem;
    public SpawnManager spawner;
    public LevelEditorSystem editor;

    private void Start()
    {

        // Creates the Map
        // TODO : Determine if Static Map or RNG Map
        maker.SetUp(makeRandomMap);
        mapManager.SetUp();

        // Spawn Unit on the Map : When units are spawned add to list
        if (spawnUnits)
            spawner.SpawnUnits(maker.map, battleSystem.units);

        if (haveCombat)
            battleSystem.SetUp();

        if (editLevelOnRunTime)
            editor.SetUp(!makeRandomMap);
    }
}

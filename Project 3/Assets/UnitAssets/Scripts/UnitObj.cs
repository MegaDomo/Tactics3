using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewUnit", menuName = "Units/Unit")]
public class UnitObj : ScriptableObject
{
    [Header("Scriptable Object References")]
    public GameMaster gameMaster;
    public BattleSystem battleSystem;
    public PlayerTurn playerTurn;
    public EnemyAI enemyAI;

    [Header("Character Attributes")]
    public string characterName;
    public Sprite portrait;
    public Sprite fullBody;
    public Expression[] expressionSheet;

    [Header("Attributes")]
    public Unit.UnitType unitType;
    public GameObject prefab;
    public AnimatorOverrideController overrideController;
    public Weapon equippedWeapon;
    public List<Weapon> weapons;
    public List<Ability> abilities;
    public List<Item> items;
    public UnitStats stats;

    [Header("If Enemy Attributes")]
    public Unit.BehaviorType behaviorType;
    [HideInInspector] public Behavior behavior;

    [Header("Debugging")]
    public Unit tauntedTarget;


    private Grid<Node> map;

    #region Getters & Setters
    public Grid<Node> GetMap()
    {
        return map;
    }
    #endregion
}

using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewUnit", menuName = "Units/Unit")]
public class UnitObj : ScriptableObject
{
    [Header("Scriptable Object References")]
    public GameMaster gameMaster;
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
    public UnitStats stats;

    [Header("If Enemy Attributes")]
    public Unit.BehaviorType behaviorType;
    [HideInInspector] public Behavior behavior;

    [Header("Debugging")]
    public Unit tauntedTarget;

    [HideInInspector] public Weapon equippedWeapon;
    [HideInInspector] public List<Weapon> weapons;
    [HideInInspector] public List<Ability> abilities;
    [HideInInspector] public List<Item> items;

    private Grid<Node> map;

    #region Getters & Setters
    public Grid<Node> GetMap()
    {
        return map;
    }
    #endregion
}

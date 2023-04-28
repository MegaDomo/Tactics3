using System;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public enum UnitType { Player, Enemy, Neutral, Ally, Civilian }
    public enum BehaviorType { Attacker, Killer }

    [Header("Debugging")]
    public Unit tauntedTarget;

    public UnitStats stats; // Not Hidden for Debbuging

    [HideInInspector] public Node node;
    [HideInInspector] public UnitMovement unitMovement;
    [HideInInspector] public Unit target;

    [HideInInspector] public Weapon equippedWeapon;
    [HideInInspector] public List<Weapon> weapons;
    [HideInInspector] public List<Ability> abilities;
    [HideInInspector] public List<Item> items;

    [HideInInspector] public Action startTurnEvent;
    [HideInInspector] public Action endTurnEvent;
    [HideInInspector] public Action<int, int> healthChangeEvent;
    [HideInInspector] public Action movementEvent;

    [HideInInspector] public UnitObj unitObj;
    [HideInInspector] public UnitType unitType;
    [HideInInspector] public BehaviorType behaviorType;
    [HideInInspector] public Behavior behavior;

    private GameMaster gameMaster;
    private BattleSystem battleSystem;
    private PlayerTurn playerTurn;
    private EnemyAI enemyAI;

    private Grid<Node> map;

    private AnimatorOverrideController overrideController;

    public void Setup(Grid<Node> map, UnitObj unitObj)
    {
        this.unitObj = unitObj;

        gameMaster = unitObj.gameMaster;
        battleSystem = unitObj.battleSystem;
        playerTurn = unitObj.playerTurn;
        enemyAI = unitObj.enemyAI;

        unitType = unitObj.unitType;
        overrideController = unitObj.overrideController;

        behaviorType = unitObj.behaviorType;
        behavior = unitObj.behavior;

        stats = unitObj.stats;

        weapons = unitObj.weapons;
        abilities = unitObj.abilities;
        items = unitObj.items;

        this.map = map;

        unitMovement = GetComponent<UnitMovement>();
        unitMovement.Setup(this);
    }

    #region Turn Methods
    public void StartTurn()
    {
        // Resets how far he moved
        stats.moved = 0;

        // TODO : Handle Status Effects / Tile Effects

    }

    public void EndTurn()
    {

    }
    #endregion

    #region Move Methods
    public void Move(Node destination)
    {
        unitMovement.Move(destination);
    }
    #endregion

    #region Damage Methods
    public void TakeDamage(Unit attacker, Weapon weapon)
    {
        int damage = 0;
        if (weapon.damageType == Weapon.DamageType.Physical)
        {
            damage = attacker.stats.attack + weapon.damage;
            DecreaseHealth(damage - stats.defense < 0 ? 0 : damage - stats.defense);
        }
        if (weapon.damageType == Weapon.DamageType.Magical)
        {
            damage = attacker.stats.spAttack + weapon.damage;
            DecreaseHealth(damage - stats.spDefense < 0 ? 0 : damage - stats.spDefense);
        }
    }

    public void DecreaseHealth(int amount)
    {
        stats.curHealth -= amount;
        healthChangeEvent.Invoke(stats.curHealth, stats.maxHealth);
    }

    public void SetHealth(int amount)
    {
        stats.maxHealth = amount;
        stats.curHealth = stats.maxHealth;
        healthChangeEvent.Invoke(stats.curHealth, stats.maxHealth);
    }

    public int ForecastTakePhysicalDamage(int damage)
    {
        return damage - stats.defense < 0 ? 0 : damage - stats.defense;
    }

    public int ForecastTakeMagicalDamage(int damage)
    {
        return damage - stats.spDefense < 0 ? 0 : damage - stats.spDefense;
    }
    #endregion

    #region Attacking Methods
    
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

    public Vector3 GetPosition()
    {
        return unitMovement.transform.position;
    }

    public void SetPosition(Vector3 value)
    {
        unitMovement.transform.position = value;
    }

    public void SetWeapons(List<Weapon> weapons)
    {
        if (weapons.Count == 0)
            return;

        this.weapons = weapons;
        SetWeapon(weapons[0]);
    }

    public void SetWeapon(Weapon _weapon)
    {
        if (_weapon == null)
            return;
        unitMovement.SetWeapon(_weapon);
        equippedWeapon = _weapon;
    }

    public void SetAbilities(List<Ability> abilities)
    {
        
    }

    public void SetInventory(List<Item> items)
    {
        if (items.Count == 0)
            return;

        this.items = items;
    }

    public void SetAsPlayer(Unit player)
    {
        unitType = UnitType.Player;
    }

    public void SetAsEnemy()
    {
        UnitBehavior unitBehavior = new UnitBehavior(this);
        behavior = unitBehavior.GetBehavior();
        behavior.self = this;
    }

    public int MovementLeft()
    {
        return stats.movement;
    }

    public void SetIsMoving(bool value)
    {
        unitMovement.SetIsMoving(value);
    }

    public bool IsMoving()
    {
        return unitMovement.IsMoving();
    }

    public bool IsAttacking()
    {
        return unitMovement.IsAttacking();
    }

    public void SetIsAttacking(bool value)
    {
        unitMovement.SetIsAttacking(value);
    }

    public BattleSystem GetBattleSystem()
    {
        return battleSystem;
    }
    #endregion
}

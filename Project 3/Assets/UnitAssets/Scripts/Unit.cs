using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewUnit", menuName = "Units/Unit")]
public class Unit : ScriptableObject
{
    public enum UnitType { Player, Enemy, Neutral, Ally, Civilian }
    public enum BehaviorType { Attacker, Killer }

    [Header("Scriptable Object References")]
    public GameMaster gameMaster;
    public PlayerTurn playerTurn;
    public EnemyAI enemyAI;

    [Header("Attributes")]
    public UnitType unitType;
    public Character character;
    public GameObject prefab;
    public AnimatorOverrideController overrideController;

    [Header("If Enemy Attributes")]
    public BehaviorType behaviorType;
    [HideInInspector] public Behavior behavior;

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

    private Grid<Node> map;

    public void Setup(Grid<Node> map, UnitMovement unitMovement)
    {
        this.map = map;
        this.unitMovement = unitMovement;
        this.unitMovement.Setup(this);
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
        UnitBehavior unitBehavior = new UnitBehavior(this, gameMaster.GetMap(), behaviorType);
        behavior = unitBehavior.GetBehavior();
        behavior.self = this;
    }

    public int MovementLeft()
    {
        return unitMovement.MovementLeft();
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
    #endregion
}

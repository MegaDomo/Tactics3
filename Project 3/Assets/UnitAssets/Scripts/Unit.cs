using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public enum UnitType { Player, Enemy, Neutral, Ally, Civilian }

    [Header("Unity References")]
    public Transform ground;
    public Transform weaponPoint;
    public Transform vfx;
    public Animator anim;

    [Header("Attributes")]
    public UnitType unitType;

    [HideInInspector] public Node node;
    
    public UnitStats stats; // Not Hidden for Debbuging
    
    [HideInInspector] public Weapon equippedWeapon;
    [HideInInspector] public List<Weapon> weapons;
    [HideInInspector] public List<Ability> abilities;
    [HideInInspector] public List<Item> items;

    [HideInInspector] public EnemyObject enemyObj;
    [HideInInspector] public Behavior behavior;

    [HideInInspector] public UnitAnimation unitAnim;

    [HideInInspector] public Vector3 offset;

    private GameObject weaponPrefab;
    private MapManager mapManager;

    private void Update()
    {
        unitAnim.MoveUnit();
    }

    public void SetupUnit()
    {
        offset = transform.position - ground.position;
        mapManager = MapManager.instance;
        unitAnim = GetComponent<UnitAnimation>();
        if (unitType == UnitType.Player)
            SetAsPlayer();
        if (unitType == UnitType.Enemy)
            SetAsEnemy();
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

    #region Damage Methods
    public void TakeDamage(Unit attacker, Weapon weapon)
    {
        int damage = 0;
        if (weapon.damageType == Weapon.DamageType.Physical)
        {
            damage = attacker.stats.attack + weapon.damage;
            stats.curHealth -= damage - stats.defense < 0 ? 0 : damage - stats.defense;
        }
        if (weapon.damageType == Weapon.DamageType.Magical)
        {
            damage = attacker.stats.spAttack + weapon.damage;
            stats.curHealth -= damage - stats.spDefense < 0 ? 0 : damage - stats.spDefense;
        }
    }
    public void TakePhysicalDamage(int damage)
    {
        // TODO : If health < 0 Death()
        stats.curHealth -= damage - stats.defense < 0 ? 0 : damage - stats.defense;
    }

    public void TakeMagicalDamage(int damage)
    {
        stats.curHealth -= damage - stats.spDefense < 0 ? 0 : damage - stats.spDefense;
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

    #region Movement
    public void Move(Node end)
    {
        if (!mapManager.CanMove(this, node, end))
            return;

        List<Node> path = mapManager.GetPath(node, end);
        int pathCost = mapManager.GetPathCost(path);

        unitAnim.Move(path);

        stats.moved += pathCost;
        node.OnUnitExit();
        end.OnUnitEnter(this);
    }

    public int MovementLeft()
    {
        return stats.movement - stats.moved;
    }
    #endregion

    #region Attacking Methods
    #endregion

    #region Setters
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

        if (weaponPrefab != null)
            Destroy(weaponPrefab);
        weaponPrefab = Instantiate(_weapon.prefab, weaponPoint.position, weaponPoint.rotation);
        weaponPrefab.transform.SetParent(weaponPoint.transform);

        equippedWeapon = _weapon;
    }

    public void SetAbilities(List<Ability> abilities)
    {
        if (abilities.Count == 0)
            return;

        this.abilities = abilities;
    }

    public void SetInventory(List<Item> items)
    {
        if (items.Count == 0)
            return;

        this.items = items;
    }

    public void SetAsPlayer()
    {
        unitType = UnitType.Player;
        stats = GetComponent<Player>().stats;
    }

    public void SetAsEnemy()
    {
        EnemyObject enemyObject = GetComponent<Enemy>().enemyObject;
        if (enemyObject == null)
        {
            Debug.Log("No Enemy Data to Set for: " + name);
            return;
        }
            
        gameObject.tag = "Enemy";
        unitType = UnitType.Enemy;
        enemyObj = enemyObject;
        UnitBehavior unitBehavior = new UnitBehavior(this);
        behavior = unitBehavior.GetBehavior();
    }

    public void SetIsMoving(bool value)
    {
        unitAnim.SetIsMoving(value);
    }

    public bool IsMoving()
    {
        return unitAnim.IsMoving();
    }

    public void SetIsAttacking(bool value)
    {
        unitAnim.SetIsAttacking(value);
    }

    public bool IsAttacking()
    {
        return unitAnim.IsAttacking();
    }
    #endregion
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public enum UnitType { Player, Enemy, Neutral, Ally, Civilian }

    [Header("Unity References")]
    public Transform ground;
    public Transform weaponPoint;
    public Transform EffectPoint;
    public Transform vfx;
    public Animator anim;

    [Header("Attributes")]
    public UnitType unitType;

    [Header("Debugging")]
    public Unit tauntedTarget;

    [HideInInspector] public Node node;
    
    public UnitStats stats; // Not Hidden for Debbuging
    
    [HideInInspector] public Weapon equippedWeapon;
    [HideInInspector] public List<Weapon> weapons;
    [HideInInspector] public List<Item> items;

    [HideInInspector] public Enemy enemy;
    [HideInInspector] public Player player;
    [HideInInspector] public UnitAbilities unitAbilities;

    [HideInInspector] public UnitAnimation unitAnim;

    [HideInInspector] public Vector3 offset;

    private GameObject weaponPrefab;
    private MapManager mapManager;

    private void Update()
    {
        unitAnim.MoveUnit();

        if (enemy == null)
            return;
        enemy.behavior.SetTauntedPlayer(tauntedTarget);
    }

    public void SetupUnit()
    {
        offset = transform.position - ground.position;
        mapManager = MapManager.instance;
        unitAnim = GetComponent<UnitAnimation>();
        unitAbilities = GetComponent<UnitAbilities>();
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
    public void Move(Node destination)
    {
        List<Node> path = mapManager.GetPath(node, destination);
        int pathCost = mapManager.GetPathCost(path);

        unitAnim.Move(path);

        stats.moved += pathCost;
        node.OnUnitExit();
        destination.OnUnitEnter(this);
    }

    public int MovementLeft()
    {
        return stats.movement - stats.moved;
    }
    #endregion

    #region Attacking Methods
    public void Attack()
    {
        unitAnim.SetIsAttacking(true);
        unitAnim.Attack();
    }
    #endregion

    #region Getters & Setters
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
        
    }

    public void SetInventory(List<Item> items)
    {
        if (items.Count == 0)
            return;

        this.items = items;
    }

    public void SetAsPlayer()
    {
        player = GetComponent<Player>();
        if (player == null)
        {
            Debug.Log("No Player Data to Set for: " + name);
            return;
        }
        stats = player.stats;
    }

    public void SetAsEnemy()
    {
        enemy = GetComponent<Enemy>();
        if (enemy == null)
        {
            Debug.Log("No Enemy Data to Set for: " + name);
            return;
        }
        enemy.SetupEnemy(this);
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

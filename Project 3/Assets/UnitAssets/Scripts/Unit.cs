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
    public UnitStats stats;
    public Weapon equippedWeapon;
    public List<Weapon> weapons;
    public List<Ability> abilities;
    public EnemyObject enemyObj;

    [HideInInspector] public Vector3 offset;
    [HideInInspector] public Node node;
    [HideInInspector] public UnitAnimation unitAnim;
    [HideInInspector] public UnitBehavior unitBehavior;

    private GameObject weaponPrefab;

    void Awake()
    {
        offset = transform.position - ground.position;
        unitAnim = GetComponent<UnitAnimation>();
        unitBehavior = GetComponent<UnitBehavior>();
    }

    private void Start()
    {
        //unitAnim = new UnitAnimation(vfx, anim);
        SetWeapon(weapons[0]);
    }

    private void Update()
    {
        unitAnim.MoveUnit();
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
    public void TakePhysicalDamage(int damage)
    {
        // TODO : If health < 0 Death()
        stats.curHealth -= damage - stats.defense < 0 ? 0 : damage - stats.defense;
    }

    public void TakeMagicalDamage(int damage)
    {
        stats.curHealth -= damage - stats.spDefense < 0 ? 0 : damage - stats.spDefense;
    }

    public int ForecastPhysicalDamage(int damage)
    {
        return damage - stats.defense < 0 ? 0 : damage - stats.defense;
    }

    public int ForecastMagicalDamage(int damage)
    {
        return damage - stats.spDefense < 0 ? 0 : damage - stats.spDefense;
    }
    #endregion

    #region Movement
    public void Move(int pathCost, List<Node> path)
    {
        stats.moved += pathCost;
        unitAnim.Move(path);
    }

    public int MovementLeft()
    {
        return stats.movement - stats.moved;
    }
    #endregion

    #region Attacking Methods
    public void WeaponStrike()
    {
        unitAnim.WeaponStrike(equippedWeapon);
    }
    #endregion

    #region Setters
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
    #endregion
}

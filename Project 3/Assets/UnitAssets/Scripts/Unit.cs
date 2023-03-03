using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public enum UnitType { Player, Enemy, Neutral, Ally, Civilian }
    

    [Header("Unity References")]
    public Transform ground;
    public Transform weaponPoint;

    [Header("Attributes")]
    public UnitType unitType;
    public UnitStats stats;
    public Weapon equippedWeapon;
    public List<Weapon> weapons;
    public List<Ability> abilities;

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
        SetWeapon(weapons[0]);
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

    public int MovementLeft()
    {
        return stats.movement - stats.moved;
    }

    #region Setters


    public void SetWeapon(Weapon _weapon)
    {
        if (_weapon == null)
            return;
        weaponPrefab = Instantiate(_weapon.prefab, weaponPoint.position, weaponPoint.rotation);
        weaponPrefab.transform.SetParent(weaponPoint.transform);
        equippedWeapon = _weapon;
    }

    #endregion
}

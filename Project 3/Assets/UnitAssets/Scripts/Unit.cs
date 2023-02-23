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
    public UnitType type;
    public UnitStats stats;
    public Weapon equippedWeapon;
    public List<Weapon> weapons;
    public List<Ability> abilities;

    [Header("Debugging")]
    public Behavior behavior;

    [HideInInspector] public Node node;
    [HideInInspector] public Vector3 offset;

    private GameObject weaponPrefab;

    void Awake()
    {
        offset = transform.position - ground.position;
    }

    private void Start()
    {
        SetSelf();
        //SetWeapon(weapons[0]);
    }

    public void StartTurn()
    {
        // Resets how far he moved
        stats.moved = 0;




        // TODO : Handle Status Effects / Tile Effects

    }

    public void EndTurn()
    {

    }

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




    public void SetWeapon(Weapon _weapon)
    {
        weaponPrefab = Instantiate(_weapon.prefab, weaponPoint.position, Quaternion.identity);
        weaponPrefab.transform.SetParent(gameObject.transform);
    }

    private void SetSelf()
    {
        if (type == Unit.UnitType.Enemy)
        {
            gameObject.tag = "Enemy";
            behavior.self = this;
        }

        if (type == Unit.UnitType.Player)
        {
            gameObject.tag = "Player";
        }

    }

    public int MovementLeft()
    {
        return stats.movement - stats.moved;
    }

}

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
        SetWeapon(weapons[0]);
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

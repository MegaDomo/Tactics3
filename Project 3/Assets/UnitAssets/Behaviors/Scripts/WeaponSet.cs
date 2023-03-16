using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSet
{
    private Unit self;
    private Unit target;
    private Weapon weapon;
    private Node destination;
    int bestDamage;

    public WeaponSet()
    {

    }

    public WeaponSet(Unit self, Unit target, Weapon weapon)
    {
        this.self = self;
        this.target = target;
        this.weapon = weapon;
        CalculateBestDamage();
        // Calculate Aggro
    }

    public WeaponSet(Unit self, Unit target, Weapon weapon, Node destination)
    {
        this.self = self;
        this.target = target;
        this.weapon = weapon;
        this.destination = destination;
        CalculateBestDamage();
        // Calculate Aggro
    }

    public WeaponSet(Weapon weapon, Unit target, Unit self, int damage)
    {
        this.weapon = weapon;
        this.target = target;
        this.self = self;
        this.bestDamage = damage;
    }

    #region Utility
    private void CalculateBestDamage()
    {
        if (weapon == null || target == null || self == null)
            return;

        int damage = -1;
        if (weapon.damageType == Weapon.DamageType.Physical)
            damage = target.ForecastTakePhysicalDamage(weapon.damage + self.stats.attack);
        if (weapon.damageType == Weapon.DamageType.Magical)
            damage = target.ForecastTakeMagicalDamage(weapon.damage + self.stats.spAttack);

        bestDamage = damage;
    }
    #endregion

    #region Getters & Setters
    public Unit GetSelf()
    {
        return self;
    }

    public void SetSelf(Unit self)
    {
        this.self = self;
    }

    public Unit GetTarget()
    {
        return target;
    }

    public void SetTarget(Unit target)
    {
        this.target = target;
    }

    public Weapon GetWeapon()
    {
        return weapon;
    }

    public void SetWeapon(Weapon weapon)
    {
        this.weapon = weapon;
    }

    public Node GetDestination()
    {
        return destination;
    }

    public void SetDestination(Node destination)
    {
        this.destination = destination;
    }

    public int GetBestDamage()
    {
        return bestDamage;
    }

    public void SetBestDamage(int damage)
    {
        bestDamage = damage;
    }
    #endregion
}

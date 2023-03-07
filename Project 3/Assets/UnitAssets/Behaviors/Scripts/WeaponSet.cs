using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSet
{
    private Unit self;
    private Unit target;
    private Weapon weapon;
    int bestDamage;

    public WeaponSet()
    {

    }

    public WeaponSet(Weapon weapon, Unit target, Unit self)
    {
        this.weapon = weapon;
        this.target = target;
        this.self = self;
        CalculateBestDamage();
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

    public Unit GetTarget()
    {
        return target;
    }

    public Weapon GetWeapon()
    {
        return weapon;
    }

    public int GetBestDamage()
    {
        return bestDamage;
    }

    public void SetSelf(Unit self)
    {
        this.self = self;
    }

    public void SetTarget(Unit target)
    {
        this.target = target;
    }

    public void SetWeapon(Weapon weapon)
    {
        this.weapon = weapon;
    }

    public void SetBestDamage(int damage)
    {
        bestDamage = damage;
    }
    #endregion
}

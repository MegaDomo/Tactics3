using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewWeapon", menuName = "Gear/Weapon")]
public class Weapon : ScriptableObject
{
    public enum DamageType { Physical, Magical }

    public enum WeaponType { Sword1h, Axe1h, Mace1h, Dagger, Bow, Staff, Scepter, Sword2h, Axe2h, Mace2h }

    [Header("Attributes")]
    public int damage;
    public int spDamage;
    public int range;
    public int minRange;
    public int charges;
    public DamageType damageType;
    public List<Ability> abilities;
    public string weaponType;

    [Header("References")]
    public AnimationClip animation;
    public GameObject prefab;
}

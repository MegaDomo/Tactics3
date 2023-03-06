using UnityEngine;

[CreateAssetMenu(fileName = "NewWeapon", menuName = "Gear/Weapon")]
public class Weapon : ScriptableObject
{
    public enum WeaponType { Physical, Magical }

    public int damage;
    public int range;
    public int minRange;
    public int charges;
    public WeaponType weaponType;
    public AnimationClip animation;
    public GameObject prefab;
}

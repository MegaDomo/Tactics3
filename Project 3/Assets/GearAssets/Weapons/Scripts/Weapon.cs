using UnityEngine;

[CreateAssetMenu(fileName = "NewWeapon", menuName = "Gear/Weapon")]
public class Weapon : ScriptableObject
{
    public int damage;
    public int range;
    public int charges;

    public GameObject prefab;
}

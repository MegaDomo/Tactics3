using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitWeapons : MonoBehaviour
{
    public List<Weapon> weapons;

    private void Start()
    {
        Unit unit = GetComponent<Unit>();
        unit.weapons = weapons;
        unit.SetWeapon(weapons[0]);
    }
}

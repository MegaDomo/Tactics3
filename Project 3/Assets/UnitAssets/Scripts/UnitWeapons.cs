using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitWeapons : MonoBehaviour
{
    public List<Weapon> weapons = new List<Weapon>();

    private void Start()
    {
        if (weapons.Count == 0)
            return;

        GetComponent<Unit>().SetWeapons(weapons);        
    }
}

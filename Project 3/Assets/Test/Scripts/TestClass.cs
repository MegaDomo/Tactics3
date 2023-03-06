using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestClass : MonoBehaviour
{
    public Unit player;

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            player.WeaponStrike();
        if (Input.GetKeyDown(KeyCode.Alpha1))
            player.SetWeapon(player.weapons[0]);
        if (Input.GetKeyDown(KeyCode.Alpha2))
            player.SetWeapon(player.weapons[1]);
    }
}

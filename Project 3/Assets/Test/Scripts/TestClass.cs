using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestClass : MonoBehaviour
{
    public Unit player;
    public AnimatorOverrideController overrideC;

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            player.WeaponStrike();
        //Sword
        if (Input.GetKeyDown(KeyCode.Alpha1))
            player.SetWeapon(player.weapons[0]);
        // Axe
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            player.unitAnim.anim.runtimeAnimatorController = overrideC;
            player.SetWeapon(player.weapons[1]);
        }
            
    }
}

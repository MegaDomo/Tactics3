using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestClass : MonoBehaviour
{
    public Unit player;
    public Unit player2;
    public AnimatorOverrideController overrideC;
    public AnimatorOverrideController overrideC2;

    void Start()
    {
        player.unitAnim.anim.runtimeAnimatorController = overrideC;
        player2.unitAnim.anim.runtimeAnimatorController = overrideC2;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            player.WeaponStrike();
            player2.WeaponStrike();
        }
            
        //Sword
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            player.SetWeapon(player.weapons[0]);
            player2.SetWeapon(player.weapons[0]);
        }
            
        // Axe
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            player.SetWeapon(player.weapons[1]);
            player2.SetWeapon(player.weapons[1 ]);
        }
            
    }
}

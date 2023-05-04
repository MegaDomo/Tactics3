using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSwordStrike", menuName = "Abilities/SwordStrike")]
public class SwordStrike : Ability
{
    public override void DirectTargeting(Unit player)
    {
        Debug.Log("DirectTargeting from Sword Strike");
    }
}

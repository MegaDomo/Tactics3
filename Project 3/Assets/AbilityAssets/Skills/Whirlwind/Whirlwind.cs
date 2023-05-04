using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewWhirlwind", menuName = "Abilities/Whirlwind")]
public class Whirlwind : Ability
{

    public override void AreaTargeting(List<Node> nodes)
    {
        Debug.Log("WhirlWind Area Targeting");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewWhirlwind", menuName = "Abilities/Class Skills/Whirlwind")]
public class Whirlwind : Ability
{
    public override void AreaTargeting(Unit player)
    {
        Debug.Log("WhirlWind Area Targeting");
    }

    public override List<Node> GetAreaTargeting(Node node)
    {
        Grid<Node> map = GameMaster.map;
        return Pathfinding.GetHollowDiamond(map, node, aoeMaxRange, aoeMinRange);
    }
}

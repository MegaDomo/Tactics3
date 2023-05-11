using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewFireBall", menuName = "Abilities/FireBall")]
public class FireBall : Ability
{
    public override void AreaTargeting(Unit player)
    {
        Debug.Log("FireBall Area Targeting");
    }

    public override List<Node> GetAbilityRange(Node node)
    {
        return Pathfinding.GetHollowDiamond(GameMaster.map, node, maxRange, minRange);
    }

    public override List<Node> GetAreaTargeting(Node node)
    {
        Grid<Node> map = GameMaster.map;
        return Pathfinding.GetHollowDiamond(map, node, aoeMaxRange, aoeMinRange);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBowFireSkill", menuName = "Abilities/Weapon Skills/BowFire")]
public class BowFireSkill : Ability
{
    public override List<Node> GetAbilityRange(Node node)
    {
        return Pathfinding.GetHollowDiamond(GameMaster.map, node, maxRange, minRange);
    }

    public override List<Node> GetAreaTargeting(Node node)
    {
        return Pathfinding.GetHollowDiamond(GameMaster.map, node, aoeMaxRange, aoeMinRange);
    }
}

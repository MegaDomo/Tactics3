using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewConeOfCold", menuName = "Abilities/ConeOfCold")]
public class ConeOfCold : Ability
{
    public override void AreaTargeting(Unit player)
    {
        Debug.Log("ConeOfCold Area Targeting");
    }

    public override List<Node> GetAbilityRange(Node node)
    {
        return Pathfinding.GetSquare(GameMaster.map, node, maxRange);
    }

    public override List<Node> GetDirectionalAreaTargeting(Node node, Vector3 direction)
    {
        return null; //Pathfinding.GetTriangle()
    }
}

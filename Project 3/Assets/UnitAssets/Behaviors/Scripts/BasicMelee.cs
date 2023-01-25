using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BasicMelee", menuName = "Behaviors/Basic Melee")]
public class BasicMelee : Behavior
{
    public override void TakeTurn() { }
    public override void FindTarget() { }
    public override void Move() 
    {
        MapManager.instance.Move(self, self.node.edges[0]);
    }
    public override void Attack() { }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Reckless", menuName = "Behaviors/Reckless")]
public class Reckless : Behavior
{
    #region Scriptable Obejct  Implementation
    public override void TakeTurn()
    {

    }

    public override void FindTarget()
    {

    }

    public override void Move()
    {
        // Specific Reckless Behavior
        Debug.Log("Reckless Unit Move");
    }

    public override void Attack()
    {

    }
    #endregion

}

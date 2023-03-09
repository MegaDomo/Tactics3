using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAnimationEventHandler : MonoBehaviour
{
    [Header("Unity References")]
    public Unit parentUnitScript;

    public void BasicAttack()
    {
        PlayerTurn.instance.DealDamage();
    }
}

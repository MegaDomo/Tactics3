using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ShoveAbility : Ability
{
    private int power;
    private int range;
    private int cooldown;

    #region Interface Implementation
    public int Power
    {
        get { return power; }
        set { power = value; }
    }
    public int Range
    {
        get { return range; }
        set { range = value; }
    }
    public int Cooldown
    {
        get { return cooldown; }
        set { cooldown = value; }
    }
    #endregion

    public int pushDistance;
    public override void Activate()
    { 

    }
}

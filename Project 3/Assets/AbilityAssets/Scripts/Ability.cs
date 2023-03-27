using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Ability
{
    public int Power { get; set; }
    public int Range{ get; set; }
    public int Cooldown{ get; set; }

    public void Activate();
}
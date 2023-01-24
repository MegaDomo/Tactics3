using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability : ScriptableObject
{
    public int power;
    public int range;
    public int cooldown;

    public virtual void Activate() { }
}

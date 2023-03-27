using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAbility", menuName = "Abilities/Ability")]
public class Ability : ScriptableObject
{
    public int power;

    public virtual void Activate() { }


    
    /*
    public int Power { get; set; }
    public int Range{ get; set; }
    public int Cooldown{ get; set; }

    public void Activate();*/
}
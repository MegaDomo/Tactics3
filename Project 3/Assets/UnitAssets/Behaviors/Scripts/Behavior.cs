using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Behavior : ScriptableObject
{
    public string name;

    [HideInInspector] public Unit target;
    [HideInInspector] public Unit self;

    public virtual void TakeTurn() { }
    public virtual void FindTarget() { }
    public virtual void Move() { }
    public virtual void Attack() { }
    
    // public virtual void GetBlackBoard() { }
}

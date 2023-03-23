using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Behavior : ScriptableObject
{
    public string name;

    [HideInInspector] public Unit target;
    [HideInInspector] public Unit self;
    [HideInInspector] public Unit tauntedPlayer;
    [HideInInspector] public Node destination;

    public virtual void TakeTurn() { }

    public void Move(Node destination)
    {
        self.Move(destination);
    }
}

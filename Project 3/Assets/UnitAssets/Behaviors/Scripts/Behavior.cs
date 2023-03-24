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
        if (destination == null)
        {
            Debug.Log(self.name + " Cannot move because Destination is Null");
            // TODO : trigger some idle behavior
            return;
        }

        self.Move(destination);
    }

    public Unit GetTauntedPlayer()
    {
        return tauntedPlayer;
    }

    public void SetTauntedPlayer(Unit tauntedPlayer)
    {
        this.tauntedPlayer = tauntedPlayer;
    }

}

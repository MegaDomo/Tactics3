using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Attacker", menuName = "Behaviors/Attacker")]
public class Attacker : Behavior
{
    // Default
    public Attacker()
    {

    }
    public override void TakeTurn() { }
    
    public override void FindTarget() 
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        List<Node> targets = new List<Node>();

        for (int i = 0; i < players.Length; i++)
        {
            targets.Add(players[i].GetComponent<Unit>().node);
        }

        target = FindClosestNode(targets).unit;
    }

    public override void FindNodeToTarget()
    {
        if (target == null)
            return;

        if (MapManager.instance.GetDistance(self.node, target.node) <= 1)
        {
            destination = self.node;
        }

        List<Node> nodes = MapManager.instance.pathing.GetNeighbors(target.node);

        if (nodes.Count == 0)
            return;

        destination = FindClosestNode(nodes);
    }

    public override Node FindClosestNode(List<Node> nodes)
    {
        Node node = new Node();
        int close= int.MaxValue;
        int temp;

        foreach (Node item in nodes)
        {
            temp = MapManager.instance.GetDistance(self.node, item);

            if (temp < close)
            {
                close = temp;
                node = item;
            }
        }

        return node;
    }

    public override void Move()
    {
        MapManager.instance.Move(self, destination);
    }
    public override void Attack() { }


}

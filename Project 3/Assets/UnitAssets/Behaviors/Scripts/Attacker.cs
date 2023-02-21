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

    // ==== TEMP : This Code finds the Closted Target - No Weights are used here =====
    public override bool FindTarget() 
    {
        List<Unit> players = BattleSystem.instance.players;

        List<Node> playerNodes = new List<Node>();
        foreach (Unit player in players)
            playerNodes.Add(player.node);

        target = FindClosestNode(playerNodes).unit;

        List<Node> targetNodes = new List<Node>();
        foreach (Node node1 in playerNodes)
            foreach (Node node2 in MapManager.instance.pathing.GetNeighbors(node1))
                targetNodes.Add(node2);

        destination = FindClosestNode(targetNodes);



        if (target == null)
        {
            Debug.Log("Enemy Could NOT Find Target");
            return false;
        }
        return true;
    }

    public override void FindNode()
    {/*
        if (target == null)
            return;

        if (MapManager.instance.GetDistance(self.node, target.node) <= 1)
        {
            destination = self.node;
        }

        List<Node> nodes = MapManager.instance.pathing.GetNeighbors(target.node);

        if (nodes.Count == 0)
            return;

        destination = FindClosestNode(nodes);*/
    }

    public override Node FindClosestNode(List<Node> nodes)
    {
        Node node = new Node();
        int close= int.MaxValue;
        int temp;

        foreach (Node item in nodes)
        {
            temp = MapManager.instance.pathing.GetPathCost(self.node, item);

            if (temp < close)
            {
                close = temp;
                node = item;
            }
        }

        return node;
    }
    public Node FindShortestPath(List<Node> nodes)
    {
        Node node = new Node();
        int close = int.MaxValue;
        int temp;

        foreach (Node item in nodes)
        {
            temp = MapManager.instance.pathing.GetPathCost(self.node, item);

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
        Grid<Node> map = MapManager.instance.map;
        Utils.CreateWorldTextPopupOnGrid(destination.x, destination.z,
                                   10f, "Moving Here", 30, map);
        MapManager.instance.MoveAsCloseAsPossible(self, destination);
    }
    public override void Attack() { }


}

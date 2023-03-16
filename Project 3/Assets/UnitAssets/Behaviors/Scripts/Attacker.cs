using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Attacker", menuName = "Behaviors/Attacker")]
public class Attacker : Behavior
{
    private WeaponSetHandler handler;
    private MapManager mapManager;
    private Grid<Node> map;

    public Attacker(MapManager mapManager, Unit self)
    {
        mapManager = MapManager.instance;
        map = mapManager.GetMap();
        this.self = self;
        handler = new WeaponSetHandler(self, mapManager, map);
    }

    public override void TakeTurn()
    {
        mapManager = MapManager.instance;
        List<Unit> players = BattleSystem.instance.players;
        List<Weapon> weapons = self.weapons;

        WeaponSet chosenSet;

        // Check for Taunt
        if (tauntedPlayer != null)
        {
            chosenSet = handler.GetMostDamagingWeaponSet(weapons, tauntedPlayer);
            DistributeSet(chosenSet);
            ChooseAction(players);
            return;
        }

        // Get Aggro
        chosenSet = handler.GetHighestAggroWeaponSet(weapons, players);
        DistributeSet(chosenSet);
        ChooseAction(players);
        return;


        //chosenSet = handler.GetMostDamagingWeaponSet(weapons, players);
    }

    #region Attacking
    private void MoveToAttack()
    {
        // TODO : Filter nodes to Move to minimum Range
        self.SetIsAttacking(true);
        Move(destination);
    }
    #endregion

    #region Moving
    private void MoveAsCloseAsPossible()
    {
        List<Node> neighbors = Pathfinding.GetPassibleNeighbors(map, target.node);
        if (neighbors.Count == 0)
        {
            // I believe getting here means no targets are in range whatsoever and the adjacent nodes of 
            // the closest target are blocked. 
            // This could be to perform another object or some kind of idle behavior
            return;
        }
        destination = Pathfinding.GetClosestPassibleNode(map, self.node, neighbors);

        List<Node> path = Pathfinding.GetClosestPath(map, self.node, destination, self);
        destination = path[path.Count - 1];

        Move(Pathfinding.GetClosestPassibleNode(map, self.node, destination));
    }
    #endregion

    #region Utility
    public bool FindClosestTarget(List<Unit> players)
    {
        // Gets all Nodes that players are on
        List<Node> playerNodes = new List<Node>();
        foreach (Unit player in players)
            playerNodes.Add(player.node);

        Node targetNode = Pathfinding.GetClosestNode(map, self.node, playerNodes);
        target = targetNode.unit;

        if (target == null)
        {
            Debug.Log("No Target for: " + self.name);
            return false;
        }
        return true;
    }

    private void ChooseAction(List<Unit> players)
    {
        if (target == null)
        {
            if (FindClosestTarget(players))
            {
                MoveAsCloseAsPossible();
            }
            return;
        }
        else
        {
            MoveToAttack();
            return;
        }
    }

    private void DistributeSet(WeaponSet set)
    {
        target = set.GetTarget();
        destination = set.GetDestination();
        self.SetWeapon(set.GetWeapon());
    }
    #endregion
}

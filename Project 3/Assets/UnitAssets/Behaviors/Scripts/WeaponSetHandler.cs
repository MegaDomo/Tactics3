using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSetHandler
{
    private Unit self;
    private Grid<Node> map;

    public WeaponSetHandler(Unit self, Grid<Node> map)
    {
        this.self = self;
        this.map = map;
    }

    #region Best Weapon Set for Most Damage
    public WeaponSet GetMostDamagingWeaponSet(List<Weapon> weapons, List<Unit> players)
    {
        List<WeaponSet> inRangeSets = FindWeaponSetsInRange(weapons, players);

        if (inRangeSets.Count == 0)
        {
            WeaponSet set = FindClosestTarget(players);
            set = GetClosestNodeInMovementRange(set);
            return set;
        }

        return FindHighestDamageSet(inRangeSets);
    }

    public WeaponSet GetMostDamagingWeaponSet(List<Weapon> weapons, Unit player)
    {
        List<WeaponSet> inRangeSets = FindWeaponSetsInRange(weapons, player);

        if (inRangeSets.Count == 0)
        {
            WeaponSet set = new WeaponSet(self, player, null);
            set = GetClosestNodeInMovementRange(set);
            return set;
        }
            
        return FindHighestDamageSet(inRangeSets);
    }

    private WeaponSet FindHighestDamageSet(List<WeaponSet> inRangeSets)
    {
        WeaponSet bestSet = new WeaponSet(null, null, null);
        
        bestSet = inRangeSets[0];

        foreach (WeaponSet set in inRangeSets)
            bestSet = CompareWeaponSetDamages(bestSet, set);

        return bestSet;
    }

    private WeaponSet CompareWeaponSetDamages(WeaponSet set1, WeaponSet set2)
    {
        if (set1.GetBestDamage() > set2.GetBestDamage())
            return set1;
        else
            return set2;
    }
    #endregion

    #region Utility
    private List<WeaponSet> FindWeaponSetsInRange(List<Weapon> weapons, List<Unit> players)
    {
        List<WeaponSet> inRangeSets = new List<WeaponSet>();
        // Filters out Weapons out of Range
        foreach (Unit player in players)
        {
            foreach (Weapon weapon in weapons)
            {
                WeaponSet set = new WeaponSet(self, player, weapon);
                if (InRange(set))
                    inRangeSets.Add(set);
            }
        }
        return inRangeSets;
    }

    private List<WeaponSet> FindWeaponSetsInRange(List<Weapon> weapons, Unit player)
    {
        List<WeaponSet> inRangeSets = new List<WeaponSet>();
        // Filters out Weapons out of Range
            foreach (Weapon weapon in weapons)
            {
                WeaponSet set = new WeaponSet(self, player, weapon);
                if (InRange(set))
                    inRangeSets.Add(set);
            }
        return inRangeSets;
    }

    private bool InRange(WeaponSet set)
    {
        Weapon weapon = set.GetWeapon();
        Unit player = set.GetTarget();

        List<Node> potentialAttackNodes = Pathfinding.GetHollowDiamond(map, player.node, weapon.range, weapon.minRange);

        foreach (Node node1 in potentialAttackNodes)
        {
            if (self.node == node1)
            {
                set.SetDestination(node1);
                return true;
            }
        }

        List<Node> nodesInRange = FindNodesInMovementRange(potentialAttackNodes, self.MovementLeft());

        if (nodesInRange.Count == 0)
            return false;

        Node node = Pathfinding.GetClosestPassibleNode(map, self.node, nodesInRange);

        if (node == null)
            return false;

        set.SetDestination(node);
        return true;
    }

    private List<Node> FindNodesInMovementRange(List<Node> nodes, int movement)
    {
        List<Node> nodesInRange = new List<Node>();

        foreach (Node node in nodes)
        {
            int temp = Pathfinding.GetPathCostWithStart(map, self.node, node);
            if (temp <= movement)
                nodesInRange.Add(node);
        }
        return nodesInRange;
    }

    public WeaponSet FindClosestTarget(List<Unit> players)
    {
        List<Node> playerNodes = new List<Node>();
        foreach (Unit player in players)
            playerNodes.Add(player.node);

        Node targetNode = Pathfinding.GetClosestNode(map, self.node, playerNodes);

        return new WeaponSet(self, targetNode.unit, null, targetNode);
    }

    public WeaponSet GetClosestNodeInMovementRange(WeaponSet set)
    {
        List<Node> neighbors = Pathfinding.GetPassibleNeighbors(map, set.GetTarget().node);
        if (neighbors.Count == 0)
        {
            // TODO : 
            // This indicates all possibilities are block and needs to trigger some idle behavior
            // Maybe Find Secondary Target
            return set;
        }

        Node destination = Pathfinding.GetClosestPassibleNode(map, self.node, neighbors);
        List<Node> path = Pathfinding.GetClosestPath(map, self.node, destination, self);
        set.SetDestination(path[path.Count - 1]);
        Debug.Log(path.Count);

        return set;
    }
    #endregion
}

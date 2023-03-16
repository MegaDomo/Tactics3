using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSetHandler
{
    private Unit self;
    private MapManager manager;
    private Grid<Node> map;

    public WeaponSetHandler(Unit self, MapManager manager, Grid<Node> map)
    {
        this.self = self;
        this.manager = manager;
        this.map = map;
    }

    #region Best Weapon Set for Most Damage
    public WeaponSet GetMostDamagingWeaponSet(List<Weapon> weapons, List<Unit> players)
    {
        List<WeaponSet> inRangeSets = FindWeaponSetsInRange(weapons, players);

        return FindHighestDamageSet(inRangeSets);
    }

    private WeaponSet FindHighestDamageSet(List<WeaponSet> inRangeSets)
    {
        WeaponSet bestSet = new WeaponSet(null, null, null);
        // No Weapon was in Range
        if (inRangeSets.Count == 0)
            return bestSet;
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

    #region Best Weapon Set for HighestAggro
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

    private bool InRange(WeaponSet set)
    {
        Weapon weapon = set.GetWeapon();
        Unit player = set.GetTarget();

        List<Node> potentialAttackNodes = Pathfinding.GetHollowDiamond(map, player.node, weapon.range, weapon.minRange);

        List<Node> nodesInRange = FindNodesInMovementRange(potentialAttackNodes, self.MovementLeft());

        if (nodesInRange.Count == 0)
            return false;

        Node node = Pathfinding.GetClosestPassibleNode(map, self.node, nodesInRange);
        set.SetDestination(node);
        return true;
    }

    private List<Node> FindNodesInMovementRange(List<Node> nodes, int movement)
    {
        List<Node> nodesInRange = new List<Node>();

        foreach (Node node in nodes)
        {
            int temp = Pathfinding.GetPathCost(map, self.node, node);
            if (temp <= movement)
                nodesInRange.Add(node);
        }
        return nodesInRange;
    }
    #endregion
}

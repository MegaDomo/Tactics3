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
                WeaponSet set = new WeaponSet(weapon, player, self);
                if (InRange(set.GetWeapon(), set.GetTarget()))
                    inRangeSets.Add(set);
            }
        }
        return inRangeSets;
    }

    private bool InRange(Weapon weapon, Unit player)
    {
        List<Node> potentialAttackNodes = Pathfinding.GetHollowDiamond(map, player.node, weapon.range, weapon.minRange);
        return FindNodeInMovementRange(potentialAttackNodes, self.MovementLeft());
    }

    private bool FindNodeInMovementRange(List<Node> nodes, int movement)
    {
        foreach (Node node in nodes)
        {
            int temp = Pathfinding.GetPathCostWithoutStart(map, self.node, node);
            if (temp <= movement)
                return true;
        }
        return false;
    }
    #endregion
}

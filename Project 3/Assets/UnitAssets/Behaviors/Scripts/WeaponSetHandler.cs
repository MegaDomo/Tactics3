using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSetHandler
{
    private Unit self;
    private MapManager manager;
    private Pathfinding pathfinding;

    public WeaponSetHandler(Unit self, MapManager manager, Pathfinding pathfinding)
    {
        this.self = self;
        this.manager = manager;
        this.pathfinding = pathfinding;
    }

    #region Best Weapon Set for Most Damage
    private WeaponSet FindBestWeaponSet(List<Weapon> weapons, List<Unit> players)
    {
        List<WeaponSet> inRangeSets = FindWeaponSetsInRange(weapons, players);

        return FindBestSet(inRangeSets);
    }

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

    private WeaponSet FindBestSet(List<WeaponSet> inRangeSets)
    {
        WeaponSet bestSet = new WeaponSet(null, null, null);
        // No Weapon was in Range
        if (inRangeSets.Count == 0)
            return bestSet;
        bestSet = inRangeSets[0];

        foreach (WeaponSet set in inRangeSets)
            bestSet = CompareWeaponSets(bestSet, set);

        return bestSet;
    }

    private WeaponSet CompareWeaponSets(WeaponSet set1, WeaponSet set2)
    {
        if (set1.GetBestDamage() > set2.GetBestDamage())
            return set1;
        else
            return set2;
    }

    private bool InRange(Weapon weapon, Unit player)
    {
        List<Node> potentialAttackNodes = pathfinding.GetHollowDiamond(player.node, weapon.range, weapon.minRange);
        return FindNodeInMovementRange(potentialAttackNodes, self.MovementLeft());
    }

    private bool FindNodeInMovementRange(List<Node> nodes, int movement)
    {
        foreach (Node node in nodes)
        {
            int temp = MapManager.instance.pathing.GetPathCostWithoutStart(self.node, node);
            Debug.Log(temp);
            if (temp <= movement)
                return true;
        }
        return false;
    }
    #endregion
}

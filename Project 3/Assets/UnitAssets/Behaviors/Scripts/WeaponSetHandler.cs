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

    #region Find Target
    // Applies all weapons to each Player
    private WeaponSet FindPlayerInWeaponRange(List<Unit> players, List<Weapon> weapons)
    {
        WeaponSet bestSet = new WeaponSet(weapons[0], players[0], self);
        foreach (Unit player in players)
        {
            foreach (Weapon weapon in weapons)
            {
                WeaponSet set = new WeaponSet(weapon, player, self);
                bestSet = CompareWeaponSets(bestSet, set);
            }            
        }
        return bestSet;
    }

    private WeaponSet CompareWeaponSets(WeaponSet set1, WeaponSet set2)
    {
        if (set1.GetBestDamage() > set2.GetBestDamage())
            return set1;
        else
            return set2;
    }
    #endregion

    private bool InRange(Unit player, Weapon weapon)
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

}

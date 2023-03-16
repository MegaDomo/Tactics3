using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Behaviorology
{
    private Unit self;
    private MapManager manager;
    private Grid<Node> map;
    private WeaponSetHandler handler;

    public Behaviorology(Unit self, MapManager manager, Grid<Node> map)
    {
        this.self = self;
        this.manager = manager;
        this.map = map;
        handler = new WeaponSetHandler(self, manager, map);
    }

    // A method that gets a list of players in WeaponAttack Range
    #region Get Player Methods
    public List<Unit> GetPlayersInWeaponRange(List<Unit> allPlayers, List<Weapon> weapons)
    {
        List<Unit> playersInWeaponRange = new List<Unit>();

        List<WeaponSet> inRangeSets = new List<WeaponSet>();
        // Filters out Weapons out of Range
        foreach (Unit player in allPlayers)
        {
            foreach (Weapon weapon in weapons)
            {
                if (InRange(player, weapon))
                    playersInWeaponRange.Add(player);
            }
        }
        return playersInWeaponRange;
    }

    public List<Unit> GetAggroPlayersInWeaponRange(List<Unit> allPlayers, List<Weapon> weapons)
    {
        List<Unit> aggroPlayersInWeaponRange = new List<Unit>();

        List<WeaponSet> inRangeSets = new List<WeaponSet>();
        // Filters out Weapons out of Range
        foreach (Unit player in allPlayers)
        {
            if (!player.stats.isAggressive)
                continue;
            foreach (Weapon weapon in weapons)
            {
                if (InRange(player, weapon))
                    aggroPlayersInWeaponRange.Add(player);
            }
        }
        return aggroPlayersInWeaponRange;
    }
    #endregion


    #region Aggro Methods
    public Unit AggroedPlayer(List<Unit> aggroPlayersInWeaponRange)
    {
        if (aggroPlayersInWeaponRange.Count == 0)
            return null;
        foreach (Unit unit in aggroPlayersInWeaponRange)
        {
            if (Random.Range(0, 100) < unit.stats.aggro)
                return unit;
        }
        return null;
    }
    #endregion



    #region Utility Methods
    private bool InRange(Unit player, Weapon weapon)
    {
        List<Node> potentialAttackNodes = Pathfinding.GetHollowDiamond(map, player.node, weapon.range, weapon.minRange);
        List<Node> nodesInRange = FindNodesInMovementRange(potentialAttackNodes, self.MovementLeft());

        if (nodesInRange.Count == 0)
            return false;
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

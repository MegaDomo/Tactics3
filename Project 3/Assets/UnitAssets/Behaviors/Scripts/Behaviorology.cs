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
    public Unit IsAggroed(List<Unit> allPlayers)
    {
        List<Unit> aggroPlayers = GetPlayersWithAggroSkill(allPlayers);
        if (aggroPlayers.Count == 0)
            return null;
        List<Unit> aggroPlayersInRange = GetPlayersInAggroRange(aggroPlayers);
        if (aggroPlayersInRange.Count == 0)
            return null;
        return AggroedPlayer(aggroPlayersInRange);
    }

    public List<Unit> GetPlayersWithAggroSkill(List<Unit> allPlayers)
    {
        List<Unit> aggroPlayers = new List<Unit>();

        foreach (Unit player in allPlayers)
        {
            if (player.stats.isAggressive)
                aggroPlayers.Add(player);
        }
        return aggroPlayers;
    }

    private List<Unit> GetPlayersInAggroRange(List<Unit> aggroPlayers)
    {
        List<Unit> aggroPlayersInRange = new List<Unit>();

        foreach (Unit player in aggroPlayers)
        {
            if (Pathfinding.GetDistance(map, self.node, player.node) <= player.stats.aggroRange)
                aggroPlayersInRange.Add(player);
        }
        return aggroPlayersInRange;
    }

    public Unit AggroedPlayer(List<Unit> aggroPlayersInRange)
    {
        aggroPlayersInRange = GetDescendingAggroList(aggroPlayersInRange);

        foreach (Unit player in aggroPlayersInRange)
        {
            if (Random.Range(0, 100) <= player.stats.aggro)
                return player;
        }
        return null;
    }

    private List<Unit> GetDescendingAggroList(List<Unit> aggroPlayersInRange)
    {
        List<int> aggroValue = new List<int>();
        List<Unit> temp = new List<Unit>();
        foreach (Unit player in aggroPlayersInRange)
            aggroValue.Add(player.stats.aggro);

        // Check if Descending
        aggroValue.Sort();
        aggroValue.Reverse();

        foreach (int value in aggroValue)
            foreach (Unit player in aggroPlayersInRange)
                if (value == player.stats.aggro)
                    temp.Add(player);

        return temp;
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

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Attacker", menuName = "Behaviors/Attacker")]
public class Attacker : Behavior
{
    private MapManager mapManager;
    private BattleSystem battle;
    private Behaviorology behaviorology;
    private WeaponSetHandler handler;
    private Grid<Node> map;

    public Attacker(MapManager mapManager, Unit self)
    {
        mapManager = MapManager.instance;
        battle = BattleSystem.instance;

        map = mapManager.GetMap();
        this.self = self;

        handler = new WeaponSetHandler(self, mapManager, map);
        behaviorology = new Behaviorology(self, mapManager, map);
    }

    public override void TakeTurn()
    {
        mapManager = MapManager.instance;
        List<Unit> players = battle.GetPlayers();
        List<Weapon> weapons = self.weapons;

        // Check for Taunt
        if (tauntedPlayer != null)
        {
            Taunted(tauntedPlayer, weapons, players);
            return;
        }

        // Get Aggro
        Unit aggroedPlayer = behaviorology.IsAggroed(players);
        if (aggroedPlayer != null)
        {
            Taunted(aggroedPlayer, weapons, players);
            return;
        }

        // Not Aggroed
        WeaponSet chosenSet = handler.GetMostDamagingWeaponSet(weapons, players);
        DistributeSet(chosenSet);
        ChooseAction(chosenSet);
    }

    #region Aggro
    public void Taunted(Unit tauntedPlayer, List<Weapon> weapons, List<Unit> players)
    {
        WeaponSet chosenSet = handler.GetMostDamagingWeaponSet(weapons, tauntedPlayer);
        DistributeSet(chosenSet);
        ChooseAction(chosenSet);
    }
    #endregion

    #region Attacking
    private void MoveToAttack()
    {
        // TODO : Filter nodes to Move to minimum Range
        self.SetIsAttacking(true);
        Move(destination);
    }
    #endregion

    #region Utility
    // Checking for range
    private void ChooseAction(WeaponSet set)
    {
        if (set.GetWeapon() != null)
            InRange();
        if (set.GetWeapon() == null)
            OutOfRange();
    }

    private void InRange()
    {
        self.SetIsAttacking(true);
        Move(destination);
    }

    private void OutOfRange()
    {
        Move(destination);
    }

    public void FindClosestTarget(List<Unit> players)
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

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Attacker", menuName = "Behaviors/Attacker")]
public class Attacker : Behavior
{
    WeaponSetHandler handler;
    MapManager mapManager;
    Pathfinding pathfinding;

    public Attacker(MapManager mapManager)
    {
        mapManager = MapManager.instance;
        pathfinding = mapManager.pathing;
        handler = new WeaponSetHandler(self, mapManager, pathfinding);
    }

    public override void TakeTurn()
    {

        List<Unit> players = BattleSystem.instance.players;
        List<Weapon> weapons = self.weapons;

        DistributeSet(handler.GetMostDamagingWeaponSet(weapons, players));
        
        if (target == null)
        {
            if (FindClosestTarget(players))
                Move(FindClosestNode(pathfinding.GetNeighbors(target.node)));
            return;
        }

        int nodeDistance = mapManager.GetDistance(self.node, target.node);
        Weapon weapon = self.equippedWeapon;
        if (nodeDistance <= weapon.range && nodeDistance >= weapon.minRange)
        {
            Attack();
            return;
        }

        MoveToAttack();

    }

    public bool FindClosestTarget(List<Unit> players)
    {
        // Gets all Nodes that players are on
        List<Node> playerNodes = new List<Node>();
        foreach (Unit player in players)
            playerNodes.Add(player.node);

        Node targetNode = FindClosestNode(playerNodes);
        target = targetNode.unit;

        if (target == null)
        {
            Debug.Log("No Target for: " + self.name);
            return false;
        }
        return true;
    }
    
    #region Attacking
    private void Attack()
    {
        int damage = self.stats.attack + self.equippedWeapon.damage;
        // TODO : Figure out whether physical or magical damage
        target.TakePhysicalDamage(damage);
        //self.anim.SetTrigger("MeleeStrike");
        //self.anim.ResetTrigger("MeleeStrike");
    }
    private void MoveToAttack()
    {
        // TODO : Filter nodes for minimum Range
        Weapon weapon = self.equippedWeapon;
        List<Node> potentialAttackNodes = pathfinding.GetHollowDiamond(target.node, weapon.range, weapon.minRange);
        Move(FindClosestNode(potentialAttackNodes));
        Attack();
    }
    #endregion

    #region Moving
    public void Move(Node destination)
    {
        Grid<Node> map = mapManager.map;
        Utils.CreateWorldTextPopupOnGrid(destination.x, destination.z,
                                   10f, "Moving Here", 30, map);
        mapManager.MoveAsCloseAsPossible(self, destination);
    }
    #endregion

    #region Utility
    public Node FindClosestNode(List<Node> nodes)
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

    private void DistributeSet(WeaponSet set)
    {
        target = set.GetTarget();
        self.SetWeapon(set.GetWeapon());
    }
    #endregion













    /*
    // ==== TEMP : This Code finds the Closted Target - No Weights are used here =====
    public  bool FindTarget() 
    {
        List<Unit> players = BattleSystem.instance.players;

        // Gets all Nodes that players are on
        List<Node> playerNodes = new List<Node>();
        foreach (Unit player in players)
            playerNodes.Add(player.node);

        Node targetNode = FindClosestNode(playerNodes);
        target = targetNode.unit;

        FindTargetDesitination(targetNode);
        
        if (target == null)
        {
            Debug.Log("Enemy Could NOT Find Target");
            return false;
        }
        return true;
    }

    public  void DecideAction()
    {
        // TEMP ===:=== This only applies to the units one ability
        // NOTE : need to include multiple weapon/Ability Ranges
        int nodeDistance = MapManager.instance.GetDistance(self.node, target.node);

        ChooseWeapon();
        Weapon weapon = self.equippedWeapon;
        if (nodeDistance <= weapon.range && nodeDistance >= weapon.minRange)
        {   
            Attack();
            return;
        }

        ChooseWeapon();
        weapon = self.equippedWeapon;
        if (CanMoveAndAttack())
        {
            MoveAndAttack();
            return;
        }

        // Move
    }

    private bool CanMoveAndAttack()
    {
        Pathfinding pathfinding = MapManager.instance.pathing;

        // TODO : Filter through list of weapons
        

        List<Node> potentialAttackNodes = pathfinding.GetDiamond(target.node, self.equippedWeapon.range);
        destination = FindClosestNode(potentialAttackNodes);

        
        return true;
    }

    public void Attack() 
    { 

    }

    public void MoveAndAttack()
    {
        Move(destination);
        Attack();
    }







    public void FindTargetDesitination(Node targetNode)
    {
        List<Node> targetNodes = new List<Node>();
        foreach (Node node in MapManager.instance.pathing.GetNeighbors(targetNode))
            targetNodes.Add(node);

        destination = FindClosestNode(targetNodes);

    }

    public  Node FindClosestNode(List<Node> nodes)
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

    public  void Move(Node destination)
    {
        Grid<Node> map = MapManager.instance.map;
        Utils.CreateWorldTextPopupOnGrid(destination.x, destination.z,
                                   10f, "Moving Here", 30, map);
        MapManager.instance.MoveAsCloseAsPossible(self, destination);
    }

    public void ChooseWeapon()
    {
        self.equippedWeapon = self.weapons[0];
    }
    */
}

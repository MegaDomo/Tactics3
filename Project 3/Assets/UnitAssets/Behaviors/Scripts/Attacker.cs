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

        DistributeSet(handler.GetMostDamagingWeaponSet(weapons, players));
        
        // Just Moving
        if (target == null)
        {
            if (FindClosestTarget(players))
                MoveAsCloseAsPossible();

            return;
        }

        MoveToAttack();

        /*int nodeDistance = mapManager.GetDistance(self.node, target.node);
        Weapon weapon = self.equippedWeapon;
        if (nodeDistance <= weapon.range && nodeDistance >= weapon.minRange)
        {
            
            return;
        }*/
    }

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
    
    #region Attacking
    private void MoveToAttack()
    {
        // TODO : Filter nodes for minimum Range
        self.SetIsAttacking(true);

        Weapon weapon = self.equippedWeapon;
        List<Node> potentialAttackNodes = Pathfinding.GetHollowDiamond(map, target.node, weapon.range, weapon.minRange);
        Move(Pathfinding.GetClosestPassibleNode(map, self.node, potentialAttackNodes));
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

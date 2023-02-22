using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Attacker", menuName = "Behaviors/Attacker")]
public class Attacker : Behavior
{
    Pathfinding pathfinding;
    public override void TakeTurn()
    {
        pathfinding = MapManager.instance.pathing;

        List<Unit> players = BattleSystem.instance.players;
        List<Weapon> weapons = self.weapons;
        FindTarget(players, weapons);

        // Move
        MoveToTarget();
    }

    #region Find Target
    private void FindTarget(List<Unit> players, List<Weapon> weapons)
    {
        // Need to find Target(Unit), Weapon, destination(Node)
        Tuple<Weapon, int, Unit> bestSet = Tuple.Create<Weapon, int, Unit>(weapons[0], Int32.MinValue, new Unit());

        foreach (Unit player in players)
        {
            // Apply all weapons to a player
            Tuple<Weapon, int> set = GetBestWeaponAgainstPlayer(weapons, player);
            if (set.Item2 == Int32.MinValue)
                continue;

            bestSet = CompareWeaponSets(bestSet, set, player);
        }

        target = bestSet.Item3;
    }

    private Tuple<Weapon, int, Unit> CompareWeaponSets(Tuple<Weapon, int, Unit> bestSet, Tuple<Weapon, int> set, Unit nextPlayer)
    {
        Tuple<Weapon, int, Unit> playerSet;

        if (bestSet.Item2 < set.Item2)
        {
            playerSet = Tuple.Create<Weapon, int, Unit>(set.Item1, set.Item2, nextPlayer);
            return playerSet;
        }
            
        return bestSet;
    }

    private Tuple<Weapon, int> GetBestWeaponAgainstPlayer(List<Weapon> weapons, Unit player)
    {
        int bestDamage = Int32.MinValue;
        Weapon bestWeapon = new Weapon();
        Tuple<Weapon, int> bestSet = Tuple.Create<Weapon, int>(bestWeapon, bestDamage);
        

        foreach (Weapon weapon in weapons)
        {
            if (InRange(player, weapon))
                continue;
            bestSet = CompareWeapons(bestSet, weapon, player);
        }
        return bestSet;
    }

    private bool InRange(Unit player, Weapon weapon)
    {
        List<Node> potentialAttackNodes = pathfinding.GetDiamond(player.node, weapon.range);
        return FindNodeInMovementRange(potentialAttackNodes, self.MovementLeft());
    }

    private bool FindNodeInMovementRange(List<Node> nodes, int movement)
    {
        foreach (Node node in nodes)
        {
            int temp = MapManager.instance.pathing.GetPathCost(self.node, node);

            if (temp <= movement)
                return true;
        }
        return false;
    }

    private Tuple<Weapon, int> CompareWeapons(Tuple<Weapon, int> bestSet, Weapon incomingWeapon, Unit player)
    {
        int damage = incomingWeapon.damage + self.stats.attack - player.stats.defense;

        if (bestSet.Item2 < damage)
        {
            Tuple<Weapon, int> newBestSet = Tuple.Create<Weapon, int>(incomingWeapon, damage);
            return newBestSet;
        }
        
        return bestSet;
    }
    #endregion

    #region Moving
    private void MoveToTarget()
    {
        List<Node> adjNodes = pathfinding.GetNeighbors(target.node);
        Move(FindClosestNode(adjNodes));
    }

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

    public void Move(Node destination)
    {
        Grid<Node> map = MapManager.instance.map;
        Utils.CreateWorldTextPopupOnGrid(destination.x, destination.z,
                                   10f, "Moving Here", 30, map);
        MapManager.instance.MoveAsCloseAsPossible(self, destination);
    }
    #endregion












    private void CheckBestTarget(Tuple<Weapon, int> combo, Unit player)
    {
        // target = p1
        // compare incoming combo against p1, combo = p2
        //
    }

    



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

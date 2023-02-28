using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Attacker", menuName = "Behaviors/Attacker")]
public class Attacker : Behavior
{
    Pathfinding pathfinding;
    MapManager mapManager;

    public override void TakeTurn()
    {
        mapManager = MapManager.instance;
        pathfinding = mapManager.pathing;

        List<Unit> players = BattleSystem.instance.players;
        List<Weapon> weapons = self.weapons;
        FindTargetInWeaponRange(players, weapons);
        
        if (target == null || self.equippedWeapon == null)
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

    #region Find Target
    private void FindTargetInWeaponRange(List<Unit> players, List<Weapon> weapons)
    {
        Tuple<Weapon, int, Unit> bestPlayerSet = Tuple.Create<Weapon, int, Unit>(null, 0, null);

        foreach (Unit player in players)
        {
            // Apply all weapons to a player
            Tuple<Weapon, int> set = GetBestWeaponAgainstPlayer(weapons, player);

            bestPlayerSet = CompareWeaponSets(bestPlayerSet, set, player);            
        }

        target = bestPlayerSet.Item3;
        self.equippedWeapon = bestPlayerSet.Item1;
    }

    private Tuple<Weapon, int> GetBestWeaponAgainstPlayer(List<Weapon> weapons, Unit player)
    {
        Tuple<Weapon, int> bestWeaponSet = Tuple.Create<Weapon, int>(null, 0);

        foreach (Weapon weapon in weapons)
        {
            if (!InRange(player, weapon))
                continue;
            Debug.Log("In Range" + weapon.name);
            bestWeaponSet = CompareWeapons(bestWeaponSet, weapon, player);
        }
        return bestWeaponSet;
    }

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

    private Tuple<Weapon, int> CompareWeapons(Tuple<Weapon, int> bestWeaponSet, Weapon incomingWeapon, Unit player)
    {
        int damage = -1;
        if (incomingWeapon.weaponType == Weapon.WeaponType.Physical)
            damage = player.ForecastPhysicalDamage(incomingWeapon.damage + self.stats.attack);
        if (incomingWeapon.weaponType == Weapon.WeaponType.Magical)
            damage = player.ForecastMagicalDamage(incomingWeapon.damage + self.stats.spAttack);

        if (bestWeaponSet.Item2 < damage)
        {
            Tuple<Weapon, int> newBestSet = Tuple.Create<Weapon, int>(incomingWeapon, damage);
            return newBestSet;
        }

        return bestWeaponSet;
    }

    private Tuple<Weapon, int, Unit> CompareWeaponSets(Tuple<Weapon, int, Unit> bestSet, Tuple<Weapon, int> set, Unit nextPlayer)
    {
        Tuple<Weapon, int, Unit> playerSet;

        //Debug.Log(bestSet.Item2 + " < " + set.Item2);
        if (bestSet.Item2 < set.Item2)
        {
            playerSet = Tuple.Create<Weapon, int, Unit>(set.Item1, set.Item2, nextPlayer);
            return playerSet;
        }
            
        return bestSet;
    }
    #endregion

    #region Attacking
    private void Attack()
    {
        int damage = self.stats.attack + self.equippedWeapon.damage;
        // TODO : Figure out whether physical or magical damage
        target.TakePhysicalDamage(damage);
        self.anim.SetTrigger("MeleeStrike");
        self.anim.ResetTrigger("MeleeStrike");
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
        Grid<Node> map = mapManager.map;
        Utils.CreateWorldTextPopupOnGrid(destination.x, destination.z,
                                   10f, "Moving Here", 30, map);
        mapManager.MoveAsCloseAsPossible(self, destination);
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

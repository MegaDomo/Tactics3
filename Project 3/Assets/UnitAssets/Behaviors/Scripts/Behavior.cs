using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Behavior : ScriptableObject
{
    public string name;

    [HideInInspector] public Unit target;
    [HideInInspector] public Unit self;
    [HideInInspector] public Unit tauntedPlayer;
    [HideInInspector] public Node destination;

    public virtual void TakeTurn() { }

    public void Move(Node destination)
    {
        self.Move(destination);
    }
    // public virtual void GetBlackBoard() { }
    /*
         * "Player"
         * "Enemy"
         * "Neutral"
         * "Civilian"
         * 
         * 
         * 
        // Gets List a of Players
        Dictionary<Unit, int> players = new List<Unit, int>();
        foreach (Unit unit in All Units)
        {
            if (unit != Enemy) // Tag/name or something
                players.Add(unit, 0);
        }

        // Gets a List of Weights per player
        foreach (Unit player in players)
        {
            ===== Distance =====
            Get distance from this unit to player
            +10
            ===== Health =====
            // If this unit can kill player
            if (ProjectedDamage(player.stats) >= player.stats.curHealth)
                players[player] += 100;
            
            // Current Health %
            // % Health Missing - player.stats.curHealt/player.stats.maxHealth
        
            // How much damage is forecasted
            ProjectedDamage(player.stats) / Unit.attackStat

            15 / 20 = 75% of his maximum damage
            25 / 20 = 125% which is very desirable, should be weighted on exponential at this point

            // ===== Abilites =====
            // players[player] += player.stats.aggro

            if (players[player] < 0)
                players[player] = 0;  
        }

        // This Method needs to get max damage forecast from stats and ability's Power (Tentative) compared against player health
        // High Profile Method / Lots of Traffic
        // Needs to check resistances, Defenses, Sp. Defenses, all that stuff
        // returns how much Damage is Forecasted
        int ProjectedDamage(UnitStats stats)
        {
        }










        // Ideas
        // Random.Range(0, 3)
        // 1 - Most damage to a target
        // 2 - Least health Target
        // 3 - Closest
        // 4 - 
        // 5 - 
        // 6 - 

        // Mercy
        // If (there are 4 more enemies than Players)
        // Target the highest health player

        // Pick a Primary Target. Doesn't matter if out of true range; 4
        // if out of true range, Check to see who is in true range, 5: 1, 3, 5
        */
}

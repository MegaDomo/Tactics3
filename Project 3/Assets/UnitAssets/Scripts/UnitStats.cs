using UnityEngine;

[System.Serializable]
public class UnitStats
{
    [Header("Attributes")]
    public bool isAggressive;

    [Header("Character Stats")]
    public int maxHealth;
    public int curHealth;
    public int movement;

    public int vision;
    public int speed;
    public int attack;
    public int spAttack;
    public int defense;
    public int spDefense;
    public int accuracy;
    public int avoidance;
    public int critChance;
    public int critDamage;
    public int inflictionChance;
    public int inflictionResist;

    [Header("Hidden Stats")]
    public int aggro;
    public int aggroRange = 5;
    public int moved = 0;
    public int pathingSpeed = 50;
    
    public UnitStats(UnitStats stats)
    {
        isAggressive = stats.isAggressive;

        maxHealth = stats.maxHealth;
        curHealth = stats.curHealth;
        movement = stats.movement;

        vision = stats.vision;
        speed = stats.speed;
        attack = stats.attack;
        spAttack = stats.spAttack;
        defense = stats.defense;
        spDefense = stats.spDefense;
        accuracy = stats.accuracy;
        avoidance = stats.avoidance;
        critChance = stats.critChance;
        critDamage = stats.critDamage;
        inflictionChance = stats.inflictionChance;
        inflictionResist = stats.inflictionResist;

        aggro = stats.aggro;
        aggroRange = stats.aggroRange;
        moved = stats.moved;
        pathingSpeed = stats.pathingSpeed;
    }
}

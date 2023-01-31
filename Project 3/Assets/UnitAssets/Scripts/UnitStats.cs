using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UnitStats
{
    [Header("Stats")]
    public int maxHealth;
    public int curHealth;
    public int movement;
    public int speed;
    public int vision;

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
    
    




    #region Background Stats
    [HideInInspector] public int moved;
    #endregion
}

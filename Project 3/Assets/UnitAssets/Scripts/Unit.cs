using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    //public enum UnitType { Player, Enemy, Neutral, Ally, Civilian }

    [Header("Unity References")]
    public Transform ground;

    [Header("Attributes")]
    //public UnitType unitType;
    public UnitStats stats;
    public string type; // "Player", "Enemy"    
    public List<Ability> abilities;

    [Header("Debugging")]
    public Behavior behavior;

    [HideInInspector] public Node node;
    [HideInInspector] public Vector3 offset;

    void Awake()
    {
        offset = transform.position - ground.position;

        if (type == "Enemy")
            behavior.self = this;
    }

    public void StartTurn()
    {
        // Resets how far he moved
        stats.moved = 0;




        // TODO : Handle Status Effects / Tile Effects

    }

    public void EndTurn()
    {

    }

}

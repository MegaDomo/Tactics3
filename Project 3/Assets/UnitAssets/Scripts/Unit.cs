using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [Header("Unity References")]
    public Transform ground;

    public UnitStats stats;

    [Header("Attributes")]
    public string type; // "Player", "Enemy"
    public Behavior behavior;

    public List<Ability> abilities;

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

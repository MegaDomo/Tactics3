using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewUnit", menuName = "Units/Enemy")]
public class EnemyObject : ScriptableObject
{
    public enum BehaviorType { Attacker, Killer }

    [Header("References")]
    public AnimatorOverrideController overrideController;

    [Header("Attributes")]
    public BehaviorType behaviorType;
    public UnitStats stats;
}

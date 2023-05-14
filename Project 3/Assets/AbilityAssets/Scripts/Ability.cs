using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAbility", menuName = "Abilities/Ability")]
public class Ability : ScriptableObject
{
    public enum TargetType { StandingSingleTarget, DirectedSingleTarget, StandingAoe, DirectedAoe, DirectionalAOE }

    public enum AnimationType { BasicAttack, Squat }

    [Header("Attributes")]
    public TargetType targetType;
    public AnimationType animationType;
    public Weapon.DamageType damageType;
    public int power;
    public int effectDuration;
    public int minRange;
    public int maxRange;
    public int aoeMinRange;
    public int aoeMaxRange;
    // public Effect effect // Buffs/Debuffs

    [Header("References")]
    public Sprite iconSprite;
    public GameObject Effect;

    public string GetAnimationTypeString()
    {
        return animationType.ToString();
    }

    public virtual void DirectTargeting(Unit player) { }

    public virtual void AreaTargeting(Unit player) { }

    public virtual List<Node> GetAreaTargeting(Node node) { return null; }

    public virtual List<Node> GetAbilityRange(Node node) { return null; }
}
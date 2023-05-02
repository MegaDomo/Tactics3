using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAbility", menuName = "Abilities/Ability")]
public class Ability : ScriptableObject
{
    public enum TargetType { DirectDamage, AreaDamage, DirectHealing, AreaHealing, DirectEffect, AreaEffect}

    public TargetType targetType;
    public Weapon.DamageType damageType;
    public int power;
    public int effectDuration;
    public int minRange;
    public int maxRange;
    public int aoeMinRange;
    public int aoeMaxRange;
    // public Effect effect // Buffs/Debuffs

    public void Activate(List<Unit> units)
    {
        switch (targetType)
        {
            case TargetType.DirectDamage:
                DirectDamage(units);
                break;
            case TargetType.DirectHealing:
                DirectHealing(units);
                break;
            case TargetType.DirectEffect:
                DirectEffect(units);
                break;
            default:
                Debug.Log("Direct TargetType Case went wrong");
                break;
        }
    }

    public void Activate(List<Node> nodes)
    {
        switch (targetType)
        {
            case TargetType.AreaDamage:
                AreaDamage(nodes);
                break;
            case TargetType.AreaHealing:
                AreaHealing(nodes);
                break;
            case TargetType.AreaEffect:
                AreaEffect(nodes);
                break;
            default:
                Debug.Log("Area TargetType Case went wrong");
                break;
        }
    }

    public virtual void DirectDamage(List<Unit> units) { }

    public virtual void AreaDamage(List<Node> nodes) { }

    public virtual void DirectHealing(List<Unit> units) { }

    public virtual void AreaHealing(List<Node> nodes) { }

    public virtual void DirectEffect(List<Unit> units) { }
    
    public virtual void AreaEffect(List<Node> nodes) { }
}
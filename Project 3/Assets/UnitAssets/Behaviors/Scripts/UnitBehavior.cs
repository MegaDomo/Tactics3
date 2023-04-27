public class UnitBehavior
{
    private Unit unit;
    private Behavior behavior;

    public UnitBehavior(Unit unit)
    {
        this.unit = unit;

        SetBehavior(unit.behaviorType);
        behavior.self = unit;
    }

    private void SetBehavior(Unit.BehaviorType behaviorType)
    {
        if (behaviorType == Unit.BehaviorType.Attacker)
            behavior = new Attacker(unit);

        if (behaviorType == Unit.BehaviorType.Killer)
            behavior = new Killer();
        // TODO : Add other Behaviors and compensate their constructors
    }

    public Behavior GetBehavior()
    {
        return behavior;
    }
}

public class UnitBehavior
{
    private Unit unit;
    private Behavior behavior;

    public UnitBehavior(Unit unit, Grid<Node> map, Unit.BehaviorType behaviorType)
    {
        this.unit = unit;

        SetBehavior(map, behaviorType);
        behavior.self = unit;
    }

    private void SetBehavior(Grid<Node> map, Unit.BehaviorType behaviorType)
    {
        if (behaviorType == Unit.BehaviorType.Attacker)
            behavior = new Attacker(map, unit);

        if (behaviorType == Unit.BehaviorType.Killer)
            behavior = new Killer();
        // TODO : Add other Behaviors and compensate their constructors
    }

    public Behavior GetBehavior()
    {
        return behavior;
    }
}

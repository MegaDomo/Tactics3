public class UnitBehavior
{
    private Unit unit;
    private Behavior behavior;

    public UnitBehavior(BattleSystem battleSystem, Unit unit, Grid<Node> map, Unit.BehaviorType behaviorType)
    {
        this.unit = unit;

        SetBehavior(battleSystem, map, behaviorType);
        behavior.self = unit;
    }

    private void SetBehavior(BattleSystem battleSystem, Grid<Node> map, Unit.BehaviorType behaviorType)
    {
        if (behaviorType == Unit.BehaviorType.Attacker)
            behavior = new Attacker(battleSystem, map, unit);

        if (behaviorType == Unit.BehaviorType.Killer)
            behavior = new Killer();
        // TODO : Add other Behaviors and compensate their constructors
    }

    public Behavior GetBehavior()
    {
        return behavior;
    }
}

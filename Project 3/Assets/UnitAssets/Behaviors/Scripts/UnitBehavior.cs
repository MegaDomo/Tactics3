public class UnitBehavior
{
    private Unit unit;
    private Behavior behavior;
    private EnemyObject.BehaviorType behaviorType;

    public UnitBehavior(Unit unit, Grid<Node> map, EnemyObject enemyObject)
    {
        this.unit = unit;
        behaviorType = enemyObject.behaviorType;

        SetBehavior(map);
        behavior.self = unit;
    }

    private void SetBehavior(Grid<Node> map)
    {
        if (behaviorType == EnemyObject.BehaviorType.Attacker)
            behavior = new Attacker(map, unit);

        if (behaviorType == EnemyObject.BehaviorType.Killer)
            behavior = new Killer();
        // TODO : Add other Behaviors and compensate their constructors
    }

    public Behavior GetBehavior()
    {
        return behavior;
    }
}

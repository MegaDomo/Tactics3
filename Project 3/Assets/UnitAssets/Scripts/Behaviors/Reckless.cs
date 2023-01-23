using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reckless : MonoBehaviour, IBehavior
{
    private string name;
    private Unit self;
    private Unit target;

    #region Interface Implementation
    public new string Name { get => name; set => name = value; }
    public Unit Self { get => self; set => self = value; }
    public Unit Target { get => target; set => target = value; }

    public void TakeTurn()
    {

    }

    public void FindTarget()
    {

    }

    public void Move()
    {
        // Specific Reckless Behavior
        Debug.Log("Reckless Unit Move");
    }

    public void Attack()
    {

    }
    #endregion

    void Start()
    {
        BehaviorManager.instance.AddBehavior(this);
    }
}

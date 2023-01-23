using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBehavior
{
    public string Name { get; set; }
    public Unit Self { get; set; }
    public Unit Target { get; set; }

    public void TakeTurn();
    public void FindTarget();
    public void Move();
    public void Attack();
}

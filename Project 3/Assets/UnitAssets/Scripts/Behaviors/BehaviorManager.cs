using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviorManager : MonoBehaviour
{
    #region Singleton
    public static BehaviorManager instance;

    void Awake()
    {
        instance = this;
    }
    #endregion

    public List<IBehavior> behaviors = new List<IBehavior>();

    public void AddBehavior(IBehavior _behavior)
    {
        behaviors.Add(_behavior);
    }
}

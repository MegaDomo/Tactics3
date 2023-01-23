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

    List<Behavior> behaviors;

    void Start()
    {
        // Adds every kind of Behavior into a list
        behaviors.Add(new Reckless());
        behaviors.Add(new BasicRanged());
    }
}

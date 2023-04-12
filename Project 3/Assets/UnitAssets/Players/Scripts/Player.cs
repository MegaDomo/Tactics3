using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// MiddleMan ScriptableObject
[CreateAssetMenu(fileName = "NewPlayer", menuName = "Units/Player")]
public class Player : Unit
{
    public new string name;
    public Sprite portrait;
    public Sprite fullBody;
    public GameObject prefab;
    public UnitStats stats;

    private void OnEnable()
    {
        stats.curHealth = stats.maxHealth;
        if (healthChangeEvent == null)
            healthChangeEvent = new UnityEvent<int, int>();
    }

    #region Getters & Setters
    
    #endregion
}

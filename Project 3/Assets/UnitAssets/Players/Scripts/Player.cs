using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// MiddleMan ScriptableObject
[CreateAssetMenu(fileName = "NewPlayer", menuName = "Units/Player")]
public class Player : Unit, IUnit
{
    public new string name;
    private string testString;
    public Sprite portrait;
    public Sprite fullBody;
    public GameObject prefab;

    public string TestString { get => testString; set => testString = value; }

    private void OnEnable()
    {
        stats.curHealth = stats.maxHealth;
        if (healthChangeEvent == null)
            healthChangeEvent = new UnityEvent<int, int>();
    }

    #region Getters & Setters
    public void Test()
    {
        
    }
    #endregion
}

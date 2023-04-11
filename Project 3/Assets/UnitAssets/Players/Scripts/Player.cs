using UnityEngine;
using UnityEngine.Events;

// MiddleMan ScriptableObject
[CreateAssetMenu(fileName = "NewPlayer", menuName = "Units/Player")]
public class Player : ScriptableObject
{
    public new string name;
    public Sprite portrait;
    public Sprite fullBody;
    public GameObject prefab;
    public UnitStats stats;

    [System.NonSerialized] public Unit unit;
    [System.NonSerialized] public UnityEvent<int, int> healthChangeEvent;

    private Grid<Node> map;

    private void OnEnable()
    {
        stats.curHealth = stats.maxHealth;
        if (healthChangeEvent == null)
            healthChangeEvent = new UnityEvent<int, int>();
    }

    public void DecreaseHealth(int amount)
    {
        stats.curHealth -= amount;
        healthChangeEvent.Invoke(stats.curHealth, stats.maxHealth);
    }

    public void SetHealth(int amount)
    {
        stats.maxHealth = amount;
        stats.curHealth = stats.maxHealth;
        healthChangeEvent.Invoke(stats.curHealth, stats.maxHealth);
    }

    #region Getters & Setters
    public Grid<Node> GetMap()
    {
        return map;
    }

    public void SetMap(Grid<Node> map)
    {
        this.map = map;
    }
    #endregion
}

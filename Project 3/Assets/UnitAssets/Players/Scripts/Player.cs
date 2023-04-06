using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "NewPlayer", menuName = "Units/Player")]
public class Player : ScriptableObject
{
    public new string name;
    public Sprite portrait; 
    public UnitStats stats;

    [System.NonSerialized]
    public UnityEvent<int, int> healthChangeEvent;

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
}

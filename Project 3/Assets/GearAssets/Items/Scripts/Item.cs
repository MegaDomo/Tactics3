using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Gear/Item")]
public class Item : ScriptableObject
{
    public enum ItemType { Consumable, Nonconsumable, Passive, Junk }

    public string name;
    public ItemType itemType;
}

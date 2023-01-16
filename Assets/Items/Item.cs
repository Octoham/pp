using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Items/Item", order = 1)]
public class Item : ScriptableObject
{
    [Header("Item")]
    public string itemName;
    public string itemNamespace; // NO SPACES OR CAPS try to make it unique
    public string internalName; // NO SPACES OR CAPS must be unique in the namespace
    public string technicalName => itemNamespace + ":" + internalName;
    public string itemDescription;
    public Sprite itemSprite;
    public enum ItemType { Weapon, Consumable, Equippable, Generic };
    public ItemType itemType;
    public int maxStack;

}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[System.Serializable]
public class Item
{
    public enum ItemType
    {
        Food,
        Weapon,
        Apparel,
        Crafting,
        Ingredients,
        Potions,
        Scrolls,
        Quest,
        Money
    }

    #region Private Variables
    [Title("Item Information")]
    [SerializeField]private string name; // item ID
    [TextArea]
    [SerializeField]private string description;
    [SerializeField]private int value;
    [SerializeField]private int amount;
    [SerializeField]private Texture2D icon;
    [SerializeField]private GameObject mesh;
    [SerializeField]private ItemType item;
    [SerializeField]private int damage;
    [SerializeField]private int armour;
    [SerializeField]private int heal;
    #endregion
    #region Public Properties
    public string Name
    {
        get { return name; }
        set { name = value; }
    }

    public string Description
    {
        get { return description; }
        set { description = value; }
    }

    public int Value
    {
        get { return value; }
        set { this.value = value; }
    }

    public int Amount
    {
        get { return amount; }
        set { amount = value; }
    }

    public Texture2D Icon
    {
        get { return icon; }
        set { icon = value; }
    }

    public GameObject Mesh
    {
        get { return Mesh; }
        set { Mesh = value; }
    }

    public ItemType Type
    {
        get { return item; }
        set { item = value; }
    }

    public int Damage
    {
        get { return damage; }
        set { damage = value; }
    }

    public int Armour
    {
        get { return armour; }
        set { armour = value; }
    }

    public int Heal
    {
        get { return heal; }
        set { heal = value; }
    }
    #endregion

    public Item()
    {

    }

    public Item(Item copyItem, int copyAmount)
    {
        Name = copyItem.Name;
        Description = copyItem.Description;
        Value = copyItem.Value;
        Amount = copyAmount;
        Icon = copyItem.Icon;
        Mesh = copyItem.Mesh;
        Type = copyItem.Type;
        Damage = copyItem.Damage;
        Armour = copyItem.Armour;
        Heal = copyItem.Heal;

    }
}

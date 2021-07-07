using System;
using FPSProject.Utils;
using UnityEngine;
using Sirenix.OdinInspector;

[System.Serializable]
[CreateAssetMenu(fileName = "itemType", menuName = "Inventory")]
public class Item : ScriptableObject
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
    [SerializeField]private Mesh mesh;
    [SerializeField] private GameObject obj;
    [SerializeField]private ItemType item;
    [SerializeField]private int damage;
    [SerializeField]private int armour;
    [SerializeField]private int heal;
    #endregion
    #region Public Properties
    public string Name
    {
        get => name;
        set => name = value;
    }

    public string Description
    {
        get => description;
        set => description = value;
    }

    public int Value
    {
        get => value;
        set => this.value = value;
    }

    public int Amount
    {
        get => amount;
        set => amount = value;
    }

    public Texture2D Icon
    {
        get => icon;
        set => icon = value;
    }

    public Mesh Mesh
    {
        get => mesh;
        set => mesh = value;
    }

    public GameObject OBJ
    {
        get => obj;
        set => obj = value;
    }

    public ItemType Type
    {
        get => item;
        set => item = value;
    }

    public int Damage
    {
        get => damage;
        set => damage = value;
    }

    public int Armour
    {
        get => armour;
        set => armour = value;
    }

    public int Heal
    {
        get => heal;
        set => heal = value;
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
        OBJ = copyItem.OBJ;
    }
    private void OnValidate()
    {
        Mesh = TextureUtils.LoadMeshResource(Name);
        OBJ = TextureUtils.LoadGameObjectResource(Name);
        Icon = TextureUtils.LoadTextureResource(Name);
    }
}

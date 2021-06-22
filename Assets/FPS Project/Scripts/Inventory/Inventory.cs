using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using FPSProject.Player;
using FPSProject.Utils;
using UnityEngine.Serialization;

public class Inventory : MonoBehaviour
{
    #region Variables
    //[SerializeField] private Item inventory;
    [SerializeField] private List<Transform> inventoryItems = new List<Transform>();
    [SerializeField] private Transform player;
    [SerializeField] private Transform contentPosition;
    [SerializeField] private GameObject prefab;
    [SerializeField] private ToolTip toolTip;
    [SerializeField] private GameObject menu;
    //[SerializeField] private bool showInventory = true;
    private Item selectedItem = null;

    #region Display Inventory
    //private Vector2 scrollPosition;
    [SerializeField] private string sortType = "All";
    #endregion
    #endregion
    private void Start() => player = FindObjectOfType<PlayerController>().transform;

    #region Sorting
    /// <summary>
    /// Sorts the inventory displayed items by type
    /// </summary>
    /// <param name="_sortIndex">the index position of the item types list</param>
    public void SortByType(int _sortIndex)
    {
        // creates a string that adds all of the item types to it as string
        List<string> itemTypes = new List<string>(Enum.GetNames(typeof(Item.ItemType)));
        itemTypes.Insert(0, "All"); // inserts All at index 0
        
        // sets the sort type
        sortType = itemTypes[_sortIndex];

        SortItems();
    }

    private void SortItems()
    {
        foreach (var t in inventoryItems)
        {
            var obj = t.GetComponent<ItemDisplay>().item;
            
            if (obj.Type.ToString() == sortType || sortType == "All") t.gameObject.SetActive(true);
            else t.gameObject.SetActive(false);
        }
    }
    #endregion
    #region AddingToInventory
    /// <summary>
    /// Handles the spawning of display items in the inventory
    /// </summary>
    /// <param name="_newItem">picked up item</param>
    public void SpawnInventoryItems(Transform _newItem)
    {
        // if item is already spawned change the item amount
        if (_newItem != null)
        {
            Item item = null;
            item = _newItem.GetComponent<PhysicalItem>().item;
            var destroyItem = _newItem.GetComponent<PhysicalItem>();
            var contains = Contains(item);

            if (!contains)
            {
                Spawn(item);
                item.Amount = 1;
                StartCoroutine(destroyItem.DestroyObject());
            }

            else
            {
                IncreaseAmount(item);
                StartCoroutine(destroyItem.DestroyObject());
            }  
        }
        
        /*// spawn prefab then set its item
        foreach (Item setItem in inventory)
        {
            var obj = Instantiate(prefab, contentPosition).GetComponent<ItemDisplay>();
            obj.item = setItem;
            inventoryItems.Add(obj.transform);
        }*/
    }
    public void DisableMenu() => menu.SetActive(false);    
    /// <summary>
    /// Checks if any item in inventory item matches the "sent" item
    /// then returns
    /// </summary>
    private bool Contains(Item _item) => inventoryItems.Any(t => _item.Name == t.GetComponent<ItemDisplay>().item.Name);
    /// <summary>
    /// Finds the matching item and increments the amount and refreshes the display
    /// </summary>
    /// <param name="_item"></param>
    private void IncreaseAmount(Item _item)
    {
        foreach (var t in inventoryItems.Where(t => _item.Name == t.GetComponent<ItemDisplay>().item.Name)) 
            t.GetComponent<ItemDisplay>().item.Amount++;
    }
    /// <summary>
    /// Spawns a prefab and sets adds the selected item to the list
    /// </summary>
    /// <param name="_setItem"></param>
    private void Spawn(Item _setItem)
    {
        var obj = Instantiate(prefab, contentPosition).GetComponent<ItemDisplay>();
        obj.item = _setItem;
        obj.toolTip = toolTip;
        obj.menu = menu;
        inventoryItems.Add(obj.transform); 
    }
    #endregion
    #region DropItems
    public void DropItem(Item _item)
    {
        // loops until it finds the matching item
        foreach (var t in inventoryItems.Where(t => _item.Name == t.GetComponent<ItemDisplay>().item.name))
        {
            if (t.GetComponent<ItemDisplay>().item.Amount > 1)
            {
                t.GetComponent<ItemDisplay>().item.Amount--;
                SpawnDroppedItem(_item);
            }
            else
            {
                SpawnDroppedItem(_item);
            }

            break;
        }
    }

    private void SpawnDroppedItem(Item _droppedItem)
    {
        var _obj = TextureUtils.LoadMeshResource("ItemGameObjectSprite");
        Instantiate(_obj, player.forward * 2, Quaternion.identity);
        _obj.GetComponent<PhysicalItem>().item = _droppedItem;  
    }
    #endregion
    
    /*#region IMGUI
    private void OnGUI()
    {
        if (showInventory)
        {
            GUI.Box(new Rect(0, 0, Screen.width, Screen.height), "");

            List<string> itemTypes = new List<string>(Enum.GetNames(typeof(Item.ItemType)));
            itemTypes.Insert(0, "All");

            for (int i = 0; i < itemTypes.Count; i++)
            {
                if (GUI.Button(new Rect(
                    (Screen.width / itemTypes.Count) * i
                    , 10
                    , Screen.width / itemTypes.Count
                    , 20), itemTypes[i]))
                {
                    sortType = itemTypes[i];
                }
            }
            Display();

            if (selectedItem != null)
            {
                DisplaySeletectedItem();
            }
        }
    }
    private void DisplaySeletectedItem()
    {
        GUI.Box(new Rect(Screen.width / 4, Screen.height / 3,
            Screen.width / 5, Screen.height / 5),
            selectedItem.Icon);

        GUI.Box(new Rect(new Rect(Screen.width / 4, (Screen.height / 3) + (Screen.height / 5),
            Screen.width / 7, Screen.height / 15)), selectedItem.Name);

        GUI.Box(new Rect(new Rect(Screen.width / 4, (Screen.height / 3) + (Screen.height / 3),
            Screen.width / 5, Screen.height / 5)), selectedItem.Description + 
            "\nValue: " + selectedItem.Value + 
            "\nAmount: " + selectedItem.Amount);
    }
    private void Display()
    {
        scrollPosition = GUI.BeginScrollView(new Rect(0, 40, Screen.width, Screen.height - 40),
        scrollPosition,
        new Rect(0, 0, 0, inventory.Count * 30),
        false,
        true);

        int count = 0;
        for (int i = 0; i < inventory.Count; i++)
        {
            if (inventory[i].Type.ToString() == sortType || sortType == "All")
            {
                if (GUI.Button(new Rect(30, 0 + (count * 30), 200, 30), inventory[i].Name))
                {
                    selectedItem = inventory[i];
                }
                count++;
            }
        }
        GUI.EndScrollView();
    }
    #endregion*/
}

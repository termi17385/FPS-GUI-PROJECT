using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using FPSProject.Player;
using FPSProject.Player.Manager;
using FPSProject.Utils;
using FPSProject.Weapons;
using UnityEditor.iOS;

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

    [SerializeField] private Transform[] equipSlots;
    public List<GameObject> equipedItems = new List<GameObject>();
    //[SerializeField] private bool showInventory = true;

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

            if (!contains && item.Type != Item.ItemType.Weapon)
            {
                Spawn(item);
                item.Amount = 1;
                StartCoroutine(destroyItem.DestroyObject());
            }

            else
            {
                if (item.Type != Item.ItemType.Weapon) IncreaseAmount(item);
                else { Spawn(item); item.Amount = 1; }
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
    public void DisableMenu()
    {
        menu.SetActive(false);
        toolTip.gameObject.SetActive(false);
    }
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
    #region Using and Equiping Items
    public void DropItem(Item _item)
    {
        Debug.LogError(_item);
        if (_item.Amount > 1)
        {
            _item.Amount--;
            SpawnDroppedItem(_item);
        }
        else SpawnDroppedItem(_item, true);
    }
    public void ConsumeItem(Item _item)
    {
        if (_item.Amount > 1)
        {
            _item.Amount--;
            Switch(_item);           
        }
        else if (_item.Amount <= 1)
        {
           Switch(_item);
           DeleteItem(_item);
        } 
    }
    private void Switch(Item _item)
    {
        switch (_item.Type)
        {
            case Item.ItemType.Food:
                player.GetComponent<PlayerManager>().Health += 10;
                player.GetComponent<PlayerManager>().Stamina += 10;
                break;
            case Item.ItemType.Potions:
                player.GetComponent<PlayerManager>().Health += 50;
                break;
        }
    }
    private void SpawnDroppedItem(Item _droppedItem, bool _delete = false)
    {
        GameObject _obj = TextureUtils.LoadGameObjectResource("MeshItem");
        if (_droppedItem.Type == Item.ItemType.Food || _droppedItem.Type == Item.ItemType.Potions) _obj = TextureUtils.LoadGameObjectResource("ItemGameObjectSprite"); 

        _obj.GetComponent<PhysicalItem>().item = _droppedItem;  
        Instantiate(_obj, transform.position, Quaternion.identity);
        if(_delete) DeleteItem(_droppedItem);
    }
    private void DeleteItem(Item _item)
    {
        for (int i = inventoryItems.Count - 1; i > -1; i--)
        {
            if (inventoryItems[i].GetComponent<ItemDisplay>().item.Name != _item.Name) continue;
            GameObject destroyThis = inventoryItems[i].gameObject;
            inventoryItems.RemoveAt(i);
            Destroy(destroyThis);
            break;
        }
    }
    public void EquipItem(GameObject _item)
    {
        Transform obj = null;
        foreach (var t in inventoryItems.Where(t => _item.transform == t)) obj = t;

        if (_item.GetComponent<ItemDisplay>().item.Type == Item.ItemType.Apparel)
        {
            obj.SetParent(equipSlots[0].transform);
            obj.transform.localPosition = new Vector3(0, 0, 0);
            equipedItems.Add(obj.gameObject);
            SpawnEquippedItemOnPlayer(_item.GetComponent<ItemDisplay>().item);
        }
        else if (_item.GetComponent<ItemDisplay>().secondary)
        {
            obj.SetParent(equipSlots[2].transform);
            obj.transform.localPosition = new Vector3(0, 0, 0);
            equipedItems.Add(obj.gameObject);
            SpawnEquippedItemOnPlayer(obj.GetComponent<ItemDisplay>().item, true);
        }
        else
        {
            obj.SetParent(equipSlots[1].transform);
            obj.transform.localPosition = new Vector3(0, 0, 0);
            equipedItems.Add(obj.gameObject);
            SpawnEquippedItemOnPlayer(obj.GetComponent<ItemDisplay>().item);
        }
    }
    private void SpawnEquippedItemOnPlayer(Item _item, bool _secondary = false)
    {
        var _player = player.GetComponent<PlayerController>();
        switch (_item.Type)
        {
            case Item.ItemType.Apparel:
                player.GetComponent<PlayerManager>().hatMesh.mesh = _item.Mesh;
                break;
            case Item.ItemType.Weapon when _secondary:
                _player.meshes[1].mesh = _item.Mesh;
                _player.meshes[1].gameObject.GetComponent<PlasmaRifle>().enabled = true;
                break;
            case Item.ItemType.Weapon:
                _player.meshes[0].mesh = _item.Mesh;
                _player.meshes[0].gameObject.GetComponent<PlasmaRifle>().enabled = true;
                break;
        }
        var obj = TextureUtils.LoadGameObjectResource(_item.Name);
    }

    private void DespawnEquippedItemOnPlayer(Item _item)
    {
        var _player = player.GetComponent<PlayerController>();
        switch (_item.Type)
        {
            case Item.ItemType.Apparel:
                player.gameObject.GetComponent<PlayerManager>().hatMesh.mesh = null;
                break;
            case Item.ItemType.Weapon when _item.Name == "LasPistol":
            {
                _player.meshes[1].gameObject.GetComponent<PlasmaRifle>().enabled = false;
                _player.meshes[1].mesh = null;
                break;
            }
            case Item.ItemType.Weapon:
            {
                _player.meshes[0].gameObject.GetComponent<PlasmaRifle>().enabled = false;
                _player.meshes[0].mesh = null;
                break;
            }
        }
    }
    public void UnEquipItem(GameObject _item)
    {
        _item.transform.SetParent(contentPosition);
        var despawn = _item.GetComponent<ItemDisplay>().item;
        equipedItems.Remove(_item);

        if (despawn.Type == Item.ItemType.Apparel) DespawnEquippedItemOnPlayer(despawn);
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

using System.Linq;
using UnityEngine;

public class MenuSystem : MonoBehaviour
{
      public Item item;
      public GameObject inventoryObject;
      [SerializeField] private Inventory inv;
      [SerializeField] private GameObject[] menuStuff;

      private void Update()
      {
            if (item.Type == Item.ItemType.Weapon || item.Type == Item.ItemType.Apparel)
            {
                  menuStuff[0].SetActive(true);
                  menuStuff[1].SetActive(false);
                  menuStuff[2].SetActive(false);
            }
            else
            {
                  menuStuff[2].SetActive(false);
                  menuStuff[1].SetActive(true);
                  menuStuff[0].SetActive(false);
            }
            if (inv.equipedItems.Contains(inventoryObject))
            {
                  menuStuff[2].SetActive(true);
                  menuStuff[1].SetActive(false);
                  menuStuff[0].SetActive(false);  
            }
      }

      public void DropItem() => inv.DropItem(item);
      public void ConsumeItem() => inv.ConsumeItem(item);
      public void EquipItem() => inv.EquipItem(inventoryObject);
      public void UnEquipItem() => inv.UnEquipItem(inventoryObject);
}

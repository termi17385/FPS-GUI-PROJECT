using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSystem : MonoBehaviour
{
      public Item item;
      [SerializeField] private Inventory inv;

      public void DropItem() => inv.DropItem(item);  
}

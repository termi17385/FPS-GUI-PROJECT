using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;

public class InventoryMenuManager : SerializedMonoBehaviour
{
    [SerializeField] private GameObject[] menus = new GameObject[4];

    public void OnClick(int id)
    {
        switch (id)
        {
            case 0:     // Inventory
                menus[0].SetActive(true);
                menus[1].SetActive(false);
                menus[2].SetActive(false);
                menus[3].SetActive(false);
                break;             
                                   
            case 1:     // Quests      
                menus[0].SetActive(false);
                menus[1].SetActive(true);
                menus[2].SetActive(false);
                menus[3].SetActive(false);
                break;             
                                   
            case 2:     // Stats       
                menus[0].SetActive(false);
                menus[1].SetActive(false);
                menus[2].SetActive(true);
                menus[3].SetActive(false);
                break;             
                                   
            case 3:     // Description 
                menus[0].SetActive(false);
                menus[1].SetActive(false);
                menus[2].SetActive(false);
                menus[3].SetActive(true);
                break;
        }
    }
}

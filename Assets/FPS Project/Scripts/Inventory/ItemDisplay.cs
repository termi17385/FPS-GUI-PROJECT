using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemDisplay : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public bool secondary = false;
    [System.NonSerialized] public Item item;
    [System.NonSerialized] public ToolTip toolTip;
    [System.NonSerialized] public GameObject menu;
    
    [SerializeField] private Transform menuPosition;

    private RawImage itemDisplay;
    private TextMeshProUGUI itemName;
    private TextMeshProUGUI itemAmount;


    private void Awake()
    {
        itemDisplay = transform.Find("ItemImage").GetComponent<RawImage>();
        itemName = transform.Find("Name").GetComponent<TextMeshProUGUI>();
        itemAmount = transform.Find("Amount").GetComponent<TextMeshProUGUI>();  
    }
    private void Start() => secondary = item.Name.Contains("Pistol");
    private void LateUpdate() => DisplaysMinimalValue();
    
    /// <summary>
    /// Displays all the values that are shown on the front "card"
    /// <br/> instead of the tooltip
    /// </summary>
    private void DisplaysMinimalValue()
    {
        itemDisplay.texture = item.Icon;
        itemName.text = item.Name;
        itemAmount.text = $"x{item.Amount}";
    }
    private void ToggleMenu(bool _toggle)
    {
        menu.SetActive(_toggle);
        menu.transform.position = menuPosition.position;
    } 
     
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (menu.activeSelf) return;
        
        toolTip.gameObject.SetActive(true);
        toolTip.name.text = item.Name;
            
        menu.GetComponent<MenuSystem>().item = item;
        toolTip.description.text = toolTip.debug
            ? $"{item.Description}\nValue: {item.Value} \nHeals \n{menu.GetComponent<MenuSystem>().item}"
            : $"{item.Description}\nValue: {item.Value} \nHeals";
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if(!menu.activeSelf) toolTip.gameObject.SetActive(false);
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        menu.GetComponent<MenuSystem>().item = item;
        menu.GetComponent<MenuSystem>().inventoryObject = this.gameObject;
        ToggleMenu(true);
    } 
}

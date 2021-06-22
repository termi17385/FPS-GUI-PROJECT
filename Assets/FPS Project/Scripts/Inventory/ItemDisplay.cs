using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemDisplay : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public Item item;
    public ToolTip toolTip;
    public GameObject menu;
    
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
    private void LateUpdate() => DisplaysMinimalValue();
    /// <summary>
    /// Displays all the values that are shown on the front "card"
    /// <br/> instead of the tooltip
    /// </summary>
    public void DisplaysMinimalValue()
    {
        itemDisplay.texture = item.Icon;
        itemName.text = item.Name;
        itemAmount.text = $"x{item.Amount}";
    }

    public void ToggleMenu(bool _toggle)
    {
        menu.SetActive(_toggle);
        menu.transform.position = menuPosition.position;
    } 
        
    public void OnPointerEnter(PointerEventData eventData)
    {
        toolTip.gameObject.SetActive(true);
        toolTip.name.text = item.Name;
        toolTip.description.text = $"{item.Description}\nValue: {item.Value} \nHeals";
    }
    public void OnPointerExit(PointerEventData eventData) => toolTip.gameObject.SetActive(false);

    public void OnPointerClick(PointerEventData eventData)
    {
        menu.GetComponent<MenuSystem>().item = item;
        ToggleMenu(true);
    } 
}

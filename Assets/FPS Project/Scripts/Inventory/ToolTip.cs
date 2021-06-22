using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ToolTip : MonoBehaviour
{
   public TextMeshProUGUI name;
   public TextMeshProUGUI description;
   
   [SerializeField] private RectTransform parent;
   [SerializeField] private Vector2 offset;
   
   private void Start()
   {
      name = transform.Find("Name").GetComponent<TextMeshProUGUI>();
      description = transform.Find("Description").GetComponent<TextMeshProUGUI>();
      parent = GetComponentInParent<Canvas>().GetComponent<RectTransform>();
   }

   private void Update()
   {
      FollowCursor();
   }

   private void FollowCursor()
   {
      Vector2 localPoint;
      // gets the rect of the parent canvas then finds the mouse position relative to the canvas and outputs the local point
      // camera should be set to null if the canvas is set to screen space overlay - unity documentation 2021 https://docs.unity3d.com/ScriptReference/RectTransformUtility.ScreenPointToLocalPointInRectangle.html
      RectTransformUtility.ScreenPointToLocalPointInRectangle(parent, Input.mousePosition, null, out localPoint);
      transform.localPosition = localPoint + offset; 
   }
}

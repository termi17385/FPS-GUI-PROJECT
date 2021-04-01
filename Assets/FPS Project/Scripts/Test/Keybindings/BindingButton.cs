using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BindingButton : MonoBehaviour
{
    [SerializeField] private string bindingToMap;
    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI buttonText;
    [SerializeField] private TextMeshProUGUI bindingName;

    private bool isRebinding = false;

    public void Setup(string _toMap)
    {
        bindingToMap = _toMap;

        // automatically set the onclick function and change the name text to the binding
        button.onClick.AddListener(OnClick);
        bindingName.text = _toMap;

        // update the buttontext with the bindings value and make the GO activate
        BindingUtils.UpdateTextWithBinding(bindingToMap, buttonText);
        gameObject.SetActive(true);
    }

    private void Start()
    {
        if(string.IsNullOrEmpty(bindingToMap))
        {
            gameObject.SetActive(false);
            return;
        }

        Setup(bindingToMap);
    }

    private void Update()
    {
        if (isRebinding)
        {
            KeyCode pressed = BindingUtils.GetAnyPressedKey();
            if (pressed != KeyCode.None)
            {   
                BindingManager.Rebind(bindingToMap, pressed);
                BindingUtils.UpdateTextWithBinding(bindingToMap, buttonText);

                isRebinding = false;
            }
        }
    }

    private void OnClick()
    {
        isRebinding = true;
    }
}

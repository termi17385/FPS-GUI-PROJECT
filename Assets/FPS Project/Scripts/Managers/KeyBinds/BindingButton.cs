using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace FPSProject.Keybinds
{
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

            button.onClick.AddListener(OnClick);
            bindingName.text = _toMap;

            BindingUtils.UpdateTextWithBinding(bindingToMap, buttonText);
            gameObject.SetActive(true);
        }

        private void Start()
        {
            if (string.IsNullOrEmpty(bindingToMap))
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
}


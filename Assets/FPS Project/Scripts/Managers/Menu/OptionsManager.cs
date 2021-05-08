using System.Collections.Generic;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;

namespace FPSProject.Menu
{
    [HideMonoScript]
    public class OptionsManager : SerializedMonoBehaviour
    {   
        public GameObject[] optionMenus;
        public Button[] buttons;

        // Start is called before the first frame update
        void Awake()
        {
            buttons[0].interactable = false;
            for(int i = 1; i < optionMenus.Length; i++)
            {
                optionMenus[0].SetActive(true);
                optionMenus[i].SetActive(false);
            }
        }

        public void OpenAudio()
        {
            if(buttons[0].interactable == true)
            {
                optionMenus[0].SetActive(true); 
                optionMenus[1].SetActive(false);
                optionMenus[2].SetActive(false);
                optionMenus[3].SetActive(false);

                buttons[0].interactable = false;
                buttons[1].interactable = true;
                buttons[2].interactable = true;
                buttons[3].interactable = true;
            }
        }

        public void OpenDisplay()
        {
            if(buttons[1].interactable == true)
            {
                optionMenus[0].SetActive(false);
                optionMenus[1].SetActive(true);
                optionMenus[2].SetActive(false);
                optionMenus[3].SetActive(false);

                buttons[0].interactable = true;
                buttons[1].interactable = false;
                buttons[2].interactable = true;
                buttons[3].interactable = true;
            }
        }

        public void OpenGraphics()
        {
            if(buttons[2].interactable == true)
            {
                optionMenus[0].SetActive(false);
                optionMenus[1].SetActive(false);
                optionMenus[2].SetActive(true);
                optionMenus[3].SetActive(false);

                buttons[0].interactable = true;
                buttons[1].interactable = true;
                buttons[2].interactable = false;
                buttons[3].interactable = true;
            }
        }

        public void OpenKeybinds()
        {
            if(buttons[3].interactable == true)
            {
                optionMenus[0].SetActive(false);
                optionMenus[1].SetActive(false);
                optionMenus[2].SetActive(false);
                optionMenus[3].SetActive(true);

                buttons[0].interactable = true;
                buttons[1].interactable = true;
                buttons[2].interactable = true;
                buttons[3].interactable = false;
            }
        }
    }
}


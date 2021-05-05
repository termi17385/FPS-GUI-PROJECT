using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;

namespace FPSProject.NPC.Dialogue
{
    public class DialogueManager : SerializedMonoBehaviour
    {  
        public static DialogueManager dM;
        private Dialogue loadedDialogue;

        [SerializeField] GameObject buttonPrefab;
        [SerializeField] Transform dialogueButtonPanel;

        private void Awake()
        {
            if(dM == null) dM = this;
            else Destroy(this);
        }

        public void LoadDialogue(Dialogue dialogue)
        {
            transform.GetChild(0).gameObject.SetActive(true);
                loadedDialogue = dialogue;
                    ClearButtons();

            Button spawnedButton;
            foreach(LineOfDialogue data in dialogue.dialogueOptions)
            {
                 spawnedButton = Instantiate(buttonPrefab, dialogueButtonPanel).GetComponent<Button>();
                    spawnedButton.GetComponentInChildren<TextMeshProUGUI>().text = data.question;
                        spawnedButton.onClick.AddListener(delegate{ButtonPressed(data.buttonID);});
            }

             spawnedButton = Instantiate(buttonPrefab, dialogueButtonPanel).GetComponent<Button>();
                    spawnedButton.GetComponentInChildren<TextMeshProUGUI>().text = dialogue.goodBye.question;
                        spawnedButton.onClick.AddListener(EndConversation);

            print(dialogue.greeting);
        }

        private void EndConversation()
        {
            ClearButtons();
            transform.GetChild(0).gameObject.SetActive(false);

            print(loadedDialogue.goodBye.response);
        }

        private void ButtonPressed(int index)
        {
            print(loadedDialogue.dialogueOptions[index].response);
        }

        private void ClearButtons()
        {
            foreach(Transform child in dialogueButtonPanel)
            {
                Destroy(child.gameObject);
            }
        }
    }
}
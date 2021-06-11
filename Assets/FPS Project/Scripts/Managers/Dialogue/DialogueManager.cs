using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;
using FPSProject.Menu;

namespace FPSProject.NPC.Dialogue
{
    public class DialogueManager : SerializedMonoBehaviour
    {  
        public static DialogueManager dM;
        private Dialogue loadedDialogue;
        [SerializeField] private TextMeshProUGUI responseText;

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
            int i = 0;
            foreach(LineOfDialogue data in dialogue.dialogueOptions)
            {
                float? currentApproval = FactionManager.instance.FactionsApproval(loadedDialogue.faction);
                if(currentApproval != null && currentApproval > data.minAproval)
                {
                    spawnedButton = Instantiate(buttonPrefab, dialogueButtonPanel).GetComponent<Button>();
                    spawnedButton.GetComponentInChildren<TextMeshProUGUI>().text = data.question;
                    data.buttonID = i;
                    spawnedButton.onClick.AddListener(() => ButtonPressed(data.buttonID));
                    i++;
                }
            }

            spawnedButton = Instantiate(buttonPrefab, dialogueButtonPanel).GetComponent<Button>();
            spawnedButton.GetComponentInChildren<TextMeshProUGUI>().text = dialogue.goodBye.question;
            spawnedButton.onClick.AddListener(EndConversation);

            print(dialogue.greeting);
            DisplayResponse(loadedDialogue.greeting);
        }

        public void EndConversation()
        {
            ClearButtons();
            DisplayResponse(loadedDialogue.goodBye.response);
            if(loadedDialogue.goodBye.nextDialogue != null)
            {
                LoadDialogue(loadedDialogue.goodBye.nextDialogue);
            }
            else
            {
                if(PauseMenu.instance != null) 
                {
                    PauseMenu.instance.paused = false;
                    Cursor.lockState = CursorLockMode.None;
                }
                transform.GetChild(0).gameObject.SetActive(false);
            }

        }

        private void ButtonPressed(int index)
        {
            if(loadedDialogue.dialogueOptions[index].nextDialogue != null)
            {
                LoadDialogue(loadedDialogue.dialogueOptions[index].nextDialogue);
            }
            else
            {
                DisplayResponse(loadedDialogue.dialogueOptions[index].response);
                
                if(loadedDialogue.quest == true)
                {
                    var questMan = QuestManager.instance;
                    questMan.AcceptQuest(loadedDialogue.dialogueOptions[index].quest);
                    loadedDialogue.RunQuestEvent.Invoke();
                }
            }
        }

        private void ClearButtons()
        {
            foreach(Transform child in dialogueButtonPanel)
            {
                Destroy(child.gameObject);
            }
        }

        private void DisplayResponse(string response)
        {
            responseText.text = response;
        }
    }
}
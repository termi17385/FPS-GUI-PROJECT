using System.Collections.Generic;
using UnityEngine.Events;
using Sirenix.OdinInspector;

namespace FPSProject.NPC.Dialogue
{
    public class Dialogue : SerializedMonoBehaviour
    {
        public string faction; // what faction the npc belongs too
        public string greeting;
        public LineOfDialogue goodBye;
        public List<LineOfDialogue> dialogueOptions = new List<LineOfDialogue>();
        public UnityEvent RunQuestEvent;
        public bool firstDialogue;
        public bool quest;

        public void Update()
        {
            if(!firstDialogue) return;
            //if (Input.GetKeyDown(KeyCode.E))
            //{
            //    DialogueManager.dM.LoadDialogue(this);
            //}
        }

        public void ClearQuest()
        {
            quest = false;
            dialogueOptions.Clear();
            greeting = "Sorry nothing avaliable today";
        }
    }
}

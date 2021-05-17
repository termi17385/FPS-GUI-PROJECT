using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;

namespace FPSProject.NPC.Dialogue
{
    public class Dialogue : SerializedMonoBehaviour
    {
        public string faction; // what faction the npc belongs too
        public string greeting;
        public LineOfDialogue goodBye;
        public LineOfDialogue[] dialogueOptions;
        public bool firstDialogue;

        public void Update()
        {
            if(!firstDialogue) return;
            if (Input.GetKeyDown(KeyCode.E))
            {
                DialogueManager.dM.LoadDialogue(this);
            }
        }
    }
}

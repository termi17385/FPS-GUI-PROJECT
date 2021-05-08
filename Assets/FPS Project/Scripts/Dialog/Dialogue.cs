using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;

namespace FPSProject.NPC.Dialogue
{
    public class Dialogue : SerializedMonoBehaviour
    {
        public string greeting;
        public LineOfDialogue goodBye;
        public LineOfDialogue[] dialogueOptions;

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                DialogueManager.dM.LoadDialogue(this);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace FPSProject.NPC.Dialogue
{
    public class Dialogue : SerializedMonoBehaviour
    {
        public string greeting;
        public LineOfDialogue goodBye;
        public LineOfDialogue[] dialogueOptions;
    }
}

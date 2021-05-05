using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace FPSProject.NPC.Dialogue
{
    [System.Serializable]
    public class LineOfDialogue
    {
        [TextArea(4,6), FoldoutGroup("DialogueStuff")]
        public string question, response;
        public int buttonID;
    }
}

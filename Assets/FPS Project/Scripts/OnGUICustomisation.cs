using System.Collections.Generic;
using UnityEngine;
using FPSProject.Customisation;

// This scripts only purpose is for debugging 
public class OnGUICustomisation : MonoBehaviour
{
    #region Variables
    [System.Serializable]
    public struct GUILayout
    {
        public Vector2 position;
        public Vector2 scale;
    }

    public List<GUILayout> gui = new List<GUILayout>();

    [SerializeField] bool runInEditor;
    bool dropDown;
    bool dropDown2;

    Vector2 scroll;
    Vector2 scroll2;

    string[] classNames = { "Stealth", "Tank", "Hunter", "SprintyBoi", "Mage" };
    string[] raceNames = {"darkElf", "woodElf", "Human", "dwarf", "lizardMan"};

    [SerializeField] PlayerCustomisation pC;
    [SerializeField] PlayerStatsCustomisation pSC;
    [SerializeField] CustomisationManager cManager;
    #endregion
    #region Method
    /// <summary>
    /// Temporay GUI used for customising the characters look
    /// </summary>
    private void OnGUI()
    {
        if (runInEditor)
        {
            #region Matrix
            Vector2 res = new Vector2(1920, 1080);
            GUI.matrix = IMGUIUtils.IMGUIMatrix(res);
            #endregion
            #region Style
            GUIStyle style = new GUIStyle();
            style.alignment = TextAnchor.MiddleCenter;
            style.fontSize = 20;
            style.normal.textColor = Color.white;
            #endregion
            #region Rect
            Rect groupOne = new Rect(gui[0].position.x, gui[0].position.y, gui[0].scale.x, gui[0].scale.y);
            Rect groupTwo = new Rect(gui[1].position.x, gui[1].position.y, gui[1].scale.x, gui[1].scale.y);
            Rect groupThree = new Rect(gui[7].position.x, gui[7].position.y, gui[7].scale.x, gui[7].scale.y);
            Rect groupFour = new Rect(gui[10].position.x, gui[10].position.y, gui[10].scale.x, gui[10].scale.y);
            Rect groupFive = new Rect(gui[12].position.x, gui[12].position.y, gui[12].scale.x, gui[12].scale.y);

            Rect TexturesGroup = new Rect(gui[4].position.x, gui[4].position.y, gui[4].scale.x, gui[4].scale.y);
            Rect StatsGroup = new Rect(gui[6].position.x, gui[6].position.y, gui[6].scale.x, gui[6].scale.y);

            Rect boxOne = new Rect(0,0, gui[0].scale.x, gui[0].scale.y);
            Rect boxTwo = new Rect(0,0, gui[1].scale.x, gui[1].scale.y);
            
            Rect button = new Rect(gui[9].position.x, gui[9].position.y, gui[9].scale.x, gui[9].scale.y);
            
            #region vectors 
            Vector2 position = gui[2].position;
            Vector2 scale = gui[2].scale;
           
            Vector2 position2= gui[3].position;
            Vector2 scale2 = gui[3].scale;

            Vector2 position3 = gui[5].position;
            Vector2 scale3 = gui[5].scale;
            #endregion
            #endregion

            #region Group 1
            GUI.BeginGroup(groupOne);
            GUI.Box(boxOne, "");
            GUI.BeginGroup(TexturesGroup);
            for (int i = 0; i < pC.textures.Length; i++)
            {
                Rect boxTexture = new Rect(position3.x, (position3.y * i), scale3.x, scale3.y);
                GUI.Box(boxTexture, "");
                GUI.Box(boxTexture, pC.matName[i], style);
                if (GUI.Button(new Rect(position.x, (position.y * i), scale.x, scale.y), ">"))
                {
                    pC.SetTexture(i, 1);
                }
                if (GUI.Button(new Rect(position2.x, (position2.y * i), scale2.x, scale2.y), "<"))
                {
                    pC.SetTexture(i, -1);
                }
            }
            GUI.EndGroup();
            GUI.EndGroup();
            #endregion
            #region Group 2
            GUI.BeginGroup(groupTwo);
            GUI.Box(boxTwo,"");
            GUI.BeginGroup(StatsGroup);
            for(int i = 0; i < pSC.characterStats.Length; i++)
            {
                PlayerStatsCustomisation.Stats _stats = pSC.characterStats[i]; 
                string boxText = string.Format("{0}: {1}", _stats.baseStatsName, (_stats.baseStats + _stats.tempStats));
                Rect boxTexture = new Rect(position3.x, (position3.y * i), scale3.x, scale3.y);

                GUI.Box(boxTexture, "");
                GUI.Box(boxTexture, boxText, style);
                
                if (GUI.Button(new Rect(position.x, (position.y * i), scale.x, scale.y), "+"))
                {
                    if(!pSC.allStatsAssigned)
                    pSC.AssignStats(1, i);
                }
                
                if (GUI.Button(new Rect(position2.x, (position2.y * i), scale2.x, scale2.y), "-"))
                {
                    pSC.AssignStats(-1, i);
                }
            }
            GUI.EndGroup();
            GUI.EndGroup();
            #endregion
            #region Group 3
            GUI.Box(groupThree, "");
            GUI.BeginGroup(groupThree);
            pSC.characterName = GUI.TextField(new Rect(gui[8].position.x, gui[8].position.y, gui[8].scale.x, gui[8].scale.y), pSC.characterName);
            if(GUI.Button(button, "Save"))
            {
                cManager.Save();
            }
            GUI.EndGroup();
            #endregion
            #region Group 4
            GUI.Box(groupFour, "");
            GUI.BeginGroup(groupFour);
            if(GUI.Button(new Rect(0,0,150,50), pSC.characterClass.ToString()))
            {
                dropDown = !dropDown;
            }
            if (dropDown)
            {
                for (int i = 0; i < classNames.Length; i++)
                {
                    Rect _button = new Rect(gui[11].position.x, gui[11].position.y * i, gui[11].scale.x, gui[11].scale.y);
                    scroll = GUI.BeginScrollView(new Rect(0, 50, 150, gui[10].scale.y - 50), scroll, new Rect(0, 0, 0, gui[10].scale.y + 50), false, true);
                    if(GUI.Button(_button, classNames[i]))
                        pSC.ChooseClass(i);
                    GUI.EndScrollView();
                }
            }
            GUI.EndGroup();
            #endregion
            #region Group 5
            GUI.Box(groupFive, "");
            GUI.BeginGroup(groupFive);
            if (GUI.Button(new Rect(0, 0, 150, 50), pSC.race.ToString()))
            {
                dropDown2 = !dropDown2;
            }
            if (dropDown2)
            {
                for (int i = 0; i < raceNames.Length; i++)
                {
                    Rect _button = new Rect(gui[11].position.x, gui[11].position.y * i, gui[11].scale.x, gui[11].scale.y);
                    scroll2 = GUI.BeginScrollView(new Rect(0, 50, 150, gui[10].scale.y - 50), scroll2, new Rect(0, 0, 0, gui[10].scale.y + 50), false, true);
                    if(GUI.Button(_button, raceNames[i])) 
                        pSC.ChooseRace(i);
                    GUI.EndScrollView();
                }
            }
            GUI.EndGroup();
            #endregion
        }
    }
    #endregion
}

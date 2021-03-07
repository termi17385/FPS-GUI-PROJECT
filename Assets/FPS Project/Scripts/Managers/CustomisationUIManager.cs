using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

namespace FPSProject.Customisation
{
    public class CustomisationUIManager : MonoBehaviour
    {
        #region Variables
        [SerializeField]
        PlayerCustomisation pc;

        [SerializeField] private TextMeshProUGUI displayStartingPoints;
        [SerializeField] private TextMeshProUGUI displayStatPoints;

        [SerializeField] private int x;

        #endregion
        private void Awake()
        {
            pc = FindObjectOfType<PlayerCustomisation>();
            DisplayText();

            pc.ChooseClass( 0);
        }

        private void Update()
        {
            DisplayText();
            EnableDebug();
        }

        private void EnableDebug()
        {   
            if (Input.GetKeyDown(KeyCode.F1))
            {
                x ++;
                switch (x)
                {
                    case 1:
                        pc.debugMode = true;
                        break;
                    case 2:
                        pc.debugMode = false;
                        x = 0;
                        break;
                }
            }
        }


        private void DisplayText()
        {
            #region variables
            int charisma =      pc.characterStats[0].baseStats + pc.characterStats[0].tempStats;    // holds the data for charisma
            int intelligence =  pc.characterStats[1].baseStats + pc.characterStats[1].tempStats;    // holds the data for intelligence
            int strength =      pc.characterStats[2].baseStats + pc.characterStats[2].tempStats;    // holds the data for strength
            int dexterity =     pc.characterStats[3].baseStats + pc.characterStats[3].tempStats;    // holds the data for dexterity
            int constitution =  pc.characterStats[4].baseStats + pc.characterStats[4].tempStats;    // holds the data for constitution
            int agility =       pc.characterStats[5].baseStats + pc.characterStats[5].tempStats;    // holds the data for agility
            #endregion

            displayStartingPoints.text = string.Format("Points : {0}", pc.statPoints);
            displayStatPoints.text = string.Format("charisma: {0}\n\nintelligence: {1}\n\nstrength: {2}\n\ndexterity: {3}\n\nconstitution: {4}\n\nagility: {5}",
                charisma, intelligence, strength, dexterity, constitution, agility);
        }

        public void CharacterClassDropDown(int i)
        {
            pc.ChooseClass(i);
            DisplayText();
        }

        public void SetTexturePos(string buttonID)
        {
            pc.SetTexture(buttonID, 1); 
        }

        public void SetTextureNeg(string buttonID)
        {
            pc.SetTexture(buttonID, -1);
        }
    }
}


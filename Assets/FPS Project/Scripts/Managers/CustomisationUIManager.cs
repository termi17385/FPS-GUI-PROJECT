using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

        #endregion
        private void Awake()
        {
            pc = FindObjectOfType<PlayerCustomisation>();
            DisplayText();
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
            displayStatPoints.text = string.Format("{0}\n{1}\n{2}\n{3}\n{4}\n{5}", charisma, intelligence, strength, dexterity, constitution, agility);
        }

        public void CharacterClassDropDown(int i)
        {
            pc.ChooseClass(i);
            DisplayText();
        }
    }
}


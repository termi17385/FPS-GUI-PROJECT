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
        }
 
        /// <summary>
        /// used for displaying text to the canvas UI elements
        /// </summary>
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

            #region Displaying Stats
            displayStartingPoints.text = string.Format("Points : {0}", pc.statPoints);
            displayStatPoints.text = string.Format("charisma: {0}\n\nintelligence: {1}\n\nstrength: {2}\n\ndexterity: {3}\n\nconstitution: {4}\n\nagility: {5}",
            charisma, intelligence, strength, dexterity, constitution, agility);
            #endregion
        }

        /// <summary>
        /// a dropdown for changing the characters class
        /// </summary>
        /// <param name="i">the id of the class in the list</param>
        public void CharacterClassDropDown(int i)
        {
            pc.ChooseClass(i);
        }

        #region Texture Customisation
        public void SetTexturePos(string buttonID)
        {
            pc.SetTexture(buttonID, 1); 
        }

        public void SetTextureNeg(string buttonID)
        {
            pc.SetTexture(buttonID, -1);
        }
        #endregion
    }
}


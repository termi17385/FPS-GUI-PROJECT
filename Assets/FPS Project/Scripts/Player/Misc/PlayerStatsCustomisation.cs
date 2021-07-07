using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace FPSProject.Customisation
{
    public enum CharacterRace
    {
        darkElf,
        woodElf,
        Human,
        dwarf,
        lizardMan
    }
    public enum CharacterClass
    {
        Stealth,
        Tank,
        Hunter,
        SprintyBoi,
        Mage,
    }

    public class PlayerStatsCustomisation : SerializedMonoBehaviour
    {
        #region Variables
        #region Stats
        [System.Serializable]
        public struct Stats
        {
            public string baseStatsName;
            public int baseStats;
            public int tempStats;
        }
        public Stats[] characterStats = new Stats[6];

        public int statPoints;
        public bool allStatsAssigned = false;
        [SerializeField] private Transform prefab;
        [SerializeField] private Transform content;
        [SerializeField] private List<TextMeshProUGUI> pointsText = new List<TextMeshProUGUI>();
        [SerializeField] private TextMeshProUGUI pointText;
        #endregion
        #region Race
        public CharacterRace race = CharacterRace.Human;
        [SerializeField] private TMP_Dropdown rDropdown;
        // debugging index
        public int raceIndex;
        #endregion
        #region Class
        public CharacterClass characterClass = CharacterClass.Hunter;
        [SerializeField] private TMP_Dropdown cDropdown;
        // debugging index
        public int classIndex;
        #endregion
        public string characterName;
        #endregion

        #region Methods
        private void Awake()
        {
            // debugging indexes
            raceIndex = (int)race;
            classIndex = (int)characterClass;
            
            #region Stat Names
            characterStats[0].baseStatsName = "Charisma";
            characterStats[1].baseStatsName = "Speed";
            characterStats[2].baseStatsName = "Strength";
            characterStats[3].baseStatsName = "Defense";
            characterStats[4].baseStatsName = "Stamina";
            characterStats[5].baseStatsName = "Mana";
            #endregion
        }
        private void Start()
        {
            SpawnButtons();
            ChooseClass(classIndex);
            ChooseRace(raceIndex);
            SetDropDowns();
        }

        public void CharacterName(string cn) => characterName = cn;

        /// <summary>
        /// Fills the drop downs with the correct names
        /// </summary>
        private void SetDropDowns()
        {
            rDropdown.ClearOptions();
            cDropdown.ClearOptions();

            List<string> rOptions = new List<string>();
            List<string> cOptions = new List<string>();
            for (int i = 0; i < 5; i++)
            {
                string rName = (race = (CharacterRace) i).ToString();
                string cName = (characterClass = (CharacterClass) i).ToString();
                rOptions.Add(rName);
                cOptions.Add(cName);
            }
            rDropdown.AddOptions(rOptions);
            cDropdown.AddOptions(cOptions);

            rDropdown.value = raceIndex;
            cDropdown.value = classIndex;
        }
        
        /// <summary>
        /// This method handles the players ability to <br/>
        /// Choose their class in the customisation menu
        /// <br/><br/>
        /// As well as assigning the base stats of the class
        /// </summary>
        /// <param name="classIndex">used to choose which class</param>
        public void ChooseClass(int classIndex)
        {
            switch (classIndex)
            {
                #region Stealth
                case 0:
                characterStats[0].baseStats = 11;   // charisma
                characterStats[1].baseStats = 15;   // Intelligence
                characterStats[2].baseStats = 5;    // Strength
                characterStats[3].baseStats = 10;   // Dexterity
                characterStats[4].baseStats = 9;    // Constitution
                characterStats[5].baseStats = 18;   // Agility

                characterClass = CharacterClass.Stealth;
                break;
                #endregion
                #region Tank
                case 1:
                characterStats[0].baseStats = 5;
                characterStats[1].baseStats = 2;
                characterStats[2].baseStats = 16;
                characterStats[3].baseStats = 5;
                characterStats[4].baseStats = 17;
                characterStats[5].baseStats = 1;

                characterClass = CharacterClass.Tank;
                break;
                #endregion
                #region Hunter
                case 2:
                characterStats[0].baseStats = 7;
                characterStats[1].baseStats = 10;
                characterStats[2].baseStats = 3;
                characterStats[3].baseStats = 6;
                characterStats[4].baseStats = 16;
                characterStats[5].baseStats = 11;

                characterClass = CharacterClass.Hunter;
                break;
                #endregion
                #region SprintyBoi
                case 3:
                characterStats[0].baseStats = 3;
                characterStats[1].baseStats = 10;
                characterStats[2].baseStats = 6;
                characterStats[3].baseStats = 11;
                characterStats[4].baseStats = 6;
                characterStats[5].baseStats = 18;

                characterClass = CharacterClass.SprintyBoi;
                break;
                #endregion
                #region Mage
                case 4:
                characterStats[0].baseStats = 18;
                characterStats[1].baseStats = 15;
                characterStats[2].baseStats = 5;
                characterStats[3].baseStats = 10;
                characterStats[4].baseStats = 19;
                characterStats[5].baseStats = 6;

                characterClass = CharacterClass.Mage;
                break;
                #endregion
            }
            UpdateText();
        }
        public void ChooseRace(int raceIndex)
        {
            race = (CharacterRace)raceIndex;
            UpdateText();
        } 
        #region Stats
        private void UpdateText()
        {
            pointText.text = "Points: " + statPoints.ToString();
            for (int i = 0; i < characterStats.Length; i++) pointsText[i].text = ":" + (characterStats[i].baseStats + characterStats[i].tempStats).ToString();
        }
        private void SpawnButtons()
        {
            for (int i = 0; i < characterStats.Length; i++)
            {
                int copy = i;
                var obj = Instantiate(prefab, content);
                string pointText = ":" + (characterStats[i].baseStats + characterStats[i].tempStats).ToString();
                
                obj.Find("Name").GetComponent<TextMeshProUGUI>().text = characterStats[i].baseStatsName;
                obj.Find("Point").GetComponent<TextMeshProUGUI>().text = pointText; 
                
                obj.Find("+").GetComponent<Button>().onClick.AddListener(delegate {  if(!allStatsAssigned)AssignStats(1, copy); UpdateText(); });
                obj.Find("-").GetComponent<Button>().onClick.AddListener(delegate { AssignStats(-1, copy); UpdateText(); });
                pointsText.Add(obj.Find("Point").GetComponent<TextMeshProUGUI>());
            }
        }
        /// <summary>
        /// Used to assign the characters stat. <br/>
        /// this method handles incrementing the points <br/>
        /// the player will not be able to assign more then 20 points <br/>
        /// to the character
        /// </summary>
        /// <param name="points">how much to increment by</param>
        /// <param name="ID">the index of the stat</param>
        public void AssignStats(int points, int id)
        {
            characterStats[id].tempStats += points;

            // make sure the player cant add more then
            // the max value of the base and temp stat combined
            int baseStat = characterStats[id].baseStats;
            int tempStat = characterStats[id].tempStats;
            
            int maxValue = 20 - baseStat;

            if (tempStat >= maxValue) characterStats[id].tempStats = maxValue;
            if (tempStat <= maxValue) statPoints -= points;

            // make sure that the player cant assign negative temp stat points
            if (tempStat <= 0) characterStats[id].tempStats = 0;
            #region StatPoints
            if (statPoints <= 0) statPoints = 0;
            if (statPoints >= 10) statPoints = 10;
            #endregion

            // check if the player has used all the stats they have avaliable
            if (statPoints <= 0) allStatsAssigned = true;
            else if (statPoints >= 1) allStatsAssigned = false;
        }
        #endregion
        #endregion 
        #region Debugging
        //private void OnGUI()
        //{
        //    GUI.matrix = IMGUIUtils.IMGUIMatrix(1920, 1080);

        //    if (GUI.Button(new Rect(0, 0, 50, 50), ""))
        //    {
        //        raceIndex++;
        //        if (raceIndex > 4)
        //        {
        //            raceIndex = 0;
        //        }
        //        ChooseRace(raceIndex);
        //    }

        //    if (GUI.Button(new Rect(0, 50, 50, 50), ""))
        //    {
        //        classIndex++;
        //        if (classIndex > 4)
        //        {
        //            classIndex = 0;
        //        }
        //        ChooseClass(classIndex);
        //    }

        //    if(GUI.Button(new Rect(50, 100, 25, 25), ">"))
        //    {
        //        AssignStats(1, 0);
        //    }

        //    if (GUI.Button(new Rect(0, 100, 25, 25), "<"))
        //    {
        //        AssignStats(-1, 0);
        //    }
        //}
        #endregion
    }
}

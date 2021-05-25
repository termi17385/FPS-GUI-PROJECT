using Sirenix.OdinInspector;
using UnityEngine;

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
        #endregion
        #region Race
        public CharacterRace race = CharacterRace.Human;
        
        // debugging index
        public int raceIndex;
        #endregion
        #region Class
        public CharacterClass characterClass = CharacterClass.Hunter;
        
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

            ChooseClass(classIndex);
            ChooseRace(raceIndex);

            #region Stat Names
            characterStats[0].baseStatsName = "Charisma";
            characterStats[1].baseStatsName = "Speed";
            characterStats[2].baseStatsName = "Strength";
            characterStats[3].baseStatsName = "Defense";
            characterStats[4].baseStatsName = "Stamina";
            characterStats[5].baseStatsName = "Mana";
            #endregion
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
        }
        public void ChooseRace(int raceIndex)
        {
            race = (CharacterRace)raceIndex;
        }
        /// <summary>
        /// Used to assign the characters stat. <br/>
        /// this method handles incrememting the points <br/>
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

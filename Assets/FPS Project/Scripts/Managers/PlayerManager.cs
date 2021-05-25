using FPSProject.Player.inventory;
using System.Collections.Generic;
using FPSProject.Player.stats;
using Sirenix.OdinInspector;
using UnityEngine.Events;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

namespace FPSProject.Player.Manager
{
    public class PlayerManager : SerializedMonoBehaviour
    {
        #region Variables
        

        #region Arrays, Lists and Events

        #region Stats, class and race data
        [Title("Character Data")]
        #region Race, class and name
        public string classType;
        public string raceType;
        public string characterName;
        #endregion
        #region Stats
        [System.Serializable] public struct _PlayerStats
        {
            public string name;
            public int value;
        }
        public List<_PlayerStats> _pStats = new List<_PlayerStats>();
        #endregion
        #endregion
        
        [Title("LoadDataEvent")]
        [SerializeField] UnityEvent LoadCustomisationData;
        [Title("Text Elements")]
        [FoldoutGroup("Arrays"), SerializeField]
        TextMeshProUGUI[] statsText;
        [FoldoutGroup("Arrays"), SerializeField]
        TextMeshProUGUI[] characterInfo;
        #endregion
        #endregion


        protected virtual void Start()
        {
            LoadCustomisationData.Invoke();
            DisplayStats();
        }
        private void DisplayStats()
        {
            for(int i = 0; i < _pStats.Count; i++)
            {
                string debugText = string.Format("name: {0}" +
                "\nvalue: {1}",_pStats[i].name, _pStats[i].value);

                Debug.Log(debugText);

                statsText[i].text = string.Format("{0}: {1}", _pStats[i].name, _pStats[i].value);
            }

            characterInfo[0].text = characterName;
            characterInfo[1].text = classType;
            characterInfo[2].text = raceType;

            characterInfo[3].text = AssignDescription(classType);
        }

        /// <summary>
        /// handles displaying the descriptions of the classes
        /// </summary>
        /// <param name="classType">The class the player loads in as</param>
        /// <returns>A description of the class</returns>
        private string AssignDescription(string classType)
        {
            string description = "";

            switch (classType)
            {
                case "Stealth":
                return description = string.Format
                ("<b>Description:</b> The stealth class is sneaky and quick able to take down enemies undetected \n \n" +
                "<b>Class Ablities:</b> this class can cloak hidding from enemies for a short period of time \n \n" +
                "<b>Class Bonus:</b> less chance of being detected faster movement when crouched");
                break;
                
                case "Tank":
                return description = string.Format
                ("<b>Description:</b> The Tank class is strong and slow able to take a lot of hits before dying \n \n" +
                "<b>Class Ablities:</b> this class can activate an energy shield for a short period of time allowing them to absorb damage \n \n" +
                "<b>Class Bonus:</b> Increase health and damage but lower speed and stamina");
                break;

                case "Hunter":
                return description = string.Format
                ("<b>Description:</b> The hunter class is faster then the stealth class but not as sneaky able to move fast and jump further \n \n" +
                "<b>Class Ablities:</b> class comes equiped with the ability to mark enemy targets seeing them through walls \n \n" +
                "<b>Class Bonus:</b> increased jump range and faster movement speed also less chance to be spotted");
                break;
                
                case "SprintyBoi":
                return description = string.Format
                ("<b>Description:</b> The sprintyboi class is well just that he runs fast and thats it \n \n" +
                "<b>Class Ablities:</b> increased speed and bullet time <i> maybe </i> \n \n" +
                "<b>Class Bonus:</b> fast reload fast sprint high stamina");
                break;
                
                case "Mage":
                return description = string.Format
                ("<b>Description:</b> The mage class is cunning and smart able to get great deals on potions and spells \n \n" +
                "<b>Class Ablities:</b> ability to teleport, shoot fireballs and self heal \n \n" +
                "<b>Class Bonus:</b> increased mana regen and great deals on potions");
                break;
            }
            return null;
        }
    }
}


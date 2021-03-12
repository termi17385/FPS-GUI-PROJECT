using UnityEngine.UI;
using UnityEngine;
using TMPro;
using FPSProject.Player.stats;
using FPSProject.Player.inventory;

namespace FPSProject.Player.Manager
{
    public class PlayerManager : MonoBehaviour
    {
        #region Variables
        public float health;
        public float maxHeatlh;
        private string assignClass;

        public int medkits;

        public Image healthbar;
        public TextMeshProUGUI number;
        public TextMeshProUGUI medKitsText;

        [SerializeField] public PlayerStats pStats = new PlayerStats();
        [SerializeField] public PlayerInventory pInv = new PlayerInventory();

        [SerializeField] private Transform LKP;  // Last Known Position;

        #region Class Abilities
        [Header("Class Abilites")]
        [Tooltip("activates the Cloaking ability")]
        [SerializeField] private bool isCloaked;
        [Tooltip("activates the Tracking ability")]
        [SerializeField] private bool isTracking;
        [Tooltip("activates the Shield ability")]
        [SerializeField] private bool deployShield;
        [Tooltip("activates the Dash ability")]
        [SerializeField] private bool canDash;
        [Tooltip("Swaps to the FireBall ability")]
        [SerializeField] private bool fireBall;
        [Tooltip("Swaps to the MagicMissile ability")] 
        [SerializeField] private bool magicMissile;
        #endregion
        #endregion

        #region Start and Update
        private void Awake()
        {
            AssignNames();
            LoadStatsAndClasses();
        }

        // Start is called before the first frame update
        void Start()
        {
            //health = maxHeatlh;
            //SetHealth(health);
            //number.text = string.Format("{0}/{1}", health, maxHeatlh);
            //medKitsText.text = string.Format("{0}", medkits);
        }

        // Update is called once per frame
        void Update()
        {
            AssignClass(assignClass);
            //medKitsText.text = string.Format("{0}", medkits);
        }
        #endregion

        #region Player Leveling
        #region Loading Values
        public void LoadStatsAndClasses()
        {
            var x = pStats.stats.Length;
            
            for (int i = 0; i < x; i++)
            {
                // assigns the stat value match name of the stat
                pStats.stats[i].statsNum = PlayerPrefs.GetInt(pStats.stats[i].statNames);
            }

            // load the class of the player
            assignClass = PlayerPrefs.GetString("CharacterClass");
        }
        public void AssignClass(string className)
        {
            switch (className)
            {
                case "Stealth":
                StealthClass();
                pStats.characterClass = 
                PlayerStats.CharacterClass.Stealth;
                break;
                
                case "Tank":
                TankClass();
                pStats.characterClass = 
                PlayerStats.CharacterClass.Tank;
                break;
                
                case "Hunter":
                HunterClass();
                pStats.characterClass = 
                PlayerStats.CharacterClass.Hunter;
                break;
                
                case "SprintyBoi":
                SprintyBoiClass();
                pStats.characterClass = 
                PlayerStats.CharacterClass.SprintyBoi;
                break;

                case "Mage":
                MageClass();
                pStats.characterClass = 
                PlayerStats.CharacterClass.Mage;
                break;
            } 
        }
        public void AssignNames()
        {
            pStats.stats[0].statNames = "Charisma";
            pStats.stats[1].statNames = "Intelligence";
            pStats.stats[2].statNames = "Strength";
            pStats.stats[3].statNames = "Dexterity";
            pStats.stats[4].statNames = "Constitution";
            pStats.stats[5].statNames = "Agility";
        }
        #endregion

        #region Player Classes
        private void StealthClass()
        {
            // The player is less likely to be detected
            // Player has a faster crouch speed then normal
            // The player can cloak and enemies will go to last known pos
            if (Input.GetKeyDown(KeyCode.T)) { isCloaked = true;
            if (GameObject.FindGameObjectWithTag("LKP") == null && isCloaked == true) 
            { Instantiate(LKP, transform.position, transform.rotation); } }
            // look around then go back to patrol
        }

        private void HunterClass()
        {
            // The player can set a target to be tracked seeing them through walls
            // The player has higher resistance to special damage
            // Faster movement and higher jumping
        }

        private void TankClass()
        {
            // The player can activate a shield around themselve
            // The player has more health 
            // The player is slightly slower but more stamina
        }

        private void SprintyBoiClass()
        {
            // fast movement
            // speed reload
            // dash ability
        }

        private void MageClass()
        {
           // Fire ball ability
           // faster heal buff
           // attack resistance
        }
        #endregion
        #endregion

        #region Methods
        /// <summary>
        /// Used to damage the player
        /// </summary>
        /// <param name="dmg">how much to damage the player by</param>
        public void Damage(float dmg)
        {
            health -= dmg;
            SetHealth(health);

            if (health <= 0f)
            {
                health = 0f;
            }

            number.text = string.Format("{0}/{1}", health, maxHeatlh);
        }

        /// <summary>
        /// Used to heal the player
        /// </summary>
        /// <param name="heal">how much to heal the player by</param>
        public void Heal(float heal)
        {
            health += heal;
            SetHealth(health);

            if (health >= maxHeatlh)
            {
                health = maxHeatlh;
            }

           // number.text = string.Format("{0}/{1}", health, maxHeatlh);
        }

        public void SetHealth(float _health)
        {
           //healthbar.fillAmount = Mathf.Clamp01(_health / maxHeatlh);
        }
        #endregion
    }
}


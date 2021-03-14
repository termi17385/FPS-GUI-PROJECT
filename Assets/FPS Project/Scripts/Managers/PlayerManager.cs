using UnityEngine.UI;
using UnityEngine;
using TMPro;
using FPSProject.Player.stats;
using FPSProject.Player.inventory;
using NaughtyAttributes;

namespace FPSProject.Player.Manager
{
    public class PlayerManager : MonoBehaviour
    {
        #region Properties
        /// <summary>
        /// Clamps the health of the player <br/>
        /// between 0 and 1 so it can be used for <br/>
        /// the health bar
        /// </summary>
        public float PlayerHeatlh
        {
            get {return (Mathf.Clamp01(health / maxHealth));}
            set {health = value;}
        }
        /// <summary>
        /// Clamps the detectionMeter of the player <br/>
        /// between 0 and 1 so it can be used for <br/>
        /// the detection bar
        /// </summary>
        public float DetectionMeter
        {
            get {return (Mathf.Clamp01(detection / maxDetection));}
            set {detection = value;}
        }
        #endregion

        #region Variables
        #region Health
        [Header("Health")]
        private float health;
        protected float maxHealth = 100;
        [SerializeField] private Image healthBar;
        #endregion
        #region Stealth
        [SerializeField] private float detection;
        private float maxDetection = 100;
        [SerializeField] private Image detectionBar;
        #endregion

        [SerializeField, ReadOnly] private float heathDisplay;
        private string assignClass;

        public int medkits;

        public TextMeshProUGUI number;
        public TextMeshProUGUI medKitsText;

        [SerializeField, ReadOnly] public PlayerStats pStats = new PlayerStats();
        [SerializeField, ReadOnly] public PlayerInventory pInv = new PlayerInventory();

        [SerializeField] private Transform LKP;  // Last Known Position;

        #region Class Abilities
        [Header("Class Abilites")]
        [Tooltip("activates the Cloaking ability"), ReadOnly]
        [SerializeField] private bool isCloaked;
        [Tooltip("activates the Tracking ability"), ReadOnly]
        [SerializeField] private bool isTracking;
        [Tooltip("activates the Shield ability"), ReadOnly]
        [SerializeField] private bool deployShield;
        [Tooltip("activates the Dash ability"), ReadOnly]
        [SerializeField] private bool canDash;
        [Tooltip("Swaps to the FireBall ability"), ReadOnly]
        [SerializeField] private bool fireBall;
        [Tooltip("Swaps to the MagicMissile ability"), ReadOnly] 
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
            PlayerHeatlh = maxHealth;
            DetectionMeter = 0;
            number.text = string.Format("{0}/{1}", health, maxHealth);
        }
        private void LateUpdate()
        {
            #region SettingStuff
            SetHealth();
            SetDetection();
            #endregion
        }

        // Update is called once per frame
        void Update()
        {
            AssignClass(assignClass);
            heathDisplay = PlayerHeatlh;
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
        #region Health
        /// <summary>
        /// Used to damage the player
        /// </summary>
        /// <param name="dmg">how much to damage the player by</param>
        public void Damage(float dmg)
        {
            PlayerHeatlh -= dmg;
            if (health <= 0){PlayerHeatlh = 0;}  // kill the player

            number.text = string.Format("{0}/{1}", PlayerHeatlh, maxHealth);
        }

        /// <summary>
        /// Used to heal the player
        /// </summary>
        /// <param name="heal">how much to heal the player by</param>
        public void Heal(float heal)
        {
            PlayerHeatlh += heal;
            if (health >= maxHealth){PlayerHeatlh = maxHealth;}

            number.text = string.Format("{0}/{1}", PlayerHeatlh, maxHealth);
        }

        public void SetHealth()
        {
            healthBar.fillAmount = PlayerHeatlh;
        }
        #endregion
        #region Detection
        private void SetDetection()
        {
            detectionBar.fillAmount = DetectionMeter;
        }

        public void DetectionMeterSpotted(float amt)
        {
            detection += amt * Time.deltaTime;
            if (detection >= maxDetection){detection = 100;}
        }
        public void DetectionMeterHidden(float amt)
        {
            detection -= amt * Time.deltaTime;
            if (detection <= 0){detection = 0;}
        }
        #endregion
        #endregion
    }
}


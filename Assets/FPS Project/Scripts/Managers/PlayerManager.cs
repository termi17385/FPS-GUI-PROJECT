using System.Collections.Generic;
using Sirenix.OdinInspector;
using System.Collections;
using FPSProject.Menu;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

namespace FPSProject.Player.Manager
{
    public class PlayerManager : SerializedMonoBehaviour
    {
        #region Properties
        #region Stats
        [ShowInInspector, FoldoutGroup("Text Elements/ArraysAndProperties")]
        public int PointPool
        {
            get => pointPool;
            set
            {
                pointPool = value;
                if (pointPool >= maxPoint) pointPool = maxPoint;
                if (pointPool > 0) allStatsAssigned = false;
                if (pointPool <= 0)
                {
                    pointPool = 0;
                    allStatsAssigned = true;
                }
            }
        }
        #endregion
        #endregion
        #region Variables
        #region Health, mana and stamina
        [TitleGroup("Health, Mana and Stamina")]
        [HorizontalGroup("Health, Mana and Stamina/Split")]
        #region CurrentValues
        [SerializeField, TabGroup("Health, Mana and Stamina/Split/Parameters", "Variables")] private float currentHealth;
        [SerializeField, TabGroup("Health, Mana and Stamina/Split/Parameters", "Variables")] private float currentStamina;
        [SerializeField, TabGroup("Health, Mana and Stamina/Split/Parameters", "Variables")] private float currentMana;   
        #endregion
        #region Max Values
        [SerializeField, TabGroup("Health, Mana and Stamina/Split/Parameters", "Variables")] private float maxHealth = 100;
        [SerializeField, TabGroup("Health, Mana and Stamina/Split/Parameters", "Variables")] private float maxStamina = 100;
        [SerializeField, TabGroup("Health, Mana and Stamina/Split/Parameters", "Variables")] private float maxMana = 100;
        #endregion
        #region Properties
        [ShowInInspector, TabGroup("Health, Mana and Stamina/Split/Parameters", "Properties")] public float Health
        {
            get => currentHealth;
            set 
            {
                currentHealth = value;

                if (currentHealth >= maxHealth)
                {
                    currentHealth = maxHealth;
                    startRegen[0] = false;
                }
            } 
            
        }
        [ShowInInspector, TabGroup("Health, Mana and Stamina/Split/Parameters", "Properties")] public float Stamina
        {
            get => currentStamina;
            set 
            { 
                currentStamina = value;

                if (currentStamina >= maxStamina)
                {
                    currentStamina = maxStamina;
                    startRegen[1] = false;
                }
            }
        }
        [ShowInInspector, TabGroup("Health, Mana and Stamina/Split/Parameters", "Properties")] public float Mana
        {
            get => currentMana;
            set 
            {
                currentMana = value;

                if (currentMana >= maxMana)
                    currentMana = maxMana;
            } 
        }
        #endregion
        #endregion
        #region Stats
        #region Text
        [TitleGroup("Text Elements")]
        [FoldoutGroup("Text Elements/ArraysAndProperties"), SerializeField] TextMeshProUGUI[] statsText;
        [FoldoutGroup("Text Elements/ArraysAndProperties"), SerializeField] TextMeshProUGUI[] characterInfo;
        [FoldoutGroup("Text Elements/ArraysAndProperties"), SerializeField] TextMeshProUGUI[] uiText;
        [FoldoutGroup("Text Elements/ArraysAndProperties"), SerializeField] Image[] uiImages;
        #endregion
        #region buttonSpawning
        [Title("Objects")]
        [SerializeField] GameObject statPrefab;
        [SerializeField] Transform buttonLocation;
        #endregion
        [SerializeField] private int pointPool = 0;
        public int maxPoint;
        #region Booleans
        public bool button;
        public bool saved;
        public bool allStatsAssigned;
        #endregion
        #region Race, class and name
        #region Stats
        /// <summary>
        /// Class that stores all the stats of the player
        /// </summary>
        [System.Serializable] public class _PlayerStats
        {
            public string name;
            public int statValue;
            private int tempStat;   
            [ShowInInspector]public int TempStat
            {
                get => tempStat;
                set
                {
                    tempStat = value;
                    if(tempStat <= 0)
                    {
                        tempStat = 0;
                    }
                }
            }
            #region Old Code
            /*
            
            Event for on variable changed

             */
            //[ShowInInspector]                        
            //public int Value
            //{
            //    get => statValue;
            //    set
            //    {
            //        if (statValue == value) 

            //        return;

            //        statValue = value;
            //        if (onValueChanged != null) 
            //        onValueChanged();
            //    }
            //}

            //public delegate void OnValueChangedDelegate();
            //public event OnValueChangedDelegate onValueChanged;
            #endregion
        }
        [Title("Character Data")]
        public List<_PlayerStats> _pStats = new List<_PlayerStats>();
        #endregion
        #region Strings
        [ReadOnly] public string classType;
        [ReadOnly] public string raceType;
        [ReadOnly] public string characterName;
        #endregion
        #endregion
        #region Load
        [Title("LoadDataEvent")]
        [SerializeField] UnityEvent LoadCustomisationData;
        #endregion
        #endregion
        public float xpLevel = 0;
        public AudioSource soundEffect;
        private PlayerController pControl; 
        [SerializeField] private AudioClip[] clips;
        public GameObject hitMarker;
        #endregion
        public Image damageIndicator;
        public float alpha;
        public float[] time = new float[2];
        [SerializeField] private bool[] startRegen = new bool[2];
        [SerializeField] private float[] regenAmt = new float[2];
        #region Start and Update
        protected virtual void Start()
        {
            #region Stats stuff
            LoadCustomisationData.Invoke();
            DisplayTextOnStart();
            #endregion
            #region Button spawning
            GameObject obj;
            Button spawnedButtonPos;
            Button spawnedButtonNeg;
            for (int i = 0; i < _pStats.Count; i++)
            {
                int x = i;
                // spawns the stat bar object
                obj = Instantiate(statPrefab, buttonLocation).gameObject;
                
                // assigns the buttons
                spawnedButtonPos = obj.transform.Find("+").GetComponent<Button>();
                spawnedButtonNeg = obj.transform.Find("-").GetComponent<Button>();
                
                // assigns the text
                statsText[i] = obj.transform.Find("Image").transform.Find("statText").GetComponent<TextMeshProUGUI>();

                // sets up listeners
                spawnedButtonPos.onClick.AddListener(delegate 
                {
                    if (!allStatsAssigned)
                    {
                        onClickStats(x, 1);
                        DisplayStats();
                    }
                });
                spawnedButtonNeg.onClick.AddListener(delegate 
                { 
                    onClickStats(x, -1);
                    DisplayStats();
                });
            }
            #endregion

            for (int i = 0; i < time.Length; i++)
            {
                time[i] = 5;
                startRegen[i] = false;
                regenAmt[i] = 10;
            }
            
            DisplayStats();
            allStatsAssigned = true;

            Health = maxHealth;
            Mana = maxMana;
            Stamina = maxStamina;

            pControl = GetComponent<PlayerController>();

            soundEffect = GetComponent<AudioSource>();
            soundEffect.clip = clips[0];
        }

        protected virtual void Update()
        {
            Timer();
            characterInfo[4].text = $"Points: {pointPool}";
            #region Health, Mana and Stamina
            DisplayHealth();
            DisplayStamina();
            DisplayMana();
            #endregion
            if (saved)
            {
                SaveStats();
            }
        }
        
        /// <summary>
        /// Timers for handling the regen of stamina, mana and health
        /// </summary>
        private void Timer()
        {
            time[0] -= Time.deltaTime;
            if (time[0] <= 0.0f)
            {
                if (Health <= 90)
                    startRegen[0] = true;
                    
                HealthRegen(regenAmt[0]);//Mathf.RoundToInt(5 * Time.deltaTime));
                time[0] = 0.0f;
            }
            
            time[1] -= Time.deltaTime;
            if (time[1] <= 0.0f)
            {
                if (Stamina <= 90) startRegen[1] = true;
                StaminaRegen(regenAmt[1]);//Mathf.RoundToInt(5 * Time.deltaTime));
                time[1] = 0.0f;
            }
        }
        #endregion
        #region Methods
        #region Health
        /// <summary>
        /// Displays the health of the character
        /// </summary>
        private void DisplayHealth()
        {
            float value = Mathf.Clamp01(Health/maxHealth);
            Color temp = damageIndicator.color;
            temp.a = Mathf.Clamp01(alpha/100);
            damageIndicator.color = temp;
            uiText[0].text = Mathf.RoundToInt(Health).ToString();
            uiImages[0].fillAmount = value;
        }
        /// <summary>
        /// Regenerates the players health by amt
        /// </summary>
        /// <param name="_Amt">the amount of health the player regens by</param>
        private void HealthRegen(float _amt)
        {
            if (startRegen[0])
            {
                Health += (_amt * Time.deltaTime); // regens the health 
                alpha -= (_amt * 20);   // turns of the red border of death
                if (alpha <= 0)
                {
                    alpha = 0;
                }
            }
        }
        /// <summary>
        /// How much to damage the player by
        /// </summary>
        /// <param name="_dmg">the amount of damage</param>
        public void DamagePlayer(int _dmg)
        {
            time[0] = 5;
            Health -= _dmg;
            alpha += _dmg * 5;
            if (Health <= 0)
            {
                Health = 0;
                StartCoroutine(PlayerDied());
            }
        }
        /// <summary>
        /// handles the killing of the player
        /// </summary>
        IEnumerator PlayerDied()
        {
            SceneManager.LoadScene("Death");
            Debug.Log("YOU HAVE DIED");
            yield return null;
        }
        #endregion
        #region Stamina
        /// <summary>
        /// Displays the stamina of the player
        /// </summary>
        private void DisplayStamina()
        {
            float value = Mathf.Clamp01(Stamina / maxStamina);
            uiText[1].text = Mathf.RoundToInt(Stamina).ToString();
            uiImages[1].fillAmount = value;
        }
        /// <summary>
        /// Handles the regeneration of the players stamina
        /// </summary>
        /// <param name="_amt">How much to regen</param>
        private void StaminaRegen(float _amt)
        {
            if (startRegen[1])
            {
                Stamina += (_amt * Time.deltaTime);
            }
        }
        /// <summary>
        /// handles the depletion of the players stamina
        /// </summary>
        /// <param name="_amt">how much to deplete</param>
        public void StaminaDepletion(float _amt)
        {
            Stamina -= (_amt * Time.deltaTime);
            if(Stamina <= 0)
            {
                Stamina = 0;
                // start stamina regen
            }
        }
        #endregion
        #region Mana
        private void DisplayMana()
        {
            float value = Mathf.Clamp01(Mana / (float)maxMana);
            uiText[2].text = Mana.ToString();
            uiImages[2].fillAmount = value;
        }
        private void RegenMana(int _amt)
        {
            Mana += _amt;
        }
        public void DepleteMana(int _amt)
        {
            Mana -= _amt;
            if (Mana <= 0)
            {
                Mana = 0;
                // start stamina regen
            }
        }
        #endregion
        #region Stats
        /// <summary>
        /// Method for handling all of the players upgrades <br/>
        /// is called only when stats are changed
        /// </summary>
        public void StatUpgrades()
        {
            #region Default values
            // sets the starting values
            pControl.sprintSpeed = 10;
            pControl.crouchSpeed = 2;
            pControl.walkSpeed = 5;
            pControl.jumpHeight = 3;
            maxHealth = 100;
            #endregion
            #region Hard Values
            // gets the values of all the stats and upgrades
            float speed = pControl.sprintSpeed;
            float jump = pControl.jumpHeight;

            int statValue = _pStats[1].statValue;
            int statValue1 = _pStats[2].statValue;
            int statValue2 = _pStats[1].statValue;
            #endregion

            pControl.sprintSpeed = speed + (statValue * 0.3f);  // increases the sprint speed by a percentage of the stat value
            pControl.walkSpeed = pControl.sprintSpeed * 0.5f;   // increases the walk speed by a percentage of the stat value

            maxHealth += statValue1;                            // increases the maxHealth
            pControl.jumpHeight = jump + (statValue1 * 0.03f);  // increases the jumpheight by a percentage of the stat value
            Health = maxHealth;                                 // health is set to maxhealth
        }

        /// <summary>
        /// changes the given stats values when called
        /// </summary>
        /// <param name="_index">which stat to change</param>
        /// <param name="_value">how much to change the stat by</param>
        private void onClickStats(int _index, int _value)
        {
            _pStats[_index].TempStat += _value;  
            PointPool -= _value;
        }   
        /// <summary>
        /// handles the point pool and 
        /// how much to allocate to the player
        /// </summary>
        /// <param name="value">how many points to give the player</param>
        public void SetPointPool(int value)
        {
            maxPoint += value;          // used to limit how many points the player can regain
            pointPool += value; 
            allStatsAssigned = false;   // stops the player from increasing
                                        // their stats when they run out of points
        }
        /// <summary>
        /// When called saves the stats so 
        /// that the player cant change them again later
        /// </summary>
        public void SaveStats()
        {
            // loops through all the stats and adds the temp stat to the stat value
            // resets the point pool and displays everything
            for (int i = 0; i < _pStats.Count; i++)
            {
                _pStats[i].statValue += _pStats[i].TempStat;
                _pStats[i].TempStat = 0;

                maxPoint = pointPool;
            }
            DisplayStats();
            StatUpgrades();
            saved = false;
        }
        #region Displaying Stats
        /// <summary>
        /// Handles displaying:<br/>
        /// * Stats <br/>
        /// * Class <br/>
        /// * Race <br/>
        /// * name <br/>
        /// and the description of the choosen class
        /// </summary>
        private void DisplayStats()
        {
            // displays all the stats formatting them
            for (int i = 0; i < _pStats.Count; i++)
            {
                int baseVal = _pStats[i].statValue;
                int tempVal = _pStats[i].TempStat;

                statsText[i].text = string.Format("{0}: {1}", _pStats[i].name, (baseVal + tempVal));
            }
        }
        /// <summary>
        /// used to display the text and descriptions for the character on game start
        /// </summary>
        private void DisplayTextOnStart()
        {
            // displays name class and race
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
        #endregion
        #endregion

        private void OnGUI()
        {
            if (Debugging.debugMode)
            {
                GUI.matrix = IMGUIUtils.IMGUIMatrix(1920, 1080);
                if(GUI.Button(new Rect(500,200,100,50), "IncreaseStat"))
                {
                    SetPointPool(1);
                }
                if(GUI.Button(new Rect(500, 150, 100, 50), "Damage Player"))
                {
                    DamagePlayer(10);
                }

                string text = $"Walk Speed = {pControl.walkSpeed} \n" +
                              $"Sprint Speed = {pControl.sprintSpeed} \n" +
                              $"Jump Height = {pControl.jumpHeight} \n" +
                              $"Crouch Speed = {pControl.crouchSpeed} \n \n" +
                              $"" +
                              $"Current Speed = {pControl.moveSpeed}";
                GUI.Box(new Rect(500, 500, 150, 100), text);
            }
        }
        #endregion
    }
}


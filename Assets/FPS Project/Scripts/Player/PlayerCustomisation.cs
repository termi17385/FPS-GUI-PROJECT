using System.Collections.Generic;
using UnityEngine;

namespace FPSProject.Customisation
{
    public class PlayerCustomisation : MonoBehaviour
    {
        #region
        #region Character name and class
        [Header("Character Name")]
        public string characterName;
        [Header("Character Class")]
        public CharacterClass characterClass = CharacterClass.Stealth;
        public string[] selectedClass = new string[4];
        public int selectedClassIndex = 0;
        [System.Serializable]
        public struct Stats
        {
            public string baseStatsName;
            public int baseStats;
            public int tempStats;
        };
        public Stats[] characterStats;
        public GameObject[] statChange;
        #endregion
        #region DropDown Menu
        [Header("DropDown Menu")]
        public bool showDropdown;
        public Vector2 scrollPos;
        public string classButton = "";
        public int statPoints = 10;
        #endregion
        #region Texture List
        [Header("Texture Lists")] // lists for holding all the textures for the different parts of the character
        public List<Texture2D> skin = new List<Texture2D>();  
        public List<Texture2D> eyes = new List<Texture2D>();
        public List<Texture2D> mouth = new List<Texture2D>();
        public List<Texture2D> hair = new List<Texture2D>();
        public List<Texture2D> armour = new List<Texture2D>();
        public List<Texture2D> clothes = new List<Texture2D>();
        #endregion
        #region misc
        [Header("Current texture Index")] // indexs for changing what position in an array
        public int skinIndex;
        public int eyesIndex, mouthIndex, hairIndex, armourIndex, clothesIndex;
        [Header("Renderer")]
        public Renderer characterRenderer;
        [Header("Max amount of textures per type")] // max values so we dont go outside of the array bounds
        public int skinMax;
        public int eyesMax, mouthMax, hairMax, armourMax, clothesMax;
        [Header("Material names")]  // ids for the materials
        public string[] matName = new string[6];

        public bool debugMode = false;
        #endregion
        #endregion

        public void Start()
        {
            matName = new string[] {"Skin", "Eyes", "Mouth", "Hair","Clothes","Armour"};
            selectedClass = new string[] {"Stealth", "Tank", "Hunter", "SprintyBoi", "Mage"};

            debugMode = false;

            #region TemporayNameAssignMentForStats
            characterStats[0].baseStatsName = "Charisma";
            characterStats[1].baseStatsName = "Intelligence";
            characterStats[2].baseStatsName = "Strength";
            characterStats[3].baseStatsName = "Dexterity";
            characterStats[4].baseStatsName = "Constitution";
            characterStats[5].baseStatsName = "Agility";
            #endregion

            #region For loops for assigning textures
            /* 
               Loops through the resources folder for
               the coresponding textures
               and adds them to the matching list
            */
            ////skin.AddRange(Resources.LoadAll<Texture2D>());
            for (int i = 0; i < skinMax; i++)       // loops until i is no longer less then defined value
            {
                Texture2D tempTexture = Resources.Load("Character/Skin_" + i) as Texture2D;     // looks through the resources folder for the skin texture
                skin.Add(tempTexture);                                                          // assigns the texture to the array
            } 
            for (int i = 0; i < eyesMax; i++)       // loops until i is no longer less then defined value
            {
                Texture2D tempTexture = Resources.Load("Character/Eyes_" + i) as Texture2D;     // looks through the resources folder for the eyes texture
                eyes.Add(tempTexture);                                                          // assigns the texture to the array
            }
            for (int i = 0; i < mouthMax; i++)      // loops until i is no longer less then defined value
            {
                Texture2D tempTexture = Resources.Load("Character/Mouth_" + i) as Texture2D;    // looks through the resources folder for the mouth texture
                mouth.Add(tempTexture);                                                         // assigns the texture to the array
            }
            for (int i = 0; i < hairMax; i++)       // loops until i is no longer less then defined value
            {
                Texture2D tempTexture = Resources.Load("Character/Hair_" + i) as Texture2D;     // looks through the resources folder for the hair texture
                hair.Add(tempTexture);                                                          // assigns the texture to the array
            }
            for (int i = 0; i < armourMax; i++)     // loops until i is no longer less then defined value
            {
                Texture2D tempTexture = Resources.Load("Character/Armour_" + i) as Texture2D;   // looks through the resources folder for the armour texture
                armour.Add(tempTexture);                                                        // assigns the texture to the array
            }
            for (int i = 0; i < clothesMax; i++)    // loops until i is no longer less then defined value
            {
                Texture2D tempTexture = Resources.Load("Character/Clothes_" + i) as Texture2D;  // looks through the resources folder for the clothes texture
                clothes.Add(tempTexture);                                                       // assigns the texture to the array
            }
            #endregion
        }

        /// <summary>
        /// Sets the textures of the character when called based off the name and index value given
        /// </summary>
        /// <param name="type">The ID of the texture being changed</param>
        /// <param name="dir">The direction in the array we are going</param>
        public void SetTexture(string type, int dir)
        {
            // ints for the index matIndex and max value
            int index = 0, max = 0, matIndex = 0;
            Texture2D[] textures = new Texture2D[0];    // list of textures

            // this switch handles changing and assigning textures
            switch (type)
            {
                case "Skin":                    // the Id of the item we want to change
                index = skinIndex;              // sets the index to the starting value of skinIndex
                max = skinMax;                  // gets the skin's max value and sets max to it
                textures = skin.ToArray();      // sends the textures for skin to the skin array 
                matIndex = 1;                   // sets what material we are changing textures on
                break;

                case "Eyes":                    // the Id of the item we want to change
                index = eyesIndex;              // sets the starting value of the index
                max = eyesMax;                  // sets the max value to the Eyes max
                textures = eyes.ToArray();      // assigns the textures to the texture list
                matIndex = 2;                   // sets what array we are on
                break;

                case "Mouth":                   // the Id of the item we want to change
                index = mouthIndex;             // sets the starting value of the index
                max = mouthMax;                 // sets the max value to the Mouth max
                textures = mouth.ToArray();     // assigns the textures to the texture list
                matIndex = 3;                   // sets what array we are on
                break;
                                                
                case "Hair":                    // the Id of the item we want to change
                index = hairIndex;              // sets the starting value of the index
                max = hairMax;                  // sets the max value to the Hair max
                textures = hair.ToArray();      // assigns the textures to the texture list
                matIndex = 4;                   // sets what array we are on
                break;

                case "Clothes":                 // the Id of the item we want to change
                index = clothesIndex;           // sets the starting value of the index
                max = clothesMax;               // sets the max value to the Clothes max
                textures = clothes.ToArray();   // assigns the textures to the texture list
                matIndex = 5;                   // sets what array we are on
                break;
                                                
                case "Armour":                  // the Id of the item we want to change
                index = armourIndex;            // sets the starting value of the index
                max = armourMax;                // sets the max value to the Armour max
                textures = armour.ToArray();    // assigns the textures to the texture list
                matIndex = 6;                   // sets what array we are on
                break;
            }

            index += dir; // adds to the index from the number given when the method is called
            if(index < 0)
            {
                index = max - 1;
            }
            if(index > max - 1)
            {
                index = 0;
            }

            // gets the material
            Material[] mat = characterRenderer.materials;   // makes an array of materials
            mat[matIndex].mainTexture = textures[index];    // gets the materials
            characterRenderer.materials = mat;              // assigns what material we are changing 

            // handles changing the index values
            switch (type)
            {
                case "Skin":
                    skinIndex = index;      // assigns the Skin index to the current value of the index
                    break;
                case "Eyes":
                    eyesIndex = index;      // assigns the Eyes index to the current value of the index
                    break;
                case "Mouth":
                    mouthIndex = index;     // assigns the Mouth index to the current value of the index
                    break;
                case "Hair":
                    hairIndex = index;      // assigns the Hair index to the current value of the index
                    break;
                case "Clothes":
                    clothesIndex = index;   // assigns the Clothes index to the current value of the index
                    break;
                case "Armour":
                    armourIndex = index;    // assigns the Armour index to the current value of the index
                    break;
            }
        }

        public void ChooseClass(int classIndex)
        {
            switch (classIndex)
            {
                case 0:
                characterStats[0].baseStats = 11;   // charisma
                characterStats[1].baseStats = 15;   // Intelligence
                characterStats[2].baseStats = 5;    // Strength
                characterStats[3].baseStats = 10;   // Dexterity
                characterStats[4].baseStats = 9;    // Constitution
                characterStats[5].baseStats = 18;   // Agility

                characterClass = CharacterClass.Stealth;
                break;

                case 1:
                characterStats[0].baseStats = 5;    // charisma
                characterStats[1].baseStats = 2;    // Intelligence
                characterStats[2].baseStats = 16;   // Strength
                characterStats[3].baseStats = 5;    // Dexterity
                characterStats[4].baseStats = 17;   // Constitution
                characterStats[5].baseStats = 1;    // Agility

                characterClass = CharacterClass.Tank;
                break;

                case 2:
                characterStats[0].baseStats = 7;    // charisma
                characterStats[1].baseStats = 10;   // Intelligence
                characterStats[2].baseStats = 3;    // Strength
                characterStats[3].baseStats = 6;    // Dexterity
                characterStats[4].baseStats = 16;   // Constitution
                characterStats[5].baseStats = 11;   // Agility

                characterClass = CharacterClass.Hunter;
                break;

                case 3:
                characterStats[0].baseStats = 3;    // charisma
                characterStats[1].baseStats = 10;   // Intelligence
                characterStats[2].baseStats = 6;    // Strength
                characterStats[3].baseStats = 11;   // Dexterity
                characterStats[4].baseStats = 6;    // Constitution
                characterStats[5].baseStats = 18;   // Agility

                characterClass = CharacterClass.SprintyBoi;
                break;

                case 4:
                characterStats[0].baseStats = 18;   // charisma
                characterStats[1].baseStats = 15;   // Intelligence
                characterStats[2].baseStats = 5;    // Strength
                characterStats[3].baseStats = 10;   // Dexterity
                characterStats[4].baseStats = 19;   // Constitution
                characterStats[5].baseStats = 6;    // Agility

                characterClass = CharacterClass.Mage;
                break;
            }
        }

        void SaveCharacter()
        {
            PlayerPrefs.SetInt("SkinIndex", skinIndex);             // saves the index for skin
            PlayerPrefs.SetInt("HairIndex", hairIndex);             // saves the index for hair
            PlayerPrefs.SetInt("EyesIndex", eyesIndex);             // saves the index for eyes
            PlayerPrefs.SetInt("MouthIndex", mouthIndex);           // saves the index for mouth
            PlayerPrefs.SetInt("ClothesIndex", clothesIndex);       // saves the index for clothes
            PlayerPrefs.SetInt("ArmourMax", armourMax);             // saves the index for armour


            PlayerPrefs.SetString("CharacterName", characterName);  // saves the name of the character

            // for i < characterStats.Length loop until i is no longer less then characterStats.Length
            for (int i = 0; i < characterStats.Length; i++)
            {
                // saves the value of the stats to the name of the stats
                PlayerPrefs.SetInt(characterStats[i].baseStatsName, 
                    characterStats[i].baseStats + characterStats[i].tempStats);
            }

            // saves the selected class
            PlayerPrefs.SetString("CharacterClass", selectedClass[selectedClassIndex]);
        }

        /// <summary>
        /// Temporay GUI used for customising the characters look
        /// </summary>
        private void OnGUI()
        {
            if (debugMode)
            {
                #region GUI value Setup
                // 116 : 9
                Vector2 scr = new Vector2(Screen.width / 16, Screen.height / 9);

                // start positions
                float left = 0.25f * scr.x;
                float mid = 0.75f * scr.x;
                float right = 2.25f * scr.x;

                // sizes
                float x = 0.5f * scr.x;
                float y = 0.5f * scr.y;
                float label = 1.5f * scr.x;
                #endregion
                #region Customisation 
                // used to create a button for each of the customisation options
                for (int i = 0; i < matName.Length; i++)
                {
                    if (GUI.Button(new Rect(left, y + i * y, x, y), "<"))
                    {
                        // the -1 is used to go backwards in the array
                        SetTexture(matName[i], -1);    // changes the textures when left arrow is pressed
                    }
                    GUI.Box(new Rect(mid, y + i * y, label, y), matName[i]);
                    if (GUI.Button(new Rect(right, y + i * y, x, y), ">"))
                    {
                        SetTexture(matName[i], 1);    // changes the textures when right arrow is pressed
                    }
                }
                #endregion

                #region Stats Stuff
                #region Handles the classes
                float classX = 12.75f * scr.x;
                float h = 0;
                if (GUI.Button(new Rect(classX, y + h * y, 4 * x, y), classButton))
                {
                    showDropdown = !showDropdown;
                }
                h++;
                if (showDropdown)
                {

                    scrollPos = GUI.BeginScrollView(
                    new Rect(classX, y + h * y, 4 * x, 4 * y), scrollPos,
                    new Rect(0, 0, 0, selectedClass.Length * y), false, true);

                    for (int i = 0; i < selectedClass.Length; i++)
                    {
                        if (GUI.Button(new Rect(0, i * y, 3 * x, y), selectedClass[i]))
                        {
                            ChooseClass(i);
                            classButton = selectedClass[i];
                            showDropdown = false;
                        }
                    }
                    GUI.EndScrollView();
                }
                #endregion

                #region Handles the stat points
                // loops until i is no longer less then characterstats length
                // while looping i increases by i
                for (int i = 0; i < characterStats.Length; i++)
                {
                    // handles creating the gui placing i amount of buttons
                    if (GUI.Button(new Rect(left * 19, y + i * y, x, y), "+"))
                    {
                        // a variable that combines both character base stats and temp stats
                        int stats = characterStats[i].baseStats + characterStats[i].tempStats;

                        // if the starting stat points are greater then 0 and stats arent in total 20
                        if (statPoints > 0 && stats < 20)
                        {
                            characterStats[i].tempStats += 1;
                            statChange[i].SetActive(true);          // show the player there has been a change by setting the object active
                            statPoints -= 1;
                        }
                    }
                    // textbox that displays the name of eacher stat
                    GUI.Box(new Rect(mid * 7, y + i * y, label, y), characterStats[i].baseStatsName);
                    if (GUI.Button(new Rect(right * 3, y + i * y, x, y), "-"))
                    {
                        // if temp stats are less or equal to 1
                        if (characterStats[i].tempStats <= 1)
                        {
                            // disables the object so that the player can see base stats are at default values
                            statChange[i].SetActive(false);
                        }
                        // if stat points are < 10 and temp stats greater than 0
                        if (statPoints < 10 && characterStats[i].tempStats > 0)
                        {
                            characterStats[i].tempStats -= 1;
                            statPoints += 1;
                        }
                    }
                }
                #endregion
                #endregion
            }
        }

        public enum CharacterClass
        {
            Stealth,
            Tank,
            Hunter,
            SprintyBoi,
            Mage,
        }
    }
}


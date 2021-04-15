using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

namespace FPSProject.Customisation
{
    public class MyTexture
    {
        int index =0;
        int max = 0;
        list textures = [];

        public void add(Texture2D texture)
        {
            textures.add(texture);
            max = textures.size();
        }

        public array[] getTexturesArray()
        {
            textures.toArray();

        }
    }

    /*
     * 
 
    myTextures myEyes = new MyTextures();
    repeat for others;
    
    Resource[] allReses = Resources.loadAll("Characture/", Txtures2D);
    
    foreach(r in allReses) 
    {
    String name = substring(r.name, 0, search(r.name, '_'));
        
        switch (name) 
        {
            case 'Eyes':
            myEyes.add(r.content);
            break;
        }

     * 
     */

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
        #endregion
        #endregion

        public void Start()
        {
            matName = new string[] {"Skin", "Eyes", "Mouth", "Hair","Clothes","Armour"};
            selectedClass = new string[] {"Stealth", "Tank", "Hunter", "SprintyBoi", "Mage"};

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

        #region Methods for changing the characters classes and textures
        /// <summary>
        /// Used to let the player change the texture of there character so they 
        /// <br/> can customise the look of the character to their desire
        /// </summary>
        /// <param name="type">The ID of the texture being changed</param>
        /// <param name="dir">The direction in the array we are going</param>
        public void SetTexture(string type, int dir)
        {
            // ints for the index matIndex and max value
            int index = 0, max = 0, matIndex = 0;
            Texture2D[] textures = new Texture2D[0];    // list of textures

            #region Switch 1
            // used to get and assign the textures
            // and the indexs of the defined items
            switch (type)
            {
                #region Skin
                case "Skin":                    // the Id of the item we want to change
                index = skinIndex;              // sets the index to match the items index
                max = skinMax;                  // sets the max to the items max
                textures = skin.ToArray();      // gets the textures from items list and converts to array
                matIndex = 1;                   // sets what material we are changing textures on
                break;
                #endregion
                #region Eyes
                case "Eyes":                   
                index = eyesIndex;             
                max = eyesMax;                 
                textures = eyes.ToArray();     
                matIndex = 2;                  
                break;
                #endregion
                #region Mouth
                case "Mouth":                  
                index = mouthIndex;            
                max = mouthMax;                
                textures = mouth.ToArray();    
                matIndex = 3;                  
                break;
                #endregion
                #region Hair
                case "Hair":                   
                index = hairIndex;             
                max = hairMax;                 
                textures = hair.ToArray();     
                matIndex = 4;                  
                break;
                #endregion
                #region Clothes
                case "Clothes":                
                index = clothesIndex;          
                max = clothesMax;              
                textures = clothes.ToArray();  
                matIndex = 5;                  
                break;
                #endregion
                #region Armour
                case "Armour":                 
                index = armourIndex;           
                max = armourMax;               
                textures = armour.ToArray();   
                matIndex = 6;                  
                break;
                #endregion
            }
            #endregion

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

            #region Switch 2
            // handles changing the index values
            switch (type)
            {
                case "Skin":        
                    skinIndex = index;      // assigns the choosen item's index to the current value of the index
                    break;
                case "Eyes":
                    eyesIndex = index;     
                    break;
                case "Mouth":
                    mouthIndex = index;    
                    break;
                case "Hair":
                    hairIndex = index;     
                    break;
                case "Clothes":
                    clothesIndex = index;  
                    break;
                case "Armour":
                    armourIndex = index;   
                    break;
            }
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

                selectedClassIndex = 0;
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

                selectedClassIndex = 1;
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

                selectedClassIndex = 2;
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

                selectedClassIndex = 3;
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

                selectedClassIndex = 4;
                characterClass = CharacterClass.Mage;
                break;
                #endregion
            }
        }
        #endregion

        void SaveCharacter()
        {
            PlayerPrefs.SetInt("SkinIndex", skinIndex);             // saves the index for skin
            PlayerPrefs.SetInt("HairIndex", hairIndex);             // saves the index for hair
            PlayerPrefs.SetInt("EyesIndex", eyesIndex);             // saves the index for eyes
            PlayerPrefs.SetInt("MouthIndex", mouthIndex);           // saves the index for mouth
            PlayerPrefs.SetInt("ClothesIndex", clothesIndex);       // saves the index for clothes
            PlayerPrefs.SetInt("ArmourIndex", armourIndex);         // saves the index for armour

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
            if (Debugging.debugMode)
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
                // creates a button
                if (GUI.Button(new Rect(classX, y + h * y, 4 * x, y), classButton))
                {
                    // when the button is pressed it either shows the dropdown or hides it
                    showDropdown = !showDropdown;
                }
                h++;
                if (showDropdown)
                {
                    // creates a scroll bar at the set position with a set size and length
                    scrollPos = GUI.BeginScrollView(
                    new Rect(classX, y + h * y, 4 * x, 4 * y), scrollPos,
                    new Rect(0, 0, 0, selectedClass.Length * y), false, true); // gets the length of the scroll bar based
                                                              // on the length of the class list * 0.5 of he aspect ratio

                    // i < class list length keep looping until i is no longer less then the length
                    for (int i = 0; i < selectedClass.Length; i++)
                    {
                        // creates a new button each time it loops 1 under the other
                        if (GUI.Button(new Rect(0, i * y, 3 * x, y), selectedClass[i]))
                        {
                            ChooseClass(i);                     // when the button is pressed select defined class
                            classButton = selectedClass[i];     // displays the choosen class on the class button 
                            showDropdown = false;               // hides the drop down
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
                if (GUI.Button(new Rect(mid * 9, y + 8 * y, x + 3, y), "Save"))
                {
                    SaveCharacter();
                    SceneManager.LoadScene(2);
                }

                characterName = GUI.TextField(new Rect(left, 12*y, 5*x, y), characterName, 32);
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


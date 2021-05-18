using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using Sirenix.OdinInspector;


namespace FPSProject.Customisation
{
    [ExecuteInEditMode]
    public class PlayerCustomisation : MonoBehaviour
    {
        #region
        [System.Serializable]
        public struct GUILayout
        {
            [FoldoutGroup("Struct")]
            public Vector2 position;
            [FoldoutGroup("Struct")]
            public Vector2 size;
        }

        public bool runInEditor;
        [SerializeField] private bool statsScreen;
        private bool allStatsAssigned;

        [SerializeField]
        private List<GUILayout> layouts = new List<GUILayout>();

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
        //public GameObject[] statChange;
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
            matName = new string[] { "Skin", "Eyes", "Mouth", "Hair", "Clothes", "Armour" };
            selectedClass = new string[] { "Stealth", "Tank", "Hunter", "SprintyBoi", "Mage" };

            #region TemporayNameAssignMentForStats
            characterStats[0].baseStatsName = "Charisma";
            characterStats[1].baseStatsName = "Intelligence";
            characterStats[2].baseStatsName = "Strength";
            characterStats[3].baseStatsName = "Dexterity";
            characterStats[4].baseStatsName = "Constitution";
            characterStats[5].baseStatsName = "Agility";
            #endregion

            #region For loops for assigning textures
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
            if (index < 0)
            {
                index = max - 1;
            }
            if (index > max - 1)
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

        public enum CharacterClass
        {
            Stealth,
            Tank,
            Hunter,
            SprintyBoi,
            Mage,
        }

        /// <summary>
        /// Used to assign the characters stat. <br/>
        /// this method handles incrememting the points <br/>
        /// the player will not be able to assign more then 20 points <br/>
        /// to the character
        /// </summary>
        /// <param name="points">how much to increment by</param>
        /// <param name="ID">the index of the stat</param>
        public void AssignStats(int points, int ID)
        {
            characterStats[ID].tempStats += points;
            
            // make sure the player cant add more then
            // the max value of the base and temp stat combined
            int baseStat = characterStats[ID].baseStats;
            int tempStat = characterStats[ID].tempStats;

            int maxValue = 20 - baseStat;
            
            if (tempStat >= maxValue) characterStats[ID].tempStats = maxValue;
            if(tempStat <= maxValue) statPoints -= points;

            // make sure that the player cant assign negative temp stat points
            if (tempStat <= 0) characterStats[ID].tempStats = 0;
            #region StatPoints
            if (statPoints <= 0) statPoints = 0;
            if (statPoints >= 10) statPoints = 10;
            #endregion
            
            // check if the player has used all the stats they have avaliable
            if (statPoints <= 0) allStatsAssigned = true;
            else if (statPoints >= 1) allStatsAssigned = false;
        }   


        /// <summary>
        /// Temporay GUI used for customising the characters look
        /// </summary>
        private void OnGUI()
        {
            #region Redundant
            if (runInEditor)
            {
                //var nativeSize = new Vector2(res.x, res.y);
                //
                //Vector3 scale = new Vector3(Screen.width / nativeSize.x, Screen.height / nativeSize.y, 1.0f);
                //var matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, scale);
                #endregion
                Vector2 res = new Vector2(1920, 1080);
                GUI.matrix = IMGUIUtils.IMGUIMatrix(res);

                #region Styles
                GUIStyle _style = new GUIStyle(GUI.skin.box);
                _style.fontSize = 50;
                _style.alignment = TextAnchor.UpperCenter;
                #endregion
                #region Group Variables
                Vector2 groupOneLayoutPosition = layouts[0].position;
                Vector2 groupOneLayoutSize = layouts[0].size;

                Vector2 groupTwoLayoutPostion = layouts[1].position;
                Vector2 groupTwoLayoutSize = layouts[1].size;
                
                Vector2 buttonPos = layouts[3].position;
                Vector2 buttonSize = layouts[3].size;
                Vector2 _buttonPos = layouts[4].position;
                Vector2 _buttonSize = layouts[4].size;
                #endregion
                #region Box and Button Variables
                Vector2 _position = layouts[2].position;
                Vector2 _size = layouts[2].size;
                #endregion
                #region Rectangles
                Rect groupOne = new Rect(groupOneLayoutPosition.x, groupOneLayoutPosition.y, groupOneLayoutSize.x, groupOneLayoutSize.y);
                Rect groupOneBox = new Rect(0, 0, groupOneLayoutSize.x, groupOneLayoutSize.y);
                Rect groupTwo = new Rect(groupTwoLayoutPostion.x, groupTwoLayoutPostion.y, groupTwoLayoutSize.x, groupTwoLayoutSize.y);
                Rect groupTwoBox = new Rect(0, 0, groupTwoLayoutSize.x, groupTwoLayoutSize.y);

                Rect dropDownButtons = new Rect(layouts[6].position.x, layouts[6].position.y, layouts[6].size.x, layouts[6].size.y);
                #endregion
                #region Styles
                GUIStyle matNameStyle = new GUIStyle(GUI.skin.box);
                GUIStyle textFieldStyle = new GUIStyle(GUI.skin.box);
                matNameStyle.fontSize = 25;
                textFieldStyle.fontSize = 25;
                matNameStyle.alignment = TextAnchor.MiddleCenter;
                #endregion
                #region TextureCustomisation
                GUI.BeginGroup(groupOne);
                GUI.Box(groupOneBox, "Textures", _style);

                for(int i = 0; i < matName.Length; i++)
                {
                    #region Variables
                    var x = (i * 55);
                    Rect buttonRectLeft = new Rect(buttonPos.x, buttonPos.y + x, buttonSize.x, buttonSize.y);
                    Rect buttonRectRight = new Rect(_buttonPos.x, _buttonPos.y + x, _buttonSize.x, _buttonSize.y);
                    #endregion
                    GUI.Box(new Rect(_position.x, _position.y + x, _size.x, _size.y), matName[i], matNameStyle);
                    if(GUI.Button(buttonRectLeft, ">", matNameStyle))
                    {
                        SetTexture(matName[i], 1);
                    }   
                    if(GUI.Button(buttonRectRight, "<", matNameStyle))
                    {
                        SetTexture(matName[i], -1);
                    }
                }
                GUI.EndGroup();
                #endregion
                #region GroupTwo
                Rect button3 = new Rect(layouts[15].position.x, layouts[15].position.y, layouts[15].size.x, layouts[15].size.y);
                GUI.BeginGroup(groupTwo);
                GUI.Box(groupTwoBox, "Class and Stats", _style);
                if(GUI.Button(button3, "Swap Menu", matNameStyle)) statsScreen = !statsScreen;
                if (!statsScreen)
                {
                    #region ScrollViewPositionAndScale
                    Rect scrolview = new Rect(layouts[7].position.x, layouts[7].position.y, layouts[7].size.x, layouts[7].size.y);
                    Rect viewRect = new Rect(layouts[8].position.x, layouts[8].position.y, layouts[8].size.x, layouts[8].size.y);
                    #endregion
                    if (GUI.Button(dropDownButtons, classButton, matNameStyle)) showDropdown = !showDropdown;
                    if (showDropdown)
                    {
                        scrollPos = GUI.BeginScrollView(scrolview, scrollPos, viewRect, false, true);
                        for (int i = 0; i < selectedClass.Length; i++)
                        {
                            #region
                            Rect x = new Rect(layouts[10].position.x, layouts[10].position.y * i, layouts[10].size.x, layouts[10].size.y);
                            #endregion
                            if (GUI.Button(x, selectedClass[i], matNameStyle))
                            {
                                ChooseClass(i);
                                classButton = selectedClass[i];
                                Debug.Log(i.ToString());
                            }                  
                        }
                        GUI.EndScrollView();
                    }
                    characterName = GUI.TextField(new Rect(layouts[5].position.x, layouts[5].position.y, layouts[5].size.x, layouts[5].size.y), characterName, textFieldStyle);
                }
                if (statsScreen)
                {
                    #region
                    Rect _box = new Rect(layouts[16].position.x, layouts[16].position.y, layouts[16].size.x, layouts[16].size.y);
                    GUI.Box(_box, string.Format("points : {0}", statPoints), matNameStyle);
                    Rect groupSize = new Rect(layouts[12].position.x, layouts[12].position.y, layouts[12].size.x, layouts[12].size.y);
                    #endregion
                    GUI.BeginGroup(groupSize);
                    for (int i = 0; i < characterStats.Length; i++)
                    {
                        #region 
                        string statName = string.Format("{0} : {1}",characterStats[i].baseStatsName, (characterStats[i].baseStats + characterStats[i].tempStats));
                        Rect button1 = new Rect(layouts[13].position.x, layouts[13].position.y * i, layouts[13].size.x, layouts[13].size.y);
                        Rect button2 = new Rect(layouts[14].position.x, layouts[14].position.y * i, layouts[14].size.x, layouts[14].size.y);
                        Rect BoxPosSize = new Rect(layouts[11].position.x, layouts[11].position.y * i, layouts[11].size.x, layouts[11].size.y);
                        #endregion
                        GUI.Box(BoxPosSize, statName, matNameStyle);
                        if(GUI.Button(button1, "+", matNameStyle)) if(!allStatsAssigned) AssignStats(1, i);
                        if(GUI.Button(button2, "-", matNameStyle)) AssignStats(-1, i);
                    }
                    GUI.EndGroup();
                }
                GUI.EndGroup();
                #endregion 
            }
        }
    }
}


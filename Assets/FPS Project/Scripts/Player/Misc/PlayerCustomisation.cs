using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine.UI;
using UnityEngine;
using TMPro;


namespace FPSProject.Customisation
{
    public class PlayerCustomisation : SerializedMonoBehaviour
    {
        #region Variables
        [SerializeField] private CustomisationManager cManager;
        #region Textures
        [Title("Texture list and prefab")] // lists for holding all the textures for the different parts of the character
        [Tooltip("Scriptable objects handling all textures")]public Textures[] textures = new Textures[6];
        [SerializeField] private GameObject prefab;
        [SerializeField] private Transform content;
        #endregion
        #region misc
        [Title("texture Index")] // indexs for changing what position in an array
        public int index; 
        [Title("Renderer")]
        public Renderer characterRenderer;
        [Title("Material names")]  // ids for the materials
        public string[] matName = new string[6];
        #endregion
        #endregion
        #region Methods

        private void Awake()
        {
            ResetIndexesOnStart();
            Debugging.DisableOnStart();
        }
        private void Start()
        {
            SpawnTextureButtons();
            // Old Code Does nothing now

            #region For loops for assigning textures

            //for (int i = 0; i < skinMax; i++)       // loops until i is no longer less then defined value
            //{
            //    Texture2D tempTexture = Resources.Load("Character/Skin_" + i) as Texture2D;     // looks through the resources folder for the skin texture
            //    skin.Add(tempTexture);                                                          // assigns the texture to the array
            //}
            //for (int i = 0; i < eyesMax; i++)       // loops until i is no longer less then defined value
            //{
            //    Texture2D tempTexture = Resources.Load("Character/Eyes_" + i) as Texture2D;     // looks through the resources folder for the eyes texture
            //    eyes.Add(tempTexture);                                                          // assigns the texture to the array
            //}
            //for (int i = 0; i < mouthMax; i++)      // loops until i is no longer less then defined value
            //{
            //    Texture2D tempTexture = Resources.Load("Character/Mouth_" + i) as Texture2D;    // looks through the resources folder for the mouth texture
            //    mouth.Add(tempTexture);                                                         // assigns the texture to the array
            //}
            //for (int i = 0; i < hairMax; i++)       // loops until i is no longer less then defined value
            //{
            //    Texture2D tempTexture = Resources.Load("Character/Hair_" + i) as Texture2D;     // looks through the resources folder for the hair texture
            //    hair.Add(tempTexture);                                                          // assigns the texture to the array
            //}
            //for (int i = 0; i < armourMax; i++)     // loops until i is no longer less then defined value
            //{
            //    Texture2D tempTexture = Resources.Load("Character/Armour_" + i) as Texture2D;   // looks through the resources folder for the armour texture
            //    armour.Add(tempTexture);                                                        // assigns the texture to the array
            //}
            //for (int i = 0; i < clothesMax; i++)    // loops until i is no longer less then defined value
            //{
            //    Texture2D tempTexture = Resources.Load("Character/Clothes_" + i) as Texture2D;  // looks through the resources folder for the clothes texture
            //    clothes.Add(tempTexture);                                                       // assigns the texture to the array
            //}

            #endregion

        }

        #region Old Code

        /// <summary>
        /// Creates an array of objects loaded from the resource folder <br/>
        /// loops through the objects and checks the name of each obj <br/> 
        /// matches the object to the corrosponding list then converts to texture2D <br/>
        /// and adds to that list
        /// </summary>
        //private void LoadTextures()
        //{
        //    object[] obj = Resources.LoadAll("Character");
        //    foreach(object ob in obj)
        //    {
        //        string texture = ob.ToString(); // turns the ob into a string for checking

        //        // checks if the string contains any of these words
        //        if (texture.Contains("Skin")) skin.Add(ob as Texture2D);
        //        if (texture.Contains("Eye")) eyes.Add(ob as Texture2D);
        //        if (texture.Contains("Mouth")) mouth.Add(ob as Texture2D);
        //        if (texture.Contains("Hair")) hair.Add(ob as Texture2D);
        //        if (texture.Contains("Armour")) armour.Add(ob as Texture2D);
        //        if (texture.Contains("Clothes")) clothes.Add(ob as Texture2D);
        //        // then adds the textures to the corrosponding list
        //    }
        //}

        #endregion

        #region Main Methods
        /// <summary>
        /// spawns all the buttons for changing the textures
        /// </summary>
        private void SpawnTextureButtons()
        {
            for (var i = 0; i < textures.Length; i++)
            {
                int copy = i;
                var obj = Instantiate(prefab, content).transform;
                obj.Find("Name").GetComponent<TextMeshProUGUI>().text = textures[i].name;
                
                obj.Find("Right").GetComponent<Button>().onClick.AddListener(()=> SetTexture(copy, 1));
                obj.Find("Left").GetComponent<Button>().onClick.AddListener(()=> SetTexture(copy, -1));
            }
        }
        /// <summary>
        /// Temporay method for resetting the index values of the textures
        /// </summary>
        private void ResetIndexesOnStart()
        {
            for (int i = 0; i < textures.Length; i++)
            {
                Textures _textureType = textures[i];
                _textureType.index = 0; 
                SetTexture(i, 0);
            }

        }
        /// <summary>
        /// Used to let the player change the texture of there character so they 
        /// <br/> can customise the look of the character to their desire
        /// </summary>
        /// <param name="type">The ID of the texture being changed</param>
        /// <param name="dir">The direction in the array we are going</param>
        public void SetTexture(int type, int dir)
        {
            // hard code for readability
            Textures _textureType = textures[type];
            int _maxIndex = _textureType.textureList.Count - 1;     // max index value for each texture list
            int _matIndex = type + 1;                               // material index

            _textureType.index += dir;                              // increments the index value

            // checks to make sure the lists arent given negative values or go out of bounds of the list
            if (_textureType.index < 0) _textureType.index = _maxIndex;
            if (_textureType.index > _maxIndex) _textureType.index = 0;

            // sets the material and texture values
            Material[] mat = characterRenderer.materials;
            mat[_matIndex].mainTexture = _textureType.textureList[_textureType.index];
            characterRenderer.materials = mat;
            #region Old code
            //// ints for the index matIndex and max value
            //int index = 0, max = 0, matIndex = 0;
            //Texture2D[] textures = new Texture2D[0];    // list of textures

            //#region Switch 1
            //// used to get and assign the textures
            //// and the indexs of the defined items
            //switch (type)
            //{
            //    #region Skin
            //    case "Skin":                    // the Id of the item we want to change
            //    index = skinIndex;              // sets the index to match the items index
            //    max = skinMax;                  // sets the max to the items max
            //    textures = skin.ToArray();      // gets the textures from items list and converts to array
            //    matIndex = 1;                   // sets what material we are changing textures on
            //    break;

            //    #endregion
            //    #region Eyes
            //    case "Eyes":
            //    index = eyesIndex;
            //    max = eyesMax;
            //    textures = eyes.ToArray();
            //    matIndex = 2;
            //    break;
            //    #endregion
            //    #region Mouth
            //    case "Mouth":
            //    index = mouthIndex;
            //    max = mouthMax;
            //    textures = mouth.ToArray();
            //    matIndex = 3;
            //    break;
            //    #endregion
            //    #region Hair
            //    case "Hair":
            //    index = hairIndex;
            //    max = hairMax;
            //    textures = hair.ToArray();
            //    matIndex = 4;
            //    break;
            //    #endregion
            //    #region Clothes
            //    case "Clothes":
            //    index = clothesIndex;
            //    max = clothesMax;
            //    textures = clothes.ToArray();
            //    matIndex = 5;
            //    break;
            //    #endregion
            //    #region Armour
            //    case "Armour":
            //    index = armourIndex;
            //    max = armourMax;
            //    textures = armour.ToArray();
            //    matIndex = 6;
            //    break;
            //    #endregion
            //}
            //#endregion

            //index += dir; // adds to the index from the number given when the method is called
            //if (index < 0)
            //{
            //    index = max - 1;
            //}
            //if (index > max - 1)
            //{
            //    index = 0;
            //}

            // gets the material
            //Material[] mat = characterRenderer.materials;   // makes an array of materials
            //mat[matIndex].mainTexture = textures[index];    // gets the materials
            //characterRenderer.materials = mat;              // assigns what material we are changing 

            //#region Switch 2
            //// handles changing the index values
            //switch (type)
            //{
            //    case "Skin":
            //    skinIndex = index;      // assigns the choosen item's index to the current value of the index
            //    break;
            //    case "Eyes":
            //    eyesIndex = index;
            //    break;
            //    case "Mouth":
            //    mouthIndex = index;
            //    break;
            //    case "Hair":
            //    hairIndex = index;
            //    break;
            //    case "Clothes":
            //    clothesIndex = index;
            //    break;
            //    case "Armour":
            //    armourIndex = index;
            //    break;
            //}
            #endregion
        }
        #endregion
        #endregion
    }
}

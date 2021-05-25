using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using Sirenix.OdinInspector;
using System;


namespace FPSProject.Customisation
{
    public class PlayerCustomisation : SerializedMonoBehaviour
    {
        [SerializeField] private CustomisationManager cManager;
        #region Texture Lists
        [Title("Texture Lists")] // lists for holding all the textures for the different parts of the character
        public List<Texture2D> skin = new List<Texture2D>();
        public List<Texture2D> eyes = new List<Texture2D>();
        public List<Texture2D> mouth = new List<Texture2D>();
        public List<Texture2D> hair = new List<Texture2D>();
        public List<Texture2D> armour = new List<Texture2D>();
        public List<Texture2D> clothes = new List<Texture2D>();
        #endregion
        #region misc
        [Title("Current texture Index")] // indexs for changing what position in an array
        public int index = 0; 
        public int skinIndex, eyesIndex, mouthIndex, hairIndex, armourIndex, clothesIndex;
        [Title("Renderer")]
        public Renderer characterRenderer;
        [Title("Max amount of textures per type")] // max values so we dont go outside of the array bounds
        public int skinMax;
        public int eyesMax, mouthMax, hairMax, armourMax, clothesMax;
        [Title("Material names")]  // ids for the materials
        public string[] matName = new string[6];
        #endregion

        #region Methods
        public void Start()
        {
            LoadTextures();
            
            matName = new string[]
            { "Skin", "Eyes", "Mouth", 
            "Hair", "Clothes", "Armour" };

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
        /// <summary>
        /// Creates an array of objects loaded from the resource folder <br/>
        /// loops through the objects and checks the name of each obj <br/> 
        /// matches the object to the corrosponding list then converts to texture2D <br/>
        /// and adds to that list
        /// </summary>
        private void LoadTextures()
        {
            object[] obj = Resources.LoadAll("Character");
            foreach(object ob in obj)
            {
                string texture = ob.ToString(); // turns the ob into a string for checking

                // checks if the string contains any of these words
                if (texture.Contains("Skin")) skin.Add(ob as Texture2D);
                if (texture.Contains("Eye")) eyes.Add(ob as Texture2D);
                if (texture.Contains("Mouth")) mouth.Add(ob as Texture2D);
                if (texture.Contains("Hair")) hair.Add(ob as Texture2D);
                if (texture.Contains("Armour")) armour.Add(ob as Texture2D);
                if (texture.Contains("Clothes")) clothes.Add(ob as Texture2D);
                // then adds the textures to the corrosponding list
            }
        }
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
        #endregion
    }
}

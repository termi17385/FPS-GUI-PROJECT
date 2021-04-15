using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;

/*
 * FixLog
 * 
 * need to fix OnGui Elements
 * for resolution and display 
 * 
 * fix text for OnGui
 * 
 */

namespace FPSProject.Menu
{
    [HideMonoScript]
    public class MenuManager : SerializedMonoBehaviour
    {

        #region Structs and properties
        /// <summary>
        /// displays the slider value as a number between 0 - 100
        /// </summary>
        public float MusicSliderValDisplay
        {
            get => Mathf.Round((musicSliderVal + 60) / 80 * 100);
        }
        /// <summary>
        /// displays the slider value as a number between 0 - 100
        /// </summary>
        public float SfxSliderValDisplay
        {
            get => Mathf.Round((sfxSliderVal + 60) / 80 * 100);
        }

        #region Debugging
        #region MainMenu
        [System.Serializable]
        struct OnGUILayout
        {
            [Space]
            public string name;
            [Space]
            public float posX;
            public float posY;
            [Space]
            public float sizeX;
            public float sizeY;
        }
        #endregion
        #endregion
        #endregion
        #region Variables
        [SerializeField] private AudioMixer mixer;
        [SerializeField] private AudioSource sfx;
        [SerializeField] private AudioSource music;
        
        [SerializeField] private bool fullscreen;
        [SerializeField] private Resolution[] resolutionArray; 
        [SerializeField] private float tempResWidth;
        [SerializeField] private float tempResHeight;
        [SerializeField] private List<string> options = new List<string>(); 
        
        private int resolutionIndex;
        public float resSpacing;
        public float scrolSpacing;

        [SerializeField, ReadOnly]
        private bool playPressed = false;
        [SerializeField, ReadOnly]
        private bool optionsMenu = false;

        [SerializeField] private float musicSliderVal;
        [SerializeField] private float sfxSliderVal;
        
        #region Debug Variables
        static bool debugMode = false;

        private bool graphicsDropDown = false;
        private bool resDropDown = false;
        private Vector2 _scrollView = Vector2.zero;
        private Vector2 nativeSize;

        #region Position and Size
        [TitleGroup("DebugGUI")]
        //[HorizontalGroup("DebugGUI/Split", Width = 0.5f)]
        [SerializeField]
        [TabGroup("DebugGUI/Parameters", "Variables")] 
        private float posX, posY;

        [SerializeField]
        [TabGroup("DebugGUI/Parameters", "Variables")] 
        private float sizeX, sizeY;
        #endregion
        #region Structs
        [SerializeField, TabGroup("DebugGUI/Parameters", "Struct")] 
        private OnGUILayout[] layoutOptions;
        private int _ID = 0;
        #endregion
        #region GUI Styling
        private GUIContent content;
        private GUIStyle style = new GUIStyle();
        private GUIStyle _style = new GUIStyle();
        [SerializeField] int textSize;
        [SerializeField] int _textSize;

        [SerializeField]
        private int selectionGridInt = 0;
        private string[] selectionStrings = { "Audio", "Graphics", "Display", "KeyBindings" };
        #endregion
        #endregion
        #endregion

        // Start is called before the first frame update
        void Start()
        {
            #region DebugStuff
            content = new GUIContent("MainMenu"); 
            style.alignment = TextAnchor.MiddleCenter;

            #if UNITY_EDITOR
            debugMode = true;
            #else
            debugMode = false;
            #endif

            #endregion
            playPressed = false;

            GetResolutions();
        }

        // Update is called once per frame
        void Update()
        {
            #region Debugging
            EnableDebugMode();
            SetFullScreen();
            #endregion
        }

        #region Settings
        #region Display
        /// <summary>
        /// sets whether the game is in fullscreen or not
        /// </summary>
        public void SetFullScreen()
        {
            Screen.fullScreen = fullscreen;
        }
        #endregion
        #region Resolution
        private void GetResolutions()
        {
            resolutionArray = Screen.resolutions;           // stores all the resolutions in the array
            
            
            // loop to assign all resolutions to a string 
            // set the current resolution
            for (int i = 0; i < resolutionArray.Length; i++)
            { 
                // formats the resolution into a user friendly format then adds that the the list of options
                string option = resolutionArray[i].width + "x" + resolutionArray[i].height;
                options.Add(option);

                #region Debugging
                // used to check if the resolutions are being logged properly
                Debug.Log(option);
                Debug.Log(options[i]);
                #endregion

                // checks to see if we are at the current resolution
                if (resolutionArray[i].width == Screen.currentResolution.width &&
                resolutionArray[i].height == Screen.currentResolution.height)
                {
                    // then sets the index to that value
                    resolutionIndex = i;
                }
            }
        }

        private void SetResolution(int resolutionIndex)
        {
            Resolution res = resolutionArray[resolutionIndex];
            Screen.SetResolution(res.width, res.height, fullscreen, 0);
        }
        #endregion
        #region Audio
        /// <summary>
        /// Handles changing the music volume
        /// </summary>
        /// <param name="vol">the parametre for<br/>changing the volume</param>
        public void MusicSlider(float vol)
        {
            mixer.SetFloat("Music", vol);
        }

        /// <summary>
        /// Handles changing the SFX volume
        /// </summary>
        /// <param name="vol">the parametre for<br/>changing the volume</param>
        public void SFXSlider(float vol)
        {
            mixer.SetFloat("SFX", vol);
        }
        #endregion

        #endregion

        public void QuitGame()
        {
            #if UNITY_EDITOR                        // checks if we are in the Unity Editior
            EditorApplication.ExitPlaymode();       // if true then exit playmode
            #endif                                  // end if

            Application.Quit();                     // Quits application if we are in build
        }

        #region Debugging
        public static void EnableDebugMode()
        {
            bool enable = Input.GetKeyDown(KeyCode.F2);
            if (enable == true)
            {
                debugMode = !debugMode;
            }
        }

        private void OnGUI()
        {
            debugMode = true;
            if (debugMode == true)
            {
                // keeps everything scaled to the native size
                nativeSize = new Vector2(tempResWidth, tempResHeight);                                          // used to set the native size of the image
                Vector3 scale = new Vector3(Screen.width / nativeSize.x, Screen.height / nativeSize.y, 1.0f);   // gets the scale of the screen
                GUI.matrix = Matrix4x4.TRS(new Vector3(0, 0, 0), Quaternion.identity, scale);                   // sets the matrix and scales the GUI accordingly

                // scales text with the native size
                GUIStyle style = new GUIStyle();
                style.fontSize = (int)(textSize * ((float) Screen.width / (float)nativeSize.x));    // resizes the font
                style.alignment = TextAnchor.UpperCenter;                                           // anchors the font to a specific part of the screen

                string[] names = {"PlayGame", "Options", "QuitToDesktop",
                "Resume", "NewGame", "LoadGame"}; // names to be assigned to the buttons

                #region Main Menu Debug Menu
                if (optionsMenu == false)
                {
                    // creates a title box for the main menu
                    GUI.Box(new Rect(layoutOptions[6].posX, layoutOptions[6].posY,
                    layoutOptions[6].sizeX, layoutOptions[6].sizeY), layoutOptions[6].name = "Main Menu", style);

                    // backPlate for the MainMenu
                    GUI.Box(new Rect(layoutOptions[6].posX, layoutOptions[6].posY,
                    layoutOptions[6].sizeX, layoutOptions[6].sizeY), "");

                    // create a set of buttons for the default menu
                    for (int i = 0; i  < 3; i++)
                    {
                        if(GUI.Button(new Rect(layoutOptions[i].posX, layoutOptions[i ].posY, 
                        layoutOptions[i].sizeX, layoutOptions[i].sizeY), layoutOptions[i].name = names[i]))
                        {
                            sfx.Play();
                            _ID = (i + 1);  // sets the ID of the button
                        }  
                    }
                    // create a set of buttons for when play is pressed
                    if (playPressed)
                    {
                        for(int i = 3; i < 6; i++)
                        {
                            if(GUI.Button(new Rect(layoutOptions[i].posX, layoutOptions[i].posY, 
                            layoutOptions[i].sizeX, layoutOptions[i].sizeY), layoutOptions[i].name = names[i]))
                            {
                                sfx.Play();
                                _ID = (i + 1);
                            }
                        }
                    }


                    // assign buttons with an ID to be used with the switch to call specific functions
                    // a switch for the button ids to determine what gets run
                    switch (_ID)
                    {
                        case 1: // goes to the play options
                        Debug.Log("Play");
                        playPressed = !playPressed;
                        _ID = 0;
                        break;
                
                        case 2: // Goes to the options Menu
                        Debug.Log("Options");
                        optionsMenu = !optionsMenu;
                        _ID = 0;
                        break;

                        case 3: 
                        // Quits application (unless in editior which case it exits playmode)
                        Debug.Log("Quit");
                        QuitGame();
                        _ID = 0;
                        break;

                        case 4:
                        Debug.Log("Resume");
                        SceneManager.LoadScene(2);
                        _ID = 0;
                        break;

                        case 5:
                        Debug.Log("New Game");
                        SceneManager.LoadScene(1);
                        _ID = 0;
                        break;

                        case 6:
                        Debug.Log("What Level Do You Want To Load?...");
                        _ID = 0;
                        break;
                    }
                }

                #endregion
                #region OptionsMenu
                if (optionsMenu == true)
                {
                    // scales text with the native size
                    GUIStyle _style = new GUIStyle();
                    GUIStyle audioTextStyle = new GUIStyle();
                    
                    _style.fontSize = (int)(25.0f * ((float)Screen.width / (float)nativeSize.x));    // resizes the font
                    _style.alignment = TextAnchor.UpperCenter;

                    audioTextStyle.fontSize = _textSize;

                    float layoutSizeX = layoutOptions[7].sizeX;
                    float layoutSizeY = layoutOptions[7].sizeY;

                    float layoutPosX = layoutOptions[7].posX;
                    float layoutPosY = layoutOptions[7].posY;

                    float posX = 25f;
                    float posY = 0;

                    float pX = posX;
                    float pY = posY;


                    float sX = sizeX;
                    float sY = sizeY;



                    //GUI.Box(new Rect(layoutPosX, layoutPosY, layoutSizeX, layoutSizeY), layoutOptions[7].name = "Options", _style);

                    #region VolumeDisplay
                    float musicVolume;
                    float sfxVolume;

                    float musicPercentage = MusicSliderValDisplay;
                    float sfxPercentage = SfxSliderValDisplay;

                    mixer.GetFloat("Music", out musicVolume);
                    mixer.GetFloat("SFX", out sfxVolume);

                    string audioContent = string.Format("Music \nmixer:{0} \npercentage:{2} \n \nSFX \nmixer:{1} \npercentage: {3}",
                    musicVolume, sfxVolume, musicPercentage, sfxPercentage);
                    #endregion

                    GUI.BeginGroup(new Rect(layoutPosX, layoutPosY, layoutSizeX, layoutSizeY));

                    GUI.Box(new Rect(0, 0, layoutSizeX, layoutSizeY), "");

                    if (GUI.Button(new Rect(layoutOptions[8].posX, layoutOptions[8].posY, 
                    layoutOptions[8].sizeX, layoutOptions[8].sizeY), layoutOptions[8].name = "Back"))
                    {
                        Debug.Log("Back");
                        sfx.Play();
                        graphicsDropDown = false;
                        resDropDown = false;
                        optionsMenu = !optionsMenu;
                    }

                    // wrap all items inside a layout
                    GUILayout.BeginArea(new Rect(0, posY, layoutSizeX, layoutSizeY));
                    GUILayout.BeginHorizontal();

                    selectionGridInt = GUILayout.SelectionGrid(selectionGridInt, selectionStrings, 2);
                    if (GUI.changed) // checks if any button in the selection grid was pressed
                    {
                        sfx.Play(); // plays sound

                        // disables all dropdowns
                        graphicsDropDown = false;
                        resDropDown = false;
                    }

                    GUILayout.EndHorizontal();
                    GUILayout.EndArea();

                    GUILayout.BeginArea(new Rect(0, (posY + 50f), layoutSizeX, layoutSizeY));
                    GUILayout.BeginVertical();

                    switch (selectionGridInt)
                    {
                        #region Audio
                        case 0:
                        GUILayout.Box("SFX");   // displays text for SFX
                        sfxSliderVal = GUILayout.HorizontalSlider(sfxSliderVal, -60.0f, 20.0f);       // slider for controlling sound effects volume
                        SFXSlider(sfxSliderVal);

                        GUILayout.Box("Music"); // displays text for Music
                        musicSliderVal = GUILayout.HorizontalSlider(musicSliderVal, -60.0f, 20.0f); // slider for controlling music volume
                        MusicSlider(musicSliderVal);

                        GUI.BeginGroup(new Rect(0, 0, layoutSizeX, layoutSizeY)); // a group for holding text for music and sfx volume display
                        GUI.Box(new Rect(0.1f, 100, 1f, 1f), audioContent, audioTextStyle);   // displays volume for sfx and music
                        GUI.EndGroup();

                        break;
                        #endregion

                        #region Graphics
                        case 1:
                        GUI.BeginGroup(new Rect(0, 0, sX, sY));
                        if (GUI.Button(new Rect(50, 0, 120, 20), "GraphicsDropDown"))
                        {
                            sfx.Play();
                            graphicsDropDown = !graphicsDropDown;
                        }
                        if (graphicsDropDown)
                        {
                            _scrollView = GUI.BeginScrollView(new Rect(60, 30, 100, 100), _scrollView, new Rect(0, 0, 0, 4 * (0.5f)), false, true);
                            if (GUI.Button(new Rect(25, 0, 50, 20), "1"))
                                sfx.Play();
                            if (GUI.Button(new Rect(25, 40, 50, 20), "2"))
                                sfx.Play();
                            if (GUI.Button(new Rect(25, 80, 50, 20), "3"))
                                sfx.Play();
                            if (GUI.Button(new Rect(25, 120, 50, 20), "4"))
                                sfx.Play();
                            GUI.EndScrollView();
                        }                                   // creates a scrollview for displaying a drop down of graphic options
                        GUI.EndGroup();
                        break;
                        #endregion

                        #region DisplaySettings
                        case 2:
                        GUI.BeginGroup(new Rect(0, 0, sX, sY));
                        if (GUI.Button(new Rect(50, 0, 120, 20), "ResolutionDropDown"))
                        {
                            sfx.Play(); // plays a sound effect when this button is pressed
                            resDropDown = !resDropDown;  // enables or disables the dropdown
                        }
                        if (resDropDown)
                        {
                            float scrollSize = 4.93f;
                            _scrollView = GUI.BeginScrollView(new Rect(60, 30, 100, 100), _scrollView, new Rect(0, 0, 0, scrollSize), false, true);
                            for (int i = 0; i < resolutionArray.Length; i++)
                            {
                                #region Size and Spacing
                                float buttonSpacing = (i * 0.45f);
                                float buttonWidth = 1.33f;
                                float buttonHeight = 0.38f;
                                #endregion
                                if (GUI.Button(new Rect(0f, buttonSpacing, buttonWidth, buttonHeight), options[i]))
                                {

                                }
                            }
                            GUI.EndScrollView();
                        }   // creates a scrollview for displaying a drop down of Resolution options
                        else
                        {
                            // toggles fullscreen when the toggle is pressed
                            fullscreen = GUI.Toggle(new Rect(60, 40, 70, 15), fullscreen, "fullscreen");
                        }
                        GUI.EndGroup();
                        break;
                        #endregion

                        #region KeyBinds
                        case 3: //KeyBindings

                        break;
                        #endregion
                    }

                    #region EndStuff
                    GUILayout.EndVertical();
                    GUILayout.EndArea();
                    GUI.EndGroup();
                    #endregion
                    #endregion
                }
            }
        }
        #endregion
    }
}

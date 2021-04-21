using System.Collections;
using FPSProject.Keybinds;
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

        #region Percentage Display for audio
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
        #endregion

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
        // audio Variables
        [SerializeField] private AudioSource sfx;
        [SerializeField] private AudioSource music;
        [SerializeField] private AudioMixer mixer;
        
        [SerializeField] private float musicSliderVal;
        [SerializeField] private float sfxSliderVal;

        // display Variables
        [SerializeField] private bool fullscreen;
        [SerializeField] private List<string> options = new List<string>();
        private Resolution[] resolutionArray; 
        private int resolutionIndex;

        // graphics Variables
        [SerializeField] private string[] graphicalNames;

        public float resSpacing;
        public float scrolSpacing;

        [SerializeField, ReadOnly]
        private bool playPressed = false;
        [SerializeField, ReadOnly]
        private bool optionsMenu = false;

        
        #region Debug Variables
        // universal bool for debugMode
        static bool debugMode = false;
        
        // bools for dropdowns
        private bool graphicsDropDown = false;
        private bool resDropDown = false;

        // vectors for scrollview and native size
        private Vector2 _scrollView = Vector2.zero;
        private Vector2 nativeSize;

        private float resWidth = 1280;
        private float resHeight = 720;

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
        #region KeyBindings IMGUI
        [SerializeField] private string bindingToMap;
        [SerializeField] private string buttonText;
        [SerializeField] private string[] _buttonText;
        [SerializeField] private string[] bindingMap;
        [SerializeField] private string bindingName;

        private bool isRebinding = false;
        #endregion
        #endregion
        #endregion

        private void Awake()
        {
#if UNITY_EDITOR
            debugMode = true;
#else
            debugMode = false;
#endif
        }
        void Start()
        {
            #region DebugStuff
            if (string.IsNullOrEmpty(bindingToMap))
            {
                return;
            }

            Setup(bindingToMap);

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
            GetAndSetGraphics();
        }

        // Update is called once per frame
        void Update()
        {
            #region Debugging
            EnableDebugMode();
            SetFullScreen();
            if (debugMode)
            {
                if (isRebinding)
                {
                    KeyCode pressed = BindingUtils.GetAnyPressedKey();
                    if (pressed != KeyCode.None)
                    {
                        BindingManager.Rebind(bindingToMap, pressed);
                        BindingUtils.UpdateTextWithBinding(bindingToMap, buttonText);

                        isRebinding = false;
                    }
                }
            }
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
                //Debug.Log(option);
                //Debug.Log(options[i]);
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
        #region Graphics
        private void GetAndSetGraphics()
        {
            graphicalNames = QualitySettings.names;
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

        private void Setup(string _toMap)
        {
            bindingToMap = _toMap;

            bindingName = _toMap;

            BindingUtils.UpdateTextWithBinding(bindingToMap, buttonText);
            gameObject.SetActive(true);
        }

        private void OnClick()
        {
            isRebinding = true;
        }

        private void OnGUI()
        {
            if (debugMode == true)
            {
                // keeps everything scaled to the native size
                nativeSize = new Vector2(resWidth, resHeight);                                          // used to set the native size of the image
                Vector3 scale = new Vector3(Screen.width / nativeSize.x, Screen.height / nativeSize.y, 1.0f);   // gets the scale of the screen
                GUI.matrix = Matrix4x4.TRS(new Vector3(0, 0, 0), Quaternion.identity, scale);                   // sets the matrix and scales the GUI accordingly

                                                                
                GUIStyle style = new GUIStyle();
                style.fontSize = textSize;                      // resizes the font
                style.alignment = TextAnchor.UpperCenter;       // anchors the font to a specific part of the screen

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

                    audioTextStyle.fontSize = _textSize;            // handles the style and size of the audio text

                    #region position and size
                    // handles the position and size of the layout group
                    float layoutSizeX = layoutOptions[7].sizeX;
                    float layoutSizeY = layoutOptions[7].sizeY;

                    float layoutPosX = layoutOptions[7].posX;
                    float layoutPosY = layoutOptions[7].posY;
                    #endregion

                    //GUI.Box(new Rect(layoutPosX, layoutPosY, layoutSizeX, layoutSizeY), layoutOptions[7].name = "Options", _style);

                    #region VolumeDisplay
                    // floats for music and sfx
                    float musicVolume;
                    float sfxVolume;

                    // handles displaying the percentages for music and sfx
                    float musicPercentage = MusicSliderValDisplay;
                    float sfxPercentage = SfxSliderValDisplay;

                    // gets the values from the audio mixer
                    mixer.GetFloat("Music", out musicVolume);
                    mixer.GetFloat("SFX", out sfxVolume);

                    // formats a string for displaying mixer and percentages values on screen
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
                        sfxSliderVal = GUILayout.HorizontalSlider(sfxSliderVal, -60.0f, 20.0f);         // slider for controlling sound effects volume
                        SFXSlider(sfxSliderVal);

                        GUILayout.Box("Music"); // displays text for Music
                        musicSliderVal = GUILayout.HorizontalSlider(musicSliderVal, -60.0f, 20.0f);     // slider for controlling music volume
                        MusicSlider(musicSliderVal);

                        GUI.BeginGroup(new Rect(0, 0, layoutSizeX, layoutSizeY)); // a group for holding text for music and sfx volume display
                        GUI.Box(new Rect(0.1f, 100, 1f, 1f), audioContent, audioTextStyle);   // displays volume for sfx and music
                        GUI.EndGroup();

                        break;
                        #endregion

                        #region Graphics
                        case 1:
                        GetAndSetGraphics();
                        GUI.BeginGroup(new Rect(0, 0, layoutOptions[9].sizeX, layoutOptions[9].sizeY));
                        if (GUI.Button(new Rect(layoutOptions[10].posX, layoutOptions[10].posY, layoutOptions[10].sizeX, layoutOptions[10].sizeY), "Graphics"))
                        {
                            sfx.Play(); // plays a sound effect when this button is pressed
                            GetResolutions();
                            graphicsDropDown = !graphicsDropDown;  // enables or disables the dropdown
                        }
                        if (graphicsDropDown)
                        {
                            float scrollSize = layoutOptions[12].posX;
                            _scrollView = GUI.BeginScrollView(new Rect(80, 50, 100, 150), _scrollView, new Rect(0, 0, 0, scrollSize), false, true);
                            for (int i = 0; i < graphicalNames.Length; i++)
                            {
                                #region Size and Spacing
                                float buttonSpacing = (i * layoutOptions[11].posY);
                                float buttonWidth = layoutOptions[11].sizeX;
                                float buttonHeight = layoutOptions[11].sizeY;
                                #endregion
                                if (GUI.Button(new Rect(0f, buttonSpacing, buttonWidth, buttonHeight), graphicalNames[i]))
                                {
                                    sfx.Play();
                                    QualitySettings.SetQualityLevel(i, true);
                                }
                            }
                            GUI.EndScrollView();
                        }                                   // creates a scrollview for displaying a drop down of graphic options
                        GUI.EndGroup();
                        break;
                        #endregion

                        #region DisplaySettings
                        case 2:
                        Debug.Log("Display");
                        GUI.BeginGroup(new Rect(0, 0, layoutOptions[9].sizeX, layoutOptions[9].sizeY));
                        if (GUI.Button(new Rect(layoutOptions[10].posX, layoutOptions[10].posY, layoutOptions[10].sizeX, layoutOptions[10].sizeY), "ResolutionDropDown"))
                        {
                            sfx.Play(); // plays a sound effect when this button is pressed
                            GetResolutions();
                            resDropDown = !resDropDown;  // enables or disables the dropdown
                        }
                        if (resDropDown)
                        {
                            float scrollSize = layoutOptions[11].posX;
                            _scrollView = GUI.BeginScrollView(new Rect(80, 50, 100, 150), _scrollView, new Rect(0, 0, 0, scrollSize), false, true);

                            // creates a set of buttons in the dropdown for displaying resolution options for the player
                            for (int i = 0; i < resolutionArray.Length; i++)
                            {
                                #region Size and Spacing
                                float buttonSpacing = (i * layoutOptions[11].posY);
                                float buttonWidth = layoutOptions[11].sizeX;
                                float buttonHeight = layoutOptions[11].sizeY;
                                #endregion

                                if (GUI.Button(new Rect(0f, buttonSpacing, buttonWidth, buttonHeight), options[i]))
                                {
                                    sfx.Play();
                                    SetResolution(i);
                                }
                            }
                            GUI.EndScrollView();
                        } 
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
                        // check if button is pressed
                        // check what key is pressed after
                        // set that key
                        // save
                        GUI.BeginGroup(new Rect(0, 0, layoutOptions[9].sizeX, layoutOptions[9].sizeY));
                        for(int i = 0; i < _buttonText.Length; i++)
                        {
                            #region Size and Spacing
                            float buttonSpacing = (i * layoutOptions[13].posY);
                            float buttonSpacingX = layoutOptions[13].posX;
                            float buttonWidth = layoutOptions[13].sizeX;
                            float buttonHeight = layoutOptions[13].sizeY;
                            #endregion

                            if (GUI.Button(new Rect(buttonSpacingX, buttonSpacing,
                            buttonWidth, buttonHeight), _buttonText[i]))
                            {
                                OnClick();
                                bindingToMap = bindingMap[i];

                            }
                        }
                        GUI.EndGroup();
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

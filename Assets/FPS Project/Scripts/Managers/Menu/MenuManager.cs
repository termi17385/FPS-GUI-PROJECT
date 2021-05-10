using UnityEngine.SceneManagement;
using FPSProject.Menu.Animations;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using FPSProject.Keybinds;
using System.Collections;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine;
using UnityEditor;
using TMPro;
 
/* FixLog
 * 
 * need to fix OnGui Elements - error 1 (fixed)
 * for resolution and display 
 * 
 * fixed made changed the  - error 1
   imgui matrix to account 
   for screen scaling
 * 
 * fix text for OnGui
 * 
 *  converted Fullscreen and audio to canvas
 *  need to fix resolution and convert to canvas
 *  overhaul player prefs
 *  convert the rest to canvas
 *  
 *  fixed the resolution dup issues by clearing 
 *  the list each time the method is called
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
            [Space, FoldoutGroup("Struct")]
            public string name;
            [Space, FoldoutGroup("Struct")]
            public float posX;
            [FoldoutGroup("Struct")]
            public float posY;
            [Space, FoldoutGroup("Struct")]
            public float sizeX;
            [FoldoutGroup("Struct")]
            public float sizeY;
        }
        #endregion
        #endregion
        #endregion
        #region Variables
        //[SerializeField]
        //private Button[] buttons = new Button[8];
        #region OdinStuff
        [TitleGroup("SettingsVariables")]
        [HorizontalGroup("SettingsVariables/Split")]
        [VerticalGroup("SettingsVariables/Split/Left")]
        #endregion
        #region AudioVariables
        // audio Variables
        [BoxGroup("SettingsVariables/Split/Left/Audio"), SerializeField] private AudioSource sfx;
        [BoxGroup("SettingsVariables/Split/Left/Audio"), SerializeField] private AudioSource music;
        [BoxGroup("SettingsVariables/Split/Left/Audio"), SerializeField] private AudioMixer mixer;
        
        [BoxGroup("SettingsVariables/Split/Left/Audio"), SerializeField] private TextMeshProUGUI musicText;
        [BoxGroup("SettingsVariables/Split/Left/Audio"), SerializeField] private TextMeshProUGUI sfxText;

        [BoxGroup("SettingsVariables/Split/Left/AudioSliders")] public float musicSliderVal;
        [BoxGroup("SettingsVariables/Split/Left/AudioSliders")] public float sfxSliderVal;
        [SerializeField] private Slider _music;
        [SerializeField] private Slider _sfx;
        #endregion
        #region DisplayVariables
        // display Variables
        [VerticalGroup("SettingsVariables/Split/Right")]

        [BoxGroup("SettingsVariables/Split/Right/Display"), SerializeField] private List<string> options = new List<string>();
        [BoxGroup("SettingsVariables/Split/Right/Display"), SerializeField] private Resolution[] resolutionArray;
        [BoxGroup("SettingsVariables/Split/Right/Display"), SerializeField] private TMP_Dropdown resDown;

        [BoxGroup("SettingsVariables/Split/Right/Display"), SerializeField] private bool fullscreen;

        [BoxGroup("SettingsVariables/Split/Right/Display"), SerializeField] private int refreshRate;
        [BoxGroup("SettingsVariables/Split/Right/Display"), SerializeField] private int resolutionIndex;
        #endregion
        #region Graphics Variables
        // graphics Variables
        [BoxGroup("SettingsVariables/Split/Right/Graphics"),SerializeField] private string[] graphicalNames;
        [BoxGroup("SettingsVariables/Split/Left/Graphics"), SerializeField] private int graphicsIndex;
        [BoxGroup("SettingsVariables/Split/Left/Graphics"), SerializeField] private TMP_Dropdown grapDown;
        #endregion
        #region Misc
        [SerializeField, ReadOnly]
        private bool playPressed = false;
        [SerializeField, ReadOnly]
        private bool optionsMenu = false;

        public GameObject[] menus;
        #endregion
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
        //[HorizontalGroup("DebugGUI/Split", Width = 0.5f)]
        [TitleGroup("DebugGUI")]
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
        [BoxGroup("DebugGUI/Variables")]
        [SerializeField] int textSize;
        [BoxGroup("DebugGUI/Variables")]
        [SerializeField] int _textSize;

        [BoxGroup("DebugGUI/Variables")]
        [SerializeField]
        private int selectionGridInt = 0;
        private string[] selectionStrings = { "Audio", "Graphics", "Display", "KeyBindings" };
        #endregion
        #region KeyBindings IMGUI
        [BoxGroup("DebugGUI/Keybinding")]
        [SerializeField] private string bindingToMap;
        [BoxGroup("DebugGUI/Keybinding")]
        [SerializeField] private string buttonText;
        [BoxGroup("DebugGUI/Keybinding")]
        [SerializeField] private string[] _buttonText;
        [BoxGroup("DebugGUI/Keybinding")]
        [SerializeField] private string[] bindingMap;
        [BoxGroup("DebugGUI/Keybinding")]
        [SerializeField] private string bindingName;

        private bool isRebinding = false;
        #endregion
        #endregion
        #endregion

        #region Start and Update
        private void Awake()
        {
            GetResolutions();
            GetGraphics();
            
            debugMode = false;
            DropDownListener();
            LoadSettings();
        }

        private void Start()
        {
            menus[0].SetActive(true);
            menus[1].SetActive(false);
            #region Initialising Settings
            #endregion
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
#endif

            #endregion
            playPressed = false;
        }

        // Update is called once per frame
        private void Update()
        {
            #region Methods
            EnableDebugMode();
            DisplayVolumeText();
            #endregion
            #region Debugging
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
            //MenuAnimationManager.menuAnimMan.DisplaySubMenu();
        }
        #endregion
        #region Settings
        public void PlayerPressed()
        {
            if(MenuAnimationManager.menuAnimMan.canInteract == true)
            {
                if(!playPressed) MenuAnimationManager.menuAnimMan.DisplaySubMenu();
                else MenuAnimationManager.menuAnimMan.CloseSubMenu();
                playPressed = !playPressed;
            }
        }

        #region Display
        /// <summary>
        /// sets whether the game is in fullscreen or not
        /// </summary>
        public void SetFullScreen(bool _fullScreen)
        {
            fullscreen = _fullScreen;
            Screen.fullScreen = fullscreen;
        }
        #endregion
        #region Resolution
        public void GetResolutions()
        {
            options.Clear();  // makes sure the list is cleared beforehand
            List<Resolution> resolutions = new List<Resolution>();  // a list for storing the resolutions
            foreach (Resolution res in Screen.resolutions)    
            {
                // checks if the refreshrate is 60 then adds resolution to list
                if (res.refreshRate == 60.0f)    
                    resolutions.Add(res);
                
                //stores the resolutions in an array
                resolutionArray = resolutions.ToArray();
            }
            
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

            resDown.ClearOptions();
            resDown.AddOptions(options);
        }
        #region ListenerStuff
        /// <summary>
        /// Used to set listeners for the dropDowns
        /// <br/> to detect when the player changes Values
        /// </summary>
        public void DropDownListener()
        {
            // add listener for the dropdown to detect when a value is changed
            resDown.onValueChanged.AddListener(delegate{OnResolutionChange(resDown);});
            grapDown.onValueChanged.AddListener(delegate{SetGraphics(grapDown.value);});

            // used to debug the drop downs
            Debug.LogWarning("First : " + grapDown.value);
            Debug.LogWarning("First : " + resDown.value);
        }
        /// <summary>
        /// used to change the resolution
        /// </summary>
        public void OnResolutionChange(TMP_Dropdown change)
        {
            // sets resolution when the dropdown is changed
            SetResolution(change.value);

            string _text = string.Format("Value : {0}", change.value);
            Debug.LogWarning(_text);
        }
        #endregion
        /// <summary>
        /// Sets the resolution based on the index given
        /// </summary>
        /// <param name="resolutionIndex">A value for changing the resolution</param>
        public void SetResolution(int resolutionIndex)
        {
            Resolution res = resolutionArray[resolutionIndex];
            Screen.SetResolution(res.width, res.height, fullscreen, refreshRate);
        }
        #endregion
        #region Graphics
        private void GetGraphics()
        {
            List<string> _graphicalNames = new List<string>();  // creates a list for displaying the graphics
            graphicalNames = QualitySettings.names;             // assigns the names to an array of string
            _graphicalNames.Clear();                            // makes sure the list is clean
            
            // gets all the names from the array and converts to list
            foreach(string name in graphicalNames)               
            {_graphicalNames.Add(name);}

            grapDown.ClearOptions();                            // makes sure the drop down is cleaned first
            grapDown.AddOptions(_graphicalNames);               // assigns the names to the dropDown
        }
        /// <summary>
        /// Sets the graphics settings of the game
        /// </summary>
        /// <param name="i">the index</param>
        public void SetGraphics(int i)
        {
            QualitySettings.SetQualityLevel(i, true);
        }
        #endregion
        #region Audio
        /// <summary>
        /// Handles changing the music volume
        /// </summary>
        /// <param name="vol">the parametre for<br/>changing the volume</param>
        public void MusicSlider(float vol)
        {
            musicSliderVal = vol;
            mixer.SetFloat("Music", musicSliderVal);
        }

        /// <summary>
        /// Handles changing the SFX volume
        /// </summary>
        /// <param name="vol">the parametre for<br/>changing the volume</param>
        public void SFXSlider(float vol)
        {
            sfxSliderVal = vol;  // stores the float for the slider val so i can be converted to text
            mixer.SetFloat("SFX", sfxSliderVal);
        }

        /// <summary>
        /// Displays the volume amount as a string
        /// </summary>
        private void DisplayVolumeText()
        {
            musicText.text = string.Format("Music : {0}", MusicSliderValDisplay); 
            sfxText.text = string.Format("SFX : {0}", SfxSliderValDisplay); 
        }

        /// <summary>
        /// Plays sfx when buttons are pressed
        /// </summary>
        private void PlayButtonSFX()
        {
            sfx.Play();
        }
        #endregion
        #region Save and Loading
        public void SaveSettings()
        {
            #region Fullscreen
            PlayerPrefs.SetString("FullScreen", fullscreen.ToString()); // saves fullscreen bool as a string
            #endregion
            #region Indexes
            PlayerPrefs.SetInt("ResIndex", resDown.value);
            PlayerPrefs.SetInt("GraphicsIndex", grapDown.value);
            #endregion
            #region Audio
            // save audio
            float musicVol;
            mixer.GetFloat("Music", out musicVol);
            PlayerPrefs.SetFloat("MusicVol", musicVol);
            
            float sfxVol;
            mixer.GetFloat("SFX", out sfxVol);
            PlayerPrefs.SetFloat("SFXVol", sfxVol);
            #endregion


            // saves player prefs to disk
            PlayerPrefs.Save();
        }

        public void LoadSettings()
        {
            #region Fullscreen
            // Loading Fullscreen    
            string _fullscreen = PlayerPrefs.GetString("FullScreen"); // loads the string
            fullscreen = bool.Parse(_fullscreen); // then converts to bool
            #endregion
            #region Indexes
            resDown.value = PlayerPrefs.GetInt("ResIndex");
            grapDown.value = PlayerPrefs.GetInt("GraphicsIndex");

            SetResolution(resDown.value);
            SetGraphics(grapDown.value);

            string debugTest = string.Format("\n Res : {0} \n Graph : {1}", resDown.value, grapDown.value);

            Debug.Log(debugTest);
            #endregion
            #region Audio
            musicSliderVal = PlayerPrefs.GetFloat("MusicVol");
            sfxSliderVal = PlayerPrefs.GetFloat("SFXVol");

            // makes sure the mixer is set on load
            mixer.SetFloat("Music", musicSliderVal);
            mixer.SetFloat("SFX", sfxSliderVal);

            _music.value = musicSliderVal;
            _sfx.value = sfxSliderVal;
            #endregion
        }
        #endregion
        #endregion
        #region Misc
        /// <summary>
        /// Quits the application or exits play mode
        /// <br/>depending on if in editor or in build
        /// </summary>
        public void QuitGame()
        {
            #if UNITY_EDITOR                        // checks if we are in the Unity Editior
            EditorApplication.ExitPlaymode();       // if true then exit playmode
            #endif                                  // end if

            Application.Quit();                     // Quits application if we are in build
        }
        public void LoadGameScene()
        {
            PlayerPrefs.SetString("SceneName", "GameScene");
            SceneManager.LoadScene("LoadingScreen");
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
                        LoadSettings();
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
                        GetGraphics();
                        GUI.BeginGroup(new Rect(0, 0, layoutOptions[9].sizeX, layoutOptions[9].sizeY));
                        if (GUI.Button(new Rect(layoutOptions[10].posX, layoutOptions[10].posY, layoutOptions[10].sizeX, layoutOptions[10].sizeY), "Graphics"))
                        {
                            sfx.Play(); // plays a sound effect when this button is pressed
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
                                    graphicsIndex = i;
                                    SetGraphics(graphicsIndex);
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
        #endregion
    }
}

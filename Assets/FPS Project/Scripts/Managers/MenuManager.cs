using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;

namespace FPSProject.Menu
{
    [HideMonoScript]
    public class MenuManager : SerializedMonoBehaviour
    {

        #region Structs and properties
        #region Debugging
        #region MainMenu
        struct Buttons
        {
            public int buttonID;
            public string buttonName;
        }
        struct GUIShape
        {
            // size of the object
            // the position of the object
            [ReadOnly]
            public string name;
            [Space]
            public float sizeX;
            public float sizeY;
            [Space]
            public float posX;
            public float posY;
        }
        struct GUIShapeTwo
        {
            // size of the object
            // the position of the object
            [ReadOnly]
            public string name;
            [Space]
            public float sizeX;
            public float sizeY;
            [Space]
            public float posX;
            public float posY;
        }
        #endregion
        
        struct LayoutPositionAndSize
        {
            public float posX;
            public float posY;
            [Space]
            public float sizeX;
            public float sizeY;
        }
        #endregion
        #endregion
        #region Variables
        [SerializeField] private AudioSource sfx;

        [SerializeField, ReadOnly]
        private bool playPressed = false;
        [SerializeField, ReadOnly]
        private bool optionsMenu = false;
        
        #region Debug Variables
        static bool debugMode = false;

        private bool graphicsDropDown = false;
        private bool resDropDown = false;
        private Vector2 _scrollView = Vector2.zero;

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
        private Buttons[] buttons;
        private int _ID = 0;

        [SerializeField, TabGroup("DebugGUI/Parameters", "ShapeStruct")]
        private GUIShape[] shape;
        [SerializeField, TabGroup("DebugGUI/Parameters", "ShapeStruct")]
        private GUIShapeTwo[] _shape;
        [SerializeField, TabGroup("DebugGUI/Parameters", "ShapeStruct")]
        private LayoutPositionAndSize layout;

        #endregion
        #region GUI Styling
        private GUIContent content;
        private GUIStyle style = new GUIStyle();
        [SerializeField] int textSize;

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
        }

        // Update is called once per frame
        void Update()
        {
            debugMode = true;
            EnableDebugMode();
        }


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
            if (debugMode == true)
            {
                #region Values
                // sets the screen size to 16 : 9
                Vector2 scr = new Vector2 (Screen.width / 16, Screen.height / 9);
                #region Position
                // sets the position values
                float _posX = posX * scr.x;
                float _posY = posY * scr.y;
                #endregion
                #region Size
                // sets the size values
                float _sizeX = sizeX * scr.x;
                float _sizeY = sizeY * scr.y;
                #endregion
                // changes the size of the text 
                style.fontSize = textSize;
                #endregion
                #region Main Menu Debug Menu
                if (optionsMenu == false)
                {
                    // displays a box with text for the menu title
                    GUI.Box(new Rect(_posX, _posY, _sizeX, _sizeY), content, style);
                    // a loop for creating a set of buttons for play options and quit
                    for (int i = 0; i < 3; i++)
                {
                    if (GUI.Button(new Rect(shape[i].posX * scr.x, shape[i].posY * scr.y,
                    shape[i].sizeX * scr.x, shape[i].sizeY * scr.y), buttons[i].buttonName))
                    {
                        sfx.Play();
                        _ID = buttons[i].buttonID;        
                    }
                }
                    // a loop for a set of buttons that are displayed when play is pressed
                    if (playPressed == true)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        if(GUI.Button(new Rect(_shape[i].posX * scr.x, _shape[i].posY * scr.y,
                        _shape[i].sizeX * scr.x, _shape[i].sizeY * scr.y), _shape[i].name))
                        {
                            sfx.Play();
                            _ID = 4 + i;
                        }
                    }
                }
            
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

                    case 3: // Quits or exitsPlaymode
                    Debug.Log("Quit");
                
    #if UNITY_EDITOR
                    EditorApplication.ExitPlaymode();
    #endif
                
                    Application.Quit();
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
                    GUI.Box(new Rect(_posX, _posY, _sizeX, _sizeY), "Options", style);
                
                    float pX = layout.posX * scr.x; 
                    float pY = layout.posY * scr.y; 
                
                    float sX = layout.sizeX * scr.x;
                    float sY = layout.sizeY * scr.y; 

                    float buttonPosX = 0.7f * scr.x;
                    float buttonPosY = 3.5f * scr.y;

                    float buttonSizeX = 1.5f * scr.x;
                    float buttonSizeY = 0.5f * scr.y;

                   

                    GUI.BeginGroup(new Rect(pX, pY, sX, sY));

                    GUI.Box(new Rect(0, 0, sX, sY), "");
                    if (GUI.Button(new Rect(buttonPosX, buttonPosY, buttonSizeX, buttonSizeY), "Back"))
                    {
                        Debug.Log("Back");
                        sfx.Play();
                        graphicsDropDown = false;
                        resDropDown = false;
                        optionsMenu = !optionsMenu;
                    }

                    // wrap all items inside a layout
                    GUILayout.BeginArea (new Rect (0, 0.1f * scr.y, sX, sY));
                    GUILayout.BeginHorizontal();
                    
                    selectionGridInt = GUILayout.SelectionGrid(selectionGridInt, selectionStrings, 2);
                    if(GUI.changed)
                    {
                        sfx.Play(); 
                        graphicsDropDown = false;
                        resDropDown = false; 
                    }
                    
                    GUILayout.EndHorizontal();
                    GUILayout.EndArea();

                    GUILayout.BeginArea(new Rect(0, 1f * scr.y, sX, sY));
                    GUILayout.BeginVertical();

                    switch (selectionGridInt)
                    {
                        #region Audio
                        case 0:
                        GUILayout.Box("SFX");
                        GUILayout.HorizontalSlider(0, 0.0f, 100);

                        GUILayout.Box("Music");
                        GUILayout.HorizontalSlider(0, 0.0f, 100);
                        break;
                        #endregion

                        #region Graphics
                        case 1:
                        GUI.BeginGroup(new Rect(0, 0, sX, sY));                          
                        if(GUI.Button(new Rect(50, 0, 120, 20), "GraphicsDropDown"))
                        {sfx.Play(); graphicsDropDown = !graphicsDropDown;}
                        if (graphicsDropDown)
                        {
                            _scrollView = GUI.BeginScrollView(new Rect(60, 30, 100, 100), _scrollView, new Rect(0, 0, 0, 4 * (0.5f * scr.y)),false,true);
                            if(GUI.Button(new Rect(25, 0, 50, 20), "1"))sfx.Play();
                            if(GUI.Button(new Rect(25, 40, 50, 20), "2"))sfx.Play();
                            if(GUI.Button(new Rect(25, 80, 50, 20), "3"))sfx.Play();
                            if(GUI.Button(new Rect(25, 120, 50, 20), "4"))sfx.Play();
                            GUI.EndScrollView();
                        }                                   // creates a scrollview for displaying a drop down of graphic options
                        GUI.EndGroup();
                        break;
                        #endregion

                        #region DisplaySettings
                        case 2:
                        GUI.BeginGroup(new Rect(0, 0, sX, sY));
                        if (GUI.Button(new Rect(50, 0, 120, 20), "ResolutionDropDown"))
                        {sfx.Play(); resDropDown = !resDropDown;}
                        if (resDropDown)
                        {
                            _scrollView = GUI.BeginScrollView(new Rect(60, 30, 100, 100), _scrollView, new Rect(0, 0, 0, 4 * (0.5f * scr.y)), false, true);
                            if (GUI.Button(new Rect(25, 0, 50, 20), "1"))
                                sfx.Play();
                            if (GUI.Button(new Rect(25, 40, 50, 20), "2"))
                                sfx.Play();
                            if (GUI.Button(new Rect(25, 80, 50, 20), "3"))
                                sfx.Play();
                            if (GUI.Button(new Rect(25, 120, 50, 20), "4"))
                                sfx.Play();
                            GUI.EndScrollView();
                        }                                   // creates a scrollview for displaying a drop down of Resolution options
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
    }
}

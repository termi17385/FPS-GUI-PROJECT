using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections; 
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine;
using System;
using System.Linq;
using TMPro;

/* ChangeLog
 * 
 * Need to give all buttons sfx when pressed
 * need to set up loading bar 
 */

namespace FPSProject.Menu
{
    public class PauseMenu : MonoBehaviour
    {
        #region Variables and Properties
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
        #region Lists and Arrays
        [SerializeField] private List<string> options = new List<string>();
        [SerializeField] private Resolution[] resolutionArray;
        [SerializeField] private string[] graphicalNames;
        #endregion
        #region DropDowns
        [SerializeField] private TMP_Dropdown resDropDown;
        [SerializeField] private TMP_Dropdown graphicsDropDown;
        #endregion
        #region Bools
        [SerializeField] private bool fullscreen;
        #endregion
        #region intergers
        [SerializeField] private int refreshRate;
        #endregion
        #region indexes
        [SerializeField] private int resolutionIndex = 0;
        #endregion
        #region Audio Stuff
        #region Sounds
        [SerializeField] private AudioMixer mixer;

        [SerializeField] private Slider musicSlider;
        [SerializeField] private Slider sfxSlider;

        [SerializeField] private AudioSource sfxSound;
        [SerializeField] private AudioSource musicSound;
        #endregion
        #region Text
        [SerializeField] private TextMeshProUGUI musicText;
        [SerializeField] private TextMeshProUGUI sfxText;
        #endregion
        #region floats
        [SerializeField] private float musicSliderVal;
        [SerializeField] private float sfxSliderVal;
        #endregion
        #endregion
        #region Misc Variables
        [NonSerialized] public static PauseMenu instance;
        [SerializeField] private GameObject[] menus = new GameObject[3];
        public bool paused;
        public bool death;
        private string sceneName = "LoadingScreen";
        #endregion
        #endregion

        #region Start, Update and menu methods
        void Awake()
        {
            death = false;
            GetResolutions();
            SetupListeners();
            GetGraphics();

            if(instance == null)instance = this;
            else Destroy(gameObject);

            LoadSettings();
        }

        void Start()
        {
            // disabling all menus at start 
            //foreach(GameObject menu in menus) 
            //menu.SetActive(false); 

            // makes sure the game isnt paused at start
            paused = false;
        }

        private void Update() => DisplayVolumeText();
        
        public void PauseGame()
        {
            paused = !paused;

            // sets timeScale depending if the game is paused or unpaused
            if(paused) 
            {
                Time.timeScale = 0f;                        // pauses time
                menus[0].SetActive(true);                   // enables the paused menu
                Cursor.lockState = CursorLockMode.None;     // brings back the cursor
                LoadSettings();
            }
            else 
            {
                Time.timeScale = 1f;
                menus[1].SetActive(true);
                menus[0].SetActive(false);
                menus[2].SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
                SaveSettings();
            }
        }

        /// <summary>
        /// Swaps between menus depending on which one is active
        /// </summary>
        public void SwapMenus()
        {
            if(menus[1].activeSelf == true)
            {
                menus[1].SetActive(false);
                menus[2].SetActive(true);
            }
            else
            {
                menus[1].SetActive(true);
                menus[2].SetActive(false);
                SaveSettings();
            }
        }
        #endregion
        #region Listener
        private void SetupListeners()
        {
            // add listeners for dropdowns to detect when a value is changed
            resDropDown.onValueChanged.AddListener(delegate
                {onResolutionChange(resDropDown); 
                    OnButtonPressPlaySound();});
            
            graphicsDropDown.onValueChanged.AddListener(delegate
                {SetGraphics(graphicsDropDown.value); 
                    OnButtonPressPlaySound();});
        }

        private void onResolutionChange(TMP_Dropdown change)
        {
            resolutionIndex = change.value;
            SetResolution(resolutionIndex);

            sfxSound.Play();
        }
        #endregion
        #region Get resolution and graphics
        /// <summary>
        /// Used to get the resolutions store them in an array and list <br/>
        /// then convert them to user friendly text to display on the dropdown
        /// </summary>
        public void GetResolutions()
        {
            options.Clear();  // makes sure the list is cleared beforehand
            List<Resolution> resolutions = new List<Resolution>();  // a list for storing the resolutions
            foreach (Resolution res in Screen.resolutions)
            {
                // checks if the refreshrate is 60 then adds resolution to list
                if (res.refreshRate == 60.0f || res.refreshRate == 120.0f)
                    resolutions.Add(res);
            }

            //stores the resolutions in an array and filters out the duplicates
            resolutionArray = Screen.resolutions.Select(resolutions => new Resolution { width = resolutions.width, height = resolutions.height }).Distinct().ToArray();


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

            resDropDown.ClearOptions();
            resDropDown.AddOptions(options);
        }
        /// <summary>
        /// Used to get the graphics settings and displays them on the drop down
        /// </summary>
        private void GetGraphics()
        {
            List<string> _graphicalNames = new List<string>();  // creates a list for displaying the graphics
            graphicalNames = QualitySettings.names;             // assigns the names to an array of string
            _graphicalNames.Clear();                            // makes sure the list is clean

            // gets all the names from the array and converts to list
            foreach (string name in graphicalNames)
            {
                _graphicalNames.Add(name);
            }

            graphicsDropDown.ClearOptions();                            // makes sure the drop down is cleaned first
            graphicsDropDown.AddOptions(_graphicalNames);               // assigns the names to the dropDown
        }
        #endregion
        #region Set resolution and graphics
        public void SetResolution(int index)
        {
            Resolution res = resolutionArray[resolutionIndex];
            Screen.SetResolution(res.width, res.height, fullscreen, refreshRate);
        }

        public void SetGraphics(int i)
        {
            QualitySettings.SetQualityLevel(i, true);
        }

        public void OnButtonPressPlaySound()
        {
            sfxSound.Play();
        }
        #endregion
        #region AudioSliders and Display
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
        #endregion
        #region Save and Loading
        public void SaveSettings()
        {
            #region Fullscreen
            // saves fullscreen bool as a string
            PlayerPrefs.SetString("FullScreen", fullscreen.ToString()); 
            #endregion
            #region Indexes
            // saves the indexes of the drop downs
            PlayerPrefs.SetInt("ResIndex", resDropDown.value);
            PlayerPrefs.SetInt("GraphicsIndex", graphicsDropDown.value);
            #endregion
            #region Audio
            // Saves audio values
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
            // loads the drop downs
            resDropDown.value = PlayerPrefs.GetInt("ResIndex");
            graphicsDropDown.value = PlayerPrefs.GetInt("GraphicsIndex");

            // loads resolution and quality settings
            SetResolution(resDropDown.value);
            SetGraphics(graphicsDropDown.value);

            // makes sure that the drop downs are loaded with the correct values
            string debugTest = $"\n Res : {resDropDown.value} \n Graph : {graphicsDropDown.value}";

            Debug.Log(debugTest);
            #endregion
            #region Audio
            // loads the audio slider values
            musicSliderVal = PlayerPrefs.GetFloat("MusicVol"); 
            sfxSliderVal = PlayerPrefs.GetFloat("SFXVol");

            // makes sure the mixer is set on load
            mixer.SetFloat("Music", musicSliderVal);
            mixer.SetFloat("SFX", sfxSliderVal);

            // makes sure that the sliders are displayed properly 
            // on game load
            musicSlider.value = musicSliderVal;
            sfxSlider.value = sfxSliderVal;
            #endregion
        }
        #endregion
        #region Misc
        public void SetFullScreen(bool _fullscreen)
        {
            fullscreen = _fullscreen;
            Screen.fullScreen = fullscreen;
        }       

        public void QuitToMenu()
        {
            PlayerPrefs.SetString("SceneName", "MainMenu");
            SceneManager.LoadScene(sceneName); 
        }
        #endregion
    }
}
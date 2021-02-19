using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEditor;

public class MenuManager : MonoBehaviour
{
    #region
    #region SettingVariables 
    [Header("Resolution Settings")]
    public Resolution[] resolutions;            // an array for storing all the resolutions
    public Dropdown resolution;                 // dropdown for showing resolution settings

    [Header("Audio Settings")]
    public AudioMixer mixer;                    // this is for the main audio mixer so we can change volume ingame
    public Slider musicVolume;                  // this is so we can control the Music Volume
    public Slider sfxVolume;                    // this is so we can control the Sfx Volume

    [Header("Quality and Fullscreen")]
    public Dropdown qualityDropdown;            // dropdown for guality settings
    public Toggle fullscreenToggle;             // fullscreen Toggle
    [Space]

    public GameObject disable;
    #endregion

    #region Scenes
    [Header("Scenes we want to load")]
    [SerializeField] private string[] loadScene;
    #endregion
    #endregion

    private void Awake()
    {
        VideoSettings();
    }

    #region Settings
    /// <summary>
    /// Method for saving the video settings
    /// </summary>
    public void VideoSettings()
    {
        #region Resolution
        // makes an array of the different resolutions
        resolutions = Screen.resolutions;                       // gets and stores all the resolutions
        resolution.ClearOptions();                              // clears the dropdown menu
        List<string> options = new List<string>();              // creates a new list of options

        int currentResolutionIndex = 0;                         // an index for the list
        // go through every resolution
        for (int i = 0; i < resolutions.Length; i++)                                // creates a for loop setting i to zero and loops each time until the condition is met 
        {
            // Build a string for displaying the resolution
            string option = resolutions[i].width + "x" + resolutions[i].height;     // displays the resolutions in a user friendly format
            options.Add(option);                                                    // adds the new option

            // checks what the current resolution is
            if (resolutions[i].width == Screen.currentResolution.width &&
            resolutions[i].height == Screen.currentResolution.height)
            {
                // if we have found the current resolution, save that number
                currentResolutionIndex = i;
            }
        } 

        #region DropDown
        // Sets up our dropDown
        resolution.AddOptions(options);                     // adds options to the drop down from the list
        resolution.value = currentResolutionIndex;          // sets the value to the current resolution
        resolution.RefreshShownValue();                     // refresh the dropdown values
        #endregion
        #endregion

        #region Fullscreen and Quality
        // Fullscreen Checking
        if (!PlayerPrefs.HasKey("fullscreen"))      // checking if fullscreen is saved or not
        {
            // if no then set fullscreen disabled by defualt
            PlayerPrefs.SetInt("fullscreen", 0);
            Screen.fullScreen = false;
        }
        else
        {
            if (PlayerPrefs.GetInt("fullscreen") == 0)  // if fullscreen is not on (0)
            {
                Screen.fullScreen = false;              // disable fullscreen
            }
            else
            {
                Screen.fullScreen = true;               // enable fullscreen
            }
        }

        // Quality Checking 
        if (!PlayerPrefs.HasKey("quality"))         // checks if the quality settings are saved
        {
            PlayerPrefs.SetInt("quality", 3);       // sets the settings
            QualitySettings.SetQualityLevel(3);
        }
        else
        {
            QualitySettings.SetQualityLevel(PlayerPrefs.GetInt("quality"));
        }
        #endregion

        PlayerPrefs.Save();
    }
    #region Set Values
    public void SetResolution(int ResolutionIndex)
    {
        Resolution res = resolutions[ResolutionIndex];
        Screen.SetResolution(res.width, res.height, Screen.fullScreen);
    }

    /// <summary>
    ///  sets the fullscreen on or off
    /// </summary>
    public void SetFullscreen(bool fullscreen)
    {
        // sets the scene to full screen when called
        Screen.fullScreen = fullscreen;
    }
    /// <summary>
    /// method for changing the quality
    /// </summary>
    public void ChangeQuality(int index)
    {
        // sets the quality to a choosen int when called
        QualitySettings.SetQualityLevel(index);
    }
    /// <summary>
    /// method for setting the music values
    /// </summary>
    public void SetMusicVolume(float value)
    {
        mixer.SetFloat("Music", value);
    }
    /// <summary>
    /// method for setting the SFX values
    /// </summary>
    public void SetSFXVolume(float value)
    {
        mixer.SetFloat("SFX", value);
    }
    #endregion
    #endregion

    #region Saving and Loading
    /// <summary>
    /// Method for saving the players preferences
    /// </summary>
    public void SavePrefs()
    {
        PlayerPrefs.SetInt("quality", QualitySettings.GetQualityLevel());  // sets the values of the quality setting in the player prefs

        // sets if fullscreen is on or off in the player prefs
        if (fullscreenToggle.isOn)
        {
            PlayerPrefs.SetInt("fullscreen", 1);
        }
        else
        {
            PlayerPrefs.SetInt("fullscreen", 0);
        }

        float musicVol;                                 // float for containing the musicVolume values
        if (mixer.GetFloat("Music", out musicVol))      // checks for a value change 
        {
            PlayerPrefs.SetFloat("Music", musicVol);    // then sets the volume values
        }

        float sfxVol;                                   // float for containing the musicVolume values
        if (mixer.GetFloat("SFX", out sfxVol))          // checks for a value change 
        {
            PlayerPrefs.SetFloat("SFX", sfxVol);        // then sets the volume values
        }

        PlayerPrefs.Save();
    }
    /// <summary>
    /// Method for loading the players preferences
    /// </summary>
    public void LoadPrefs()
    {
        // loads the values for the quality settings 
        qualityDropdown.value = PlayerPrefs.GetInt("quality");

        // loads the fullscreen values
        if (PlayerPrefs.GetInt("fullscreen") == 0)
        {
            fullscreenToggle.isOn = false;
        }
        else
        {
            fullscreenToggle.isOn = true;
        }

        float musicVol = PlayerPrefs.GetFloat("Music");     // loads the volume values
        musicVolume.value = musicVol;                       // sets the volume
        mixer.SetFloat("Music", musicVol);                  // sets the mixer volume

        float sfxVol = PlayerPrefs.GetFloat("SFX");         // loads the volume values
        sfxVolume.value = sfxVol;                           // sets the volume
        mixer.SetFloat("SFX", sfxVol);                      // sets the mixer volume
    }
    #endregion

    public void QuitGame()
    {
        #if UNITY_EDITOR
        EditorApplication.ExitPlaymode();   // exits playmode when using editor
        #endif

        Application.Quit();                 // quits the game to desktop when in build mode
    }
}

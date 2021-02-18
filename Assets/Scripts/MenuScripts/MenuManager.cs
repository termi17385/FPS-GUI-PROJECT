using System.Collections;
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

    #region Settings
    public void VideoSettings()
    {
        #region Resolution
        // makes an array of the different resolutions
        resolutions = Screen.resolutions;
        resolution.ClearOptions();
        List<string> options = new List<string>();

        int currentResolutionIndex = 0;
        // go through every resolution
        for (int i = 0; i < resolutions.Length; i++)
        {
            // Build a string for displaying the resolution
            string option = resolutions[i].width + "x" + resolutions[i].height;
            options.Add(option);

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
        resolution.AddOptions(options);
        resolution.value = currentResolutionIndex;
        resolution.RefreshShownValue();
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
            PlayerPrefs.SetInt("quality", 5);       // sets the settings
            QualitySettings.SetQualityLevel(5);
        }
        else
        {
            QualitySettings.SetQualityLevel(PlayerPrefs.GetInt("quality"));
        }
        #endregion

        PlayerPrefs.Save();
    }

    public void SetFullscreen(bool fullscreen)
    {
        // sets the scene to full screen when called
        Screen.fullScreen = fullscreen;
    }

    public void ChangeQuality(int index)
    {
        // sets the quality to a choosen int when called
        QualitySettings.SetQualityLevel(index);
    }

    public void SetMusicVolume(float value)
    {
        mixer.SetFloat("Music", value);
    }

    public void SetSFXVolume(float value)
    {
        mixer.SetFloat("SFX", value);
    }
    #endregion

    #region Saving and Loading
    #endregion
}

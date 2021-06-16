using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RespawnManager : MonoBehaviour
{
    private void Awake() => Cursor.lockState = CursorLockMode.None;

    public void Respawn()
    {
        // Tell Data Manager to load save file
        // Load the loading scene and set the loading payload to load game scene
        PlayerPrefs.SetString("Load", "true");
        SceneManager.LoadScene("LoadingScreen");
        PlayerPrefs.SetString("SceneName", "GameScene");  
    }

    /// Quits to the main menu
    public void QuitToMenu()
    {
        SceneManager.LoadScene("LoadingScreen");
        PlayerPrefs.SetString("SceneName", "MainMenu");
    }
}

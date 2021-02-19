using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuHandler : MonoBehaviour
{
    #region Variables
    [SerializeField] private GameObject[] menus = new GameObject[3];

    [SerializeField] private bool goBack = false;
    [SerializeField] private bool paused = false;
    #endregion

    #region Start and Update
    private void Start()
    {
        SetMenusAtStart();
    }
    private void Update()
    {
        PauseGame();
    }
    #endregion

    #region PauseMenu
    private void SetMenusAtStart()
    {
        #region Menus
        menus[0].gameObject.SetActive(true);    // MainScreen
        menus[1].gameObject.SetActive(false);   // Menu
        menus[2].gameObject.SetActive(false);   // Options
        #endregion
    }

    private void PauseGame()
    {
        #region Bool
        bool pauseGame = Input.GetKeyDown(KeyCode.Escape) && paused == false;
        bool returnToScreen = Input.GetKeyDown(KeyCode.Escape) && goBack == true;
        bool unpauseGame = Input.GetKeyDown(KeyCode.Escape) && (paused == true && goBack == false);
        #endregion

        #region Pausing
        if (pauseGame)
        {
            menus[0].gameObject.SetActive(true);    // MainScreen
            menus[1].gameObject.SetActive(true);   // Menu 
            Cursor.lockState = CursorLockMode.None;
            paused = true;
        }

        if (unpauseGame)
        {
            menus[0].gameObject.SetActive(true);    // MainScreen
            menus[1].gameObject.SetActive(false);   // Menu
            menus[2].gameObject.SetActive(false);   // Options
            Cursor.lockState = CursorLockMode.Locked;
            paused = false;
        }

        if (returnToScreen)
        {
            menus[2].gameObject.SetActive(false);   // Options
            menus[0].gameObject.SetActive(true);    // MainScreen
            goBack = false;
        }
        #endregion
    }

    public void GoBack(bool value)
    {
        goBack = value;
    }
    #endregion
}

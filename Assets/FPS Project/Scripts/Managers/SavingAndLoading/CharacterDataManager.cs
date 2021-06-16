using System;
using System.Collections.Generic;
using FPSProject.Player;
using FPSProject.Player.Manager;
using FPSProject.Saving;
using UnityEngine;

public class CharacterDataManager : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private PlayerManager pManager;
    [SerializeField] private bool loadSave;
    private SaveManager sm;

    private void Awake()
    {
        sm = FindObjectOfType<SaveManager>();
        player = FindObjectOfType<PlayerManager>().transform;
        pManager = FindObjectOfType<PlayerManager>();

        loadSave = false;
        string text = PlayerPrefs.GetString("Load").ToLower();
        if(text != "") loadSave = bool.Parse(PlayerPrefs.GetString("Load").ToLower());
        
        if (loadSave)
       {
            LoadData();
       }
        else
       {
           Debug.LogError("No Save To Load");
       }
    }

    public void SaveData(CheckPoint _checkPoint)
    {
        sm.SaveCharacterData(new CharacterData(_checkPoint, FindObjectOfType<PlayerManager>()));
    }

    public void LoadData()
    {
        PlayerPrefs.SetInt("DataSaved", 1);
        CharacterData _data = sm.LoadCharacterData();
        
        var _pdata = pManager;
        var pRotation = player.GetComponent<PlayerController>();
        
        // loads the position and rotation of the player
        var controller = FindObjectOfType<PlayerController>(); 
        player = controller.transform;
        Debug.LogError($"LoadedPlayerPosition{player.position = Convert.FloatToVector3(_data.position)}");
        Debug.LogError($"NewLoadedPlayerPosition{player.position}");
        pRotation.rotation = Convert.FloatToVector2(_data.rotation);
        
        // loads stats for the player
        foreach(CharacterData.SavedStats stats in _data.savedStats)
        {
            pManager._pStats.Add(new PlayerManager._PlayerStats() 
            {
                name = stats._statName, 
                statValue = stats._statValue
            });
        }
        PlayerPrefs.SetString("Load", "false");
        Debug.LogError("LOADING DATA");
    }
}
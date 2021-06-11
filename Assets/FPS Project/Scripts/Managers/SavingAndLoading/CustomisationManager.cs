using UnityEngine.SceneManagement;
using System.Collections.Generic;
using FPSProject.Player.Manager;
using FPSProject.Customisation;
using System.Collections;
using FPSProject.Utils;
using UnityEngine;

public class CustomisationManager : MonoBehaviour
{
    private SaveManager sM;
    [SerializeField] private PlayerManager pManager;
    public PlayerCustomisation pCustomisation;
    public PlayerStatsCustomisation pSCustomisation;
    public Renderer characterRenderer;

    public void Save()
    {
        sM = SaveManager.instance;                                                                  // gets the instance of the save manager
        sM.SaveCustomisationData(new CustomisationDataManager(pCustomisation, pSCustomisation));    // saves the customisation and stats data
        
        // loads the next scene
        PlayerPrefs.SetString("SceneName", "GameScene");    
        SceneManager.LoadScene("LoadingScreen");

    }

    public void Load()
    {
        sM = SaveManager.instance;
        CustomisationDataManager _data = sM.LoadCustomisationData();
        
        for(int i = 0; i < _data.textures.Length; i++)
        {
            string _type = _data.textures[i].textureName;     
            int _index = _data.textures[i].textureIndex;     

            SetTexture(_type, _index);
        }

        foreach(CustomisationDataManager.Stats stats in _data.statsStruct)
        {
            pManager._pStats.Add(new PlayerManager._PlayerStats() 
            {
                name = stats.statName, 
                statValue = stats.statValue
            });
        }

        pManager.classType = _data.classType;
        pManager.raceType = _data.raceType;
        pManager.characterName = _data.characterName;
    }

    void SetTexture(string type, int index)
    {
        Texture2D texture = null;
        int matIndex = 0;

        switch (type)
        {
            case "Skin":
                texture = Resources.Load("Character/Skin_" + index) as Texture2D;
                matIndex = 1;
                break;

            case "Eyes":
                texture = Resources.Load("Character/Eyes_" + index) as Texture2D;
                matIndex = 2;
                break;

            case "Mouth":
                texture = Resources.Load("Character/Mouth_" + index) as Texture2D;
                matIndex = 3;
                break;

            case "Hair":
                texture = Resources.Load("Character/Hair_" + index) as Texture2D;
                matIndex = 4;
                break;

            case "Armour":
                texture = Resources.Load("Character/Armour_" + index) as Texture2D;
                matIndex = 5;
                break;

            case "Clothes":
                texture = Resources.Load("Character/Clothes_" + index) as Texture2D;
                matIndex = 6;
                break;
        }

        // gets the material
        Material[] mats = characterRenderer.materials;  // makes an array of materials
        mats[matIndex].mainTexture = texture;           // gets the materials
        characterRenderer.materials = mats;             // assigns what material we are changing
    }

    //private void OnGUI()
    //{
    //    GUI.matrix = IMGUIUtils.IMGUIMatrix(1920, 1080);
        
    //    if(GUI.Button(new Rect(100,100, 100, 50), "Load"))
    //    {
    //        Load();
    //    }
    //}
}

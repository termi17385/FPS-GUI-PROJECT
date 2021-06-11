using FPSProject.Customisation;
using UnityEngine;
using System;

[Serializable]
public class CustomisationDataManager
{
    #region Variables
    #region Stucts
    [Serializable]
    public struct Stats
    {
        public string statName;
        public int statValue;
    }

    [Serializable]
    public struct CharacterTextures
    {
        public string textureName;
        public int textureIndex;
    }
    #endregion

    public Stats[] statsStruct = new Stats[6];
    public CharacterTextures[] textures = new CharacterTextures[6];

    public string classType;
    public string raceType;

    public string characterName;
    #endregion

    public CustomisationDataManager(PlayerCustomisation _customisationData, PlayerStatsCustomisation _statsData)
    {
        #region save stats
        for (int i = 0; i < _statsData.characterStats.Length; i++)
        {
            var _data = _statsData.characterStats[i];
            statsStruct[i].statName = _data.baseStatsName;
            statsStruct[i].statValue = (_data.baseStats + _data.tempStats);
        }
        #endregion
        #region save class, race and character Name
        classType = _statsData.characterClass.ToString();
        raceType = _statsData.race.ToString();
        characterName = _statsData.characterName;
        #endregion
        #region save textures
        textures[0].textureName = "Skin";
        textures[1].textureName = "Eyes";
        textures[2].textureName = "Mouth";
        textures[3].textureName = "Hair";
        textures[4].textureName = "Armour";
        textures[5].textureName = "Clothes";

        for (int i = 0; i < textures.Length; i++)
        {
            textures[i].textureIndex = _customisationData.textures[i].index;
        }

        #region Old Code
        //textures[0].textureIndex = _customisationData.skinIndex;
        //textures[1].textureIndex = _customisationData.eyesIndex;
        //textures[2].textureIndex = _customisationData.mouthIndex;
        //textures[3].textureIndex = _customisationData.hairIndex;
        //textures[4].textureIndex = _customisationData.armourIndex;
        //textures[5].textureIndex = _customisationData.clothesIndex;
        #endregion
        #endregion
    }
}

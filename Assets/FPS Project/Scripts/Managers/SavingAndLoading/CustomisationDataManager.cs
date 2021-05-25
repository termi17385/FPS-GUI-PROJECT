using FPSProject.Customisation;
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
        textures[0].textureName = "skin";
        textures[1].textureName = "eyes";
        textures[2].textureName = "mouth";
        textures[3].textureName = "hair";
        textures[4].textureName = "armour";
        textures[5].textureName = "clothes";

        textures[0].textureIndex = _customisationData.skinIndex;
        textures[1].textureIndex = _customisationData.eyesIndex;
        textures[2].textureIndex = _customisationData.mouthIndex;
        textures[3].textureIndex = _customisationData.hairIndex;
        textures[4].textureIndex = _customisationData.armourIndex;
        textures[5].textureIndex = _customisationData.clothesIndex;
        #endregion
    }
}

using System;
using FPSProject.Player.Manager;
using UnityEngine;

namespace FPSProject.Saving
{
    [Serializable]
    public class CharacterData
    {
        #region Variables
        // position data
        public float[] position = new float[3];
        public float[] rotation = new float[2];
       
        [Serializable] public struct SavedStats
        {
            public string _statName;
            public int _statValue;
        }  
        public SavedStats[] savedStats = new SavedStats[6];
        #endregion
        public CharacterData(CheckPoint _data, PlayerManager _pData)
        {
            #region Position and Rotation data
            /* Handles saving the positional
            and rotational data of the player*/

            Vector3 _position = _data.playerTransform.position;
            position[0] = _position.x;
            position[1] = _position.y;
            position[2] = _position.z;
            Vector2 _rotation = _data.playerRotation;
            rotation[0] = _rotation.x;
            rotation[1] = _rotation.y;
            #endregion

            #region PlayerStats
            for (int i = 0; i < _pData._pStats.Count; i++)
            {
                var data = _pData._pStats[i];
                
                savedStats[i]._statName = data.name;
                savedStats[i]._statValue = data.statValue;
            }            
            #endregion
        }
    }
}

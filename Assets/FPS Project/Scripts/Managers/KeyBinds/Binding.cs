using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace FPSProject.Keybinds
{
    [System.Serializable]
    public class Binding
    {
        #region Properties
        public string Name {get {return name; }} // name of keybind
        public KeyCode Value {get {return value; }} // value for the keybind

        public string ValueDisplay {get { return BindingUtils.TranslateKeyCode(value); }}
        #endregion

        #region Variables
        [SerializeField, Tooltip("Name of the button")] private string name;
        [SerializeField, Tooltip("Key value for the button")] private KeyCode value;
        #endregion

        #region Binding
        /// <summary>
        /// gets and sets the default keys with
        /// the name associated to that key
        /// </summary>
        /// <param name="_name">name of the keybind</param>
        /// <param name="_defaultValue">the actual value of the keybind</param>
        public Binding(string _name, KeyCode _defaultValue)
        {
            name = _name;
            value = _defaultValue;
        }
        #endregion

        #region Save and Load
        public void Save()
        {
            // (int) converts data into interger formatt
            PlayerPrefs.SetInt(name, (int)value);
            PlayerPrefs.Save();
        }

        public void Load()
        {
            // gets the int and converts it to keycode
            value = (KeyCode)PlayerPrefs.GetInt(name, (int)value);
        }

        public void Rebind(KeyCode _new)
        {
            value = _new;
            Save();
        }
        #endregion

        #region InputValues
        // returns if key is pressed
        public bool Pressed()
        {
            return Input.GetKeyDown(value);
        }
        // returns if key is held
        public bool Held()
        {
            return Input.GetKey(value);
        }
        // returns if key is released
        public bool Release()
        {
            return Input.GetKeyUp(value);
        }
        #endregion
    }
}
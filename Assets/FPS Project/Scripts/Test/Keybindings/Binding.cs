using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Binding
{
    #region Properties
    /// <summary>
    /// name of the keybind
    /// </summary>
    public string Name {get { return name; } }
    /// <summary>
    /// Keycode value for the keybind
    /// </summary>
    public KeyCode Value { get { return value; } }
    /// <summary>
    /// what to display on the keybind
    /// </summary>
    public string ValueDisplay { get { return BindingUtils.TranslateKeyCode(value); } }
    #endregion
    #region Variable
    [SerializeField, Tooltip("Name of the button")] private string name;
    [SerializeField, Tooltip("Key Value for the button")] private KeyCode value;
    #endregion

    #region Binding
    /// <summary>
    ///  handles getting the default keys and the name associated to that keybind
    /// </summary>
    /// <param name="_name">returns the name of the keybind</param>
    /// <param name="_defaultValue">returns the default value for the key</param>
    public Binding(string _name, KeyCode _defaultValue)
    {
        name = _name;
        value = _defaultValue;
    }

    #region Save and load
    /// <summary>
    /// Saves the values and names of the keybinds
    /// </summary>
    public void Save()
    {
        // (int) converts data into an interger format
        PlayerPrefs.SetInt(name, (int)value);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Loads the values and names of the keybinds
    /// </summary>
    public void Load()
    {
        // gets the interger value for the keycode then converts it into the keycode
        value = (KeyCode)PlayerPrefs.GetInt(name, (int)value);
    }
    #endregion

    /// <summary>
    /// Rebinds the keys with the new given value
    /// </summary>
    /// <param name="_new"></param>
    public void Rebind(KeyCode _new)
    {
        value = _new;
        Save();
    }
    #endregion
    #region InputValues
    /// <summary>
    /// Is the key pressed?
    /// </summary>
    public bool Pressed()
    {
        return Input.GetKeyDown(value);
    }

    /// <summary>
    /// Is the key held?
    /// </summary>
    public bool Held()
    {
        return Input.GetKey(value);
    }

    /// <summary>
    /// is the key released?
    /// </summary>
    public bool Release()
    {
        return Input.GetKeyUp(value);
    }
    #endregion
}

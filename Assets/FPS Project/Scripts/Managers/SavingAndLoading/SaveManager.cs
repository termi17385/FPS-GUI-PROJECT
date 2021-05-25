using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System.IO;
using System;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;
    string filePath;

    #region Start and Awake
    void Awake() => filePath = Application.persistentDataPath + "/sav.cust";

    private void Start()
    {
        if(instance == null) instance = this;
        else Destroy(gameObject);
    }
    #endregion

    public void SaveCustomisationData(CustomisationDataManager data)
    {
        BinaryFormatter _formatter = new BinaryFormatter();                 // formatts the data to binary
        FileStream _stream = new FileStream(filePath, FileMode.Create);     // makes a stream for creating the file at a specified place

        _formatter.Serialize(_stream, data);    // serialises the data
        _stream.Close();                        // closes the stream
        Debug.LogError(filePath);
    } 

    public CustomisationDataManager LoadCustomisationData()
    {
        // if the file is found
        if (File.Exists(filePath))
        {
            // formatter to binary
            BinaryFormatter _formatter = new BinaryFormatter(); 
            // opens a stream that opens a file at the given data path
            FileStream _stream = new FileStream(filePath, FileMode.Open);
            // loads the data and deserialises it
            CustomisationDataManager _saveData = _formatter.Deserialize(_stream) as CustomisationDataManager;

            _stream.Close();
            return _saveData;
        }
        else
        {
            // else send error message
            Debug.LogError("File Not Found");
            return null;
        }
    } 
}

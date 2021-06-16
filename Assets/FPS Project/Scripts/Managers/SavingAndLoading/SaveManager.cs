using System.Runtime.Serialization.Formatters.Binary;
using FPSProject.Saving;
using UnityEngine;
using System.IO;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;
    string filePathCust;
    string filePathGame;

    #region Start and Awake

    void Awake()
    {
        filePathCust = Path.Combine(Application.persistentDataPath, "/sav.cust"); // customisation data only
        filePathGame = Path.Combine(Application.persistentDataPath, "/sav.game"); // all of game data
    }

    private void Start()
    {
        if (instance != null) instance = this;
        else Destroy(gameObject);
    }
    #endregion

    public void SaveCustomisationData(CustomisationDataManager data)
    {
        BinaryFormatter _formatter = new BinaryFormatter();                 // formats the data to binary
        FileStream _stream = new FileStream(filePathCust, FileMode.Create);     // makes a stream for creating the file at a specified place

        _formatter.Serialize(_stream, data);    // serialises the data
        _stream.Close();                        // closes the stream
        //Debug.LogError(filePathCust);
    } 
    
    public void SaveCharacterData(CharacterData data)
    {
        BinaryFormatter _formatter = new BinaryFormatter();                     // formats the data to binary
        FileStream _stream = new FileStream(filePathGame, FileMode.Create);     // makes a stream for creating the file at a specified place

        _formatter.Serialize(_stream, data);    // serialises the data
        _stream.Close();                        // closes the stream
        //Debug.LogError(filePathGame);
    } 

    public CustomisationDataManager LoadCustomisationData()
    {
        // if the file is found
        if (File.Exists(filePathCust))
        {
            // formatter to binary
            BinaryFormatter _formatter = new BinaryFormatter(); 
            // opens a stream that opens a file at the given data path
            FileStream _stream = new FileStream(filePathCust, FileMode.Open);
            // loads the data and deserializes it
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
    
    public CharacterData LoadCharacterData()
    {
        // if the file is found
        if (File.Exists(filePathGame))
        {
            // formatter to binary
            BinaryFormatter _formatter = new BinaryFormatter(); 
            // opens a stream that opens a file at the given data path
            FileStream _stream = new FileStream(filePathGame, FileMode.Open);
            // loads the data and deserializes it
            CharacterData _saveData = _formatter.Deserialize(_stream) as CharacterData;

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

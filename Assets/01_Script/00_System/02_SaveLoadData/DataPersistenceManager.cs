using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DataPersistenceManager : MonoBehaviour
{
    [SerializeField]
    private string _nameOfTheSaveFile;

    private GameDataContiner _gameDataContiner;
    private FileDataHandler _fileDataHandler;


    public static DataPersistenceManager Instance {get; private set;}

    public void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("Two Copy of DataPersistenceManager Exist!!!!");
        }
        Instance = this;

         _fileDataHandler = new FileDataHandler(_nameOfTheSaveFile);
    }

    public List<IDataPersistence> FindAllContractedGameObjectsInTheScen()
    {
        //by using Linq, find all class of type x.y
        IEnumerable<IDataPersistence> contectedObject = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();

        return new List<IDataPersistence>(contectedObject);
    }

    public void NewGame()
    {
        this._gameDataContiner = new GameDataContiner();
    }

    public void LoadGame()
    {
        //Get Game Data Continer from FileDataHandler Load file
        _gameDataContiner = _fileDataHandler.Load();

        //otherwise make new
        if (_gameDataContiner == null)
        {
            Debug.Log("No Data was found...");
            NewGame();
        }

        //Get all object by type MonoBehaviour and IDataPersistence
        List<IDataPersistence> contractedObjects = FindAllContractedGameObjectsInTheScen();

        //Pass Game Data Continer to contracted object to read
        foreach (var item in contractedObjects)
        {
            item.ReadDataToFileContiner(_gameDataContiner);
        }

        Debug.Log(_gameDataContiner._scoreCount);
        Debug.Log(_gameDataContiner._playerPosition);
    }

    public void SaveGame()
    {
        //Pass Game Data Continer to contracted objects to write
        //Get all object by type MonoBehaviour and IDataPersistence
        List<IDataPersistence> contractedObjects = FindAllContractedGameObjectsInTheScen();

        //Pass Game Data Continer to contracted object to read
        foreach (var item in contractedObjects)
        {
            item.WriteDataToFileContiner(ref _gameDataContiner);
        }

        //Send Game Data Continer to FileDataHandler for storing
        _fileDataHandler.Save(ref _gameDataContiner);

        Debug.Log(_gameDataContiner._scoreCount);
        Debug.Log(_gameDataContiner._playerPosition);
    }
}

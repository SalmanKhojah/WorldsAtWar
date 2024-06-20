using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class FileDataHandler
{
    private string _dataFileName = "";

    public FileDataHandler(string dataFileName)
    {
        this._dataFileName = dataFileName;
    }

    public GameDataContiner Load()
    {
        // use Path.Combine to account for different OS's having different path separators
        string fullPath = Path.Combine(Application.persistentDataPath, _dataFileName);

        GameDataContiner loadedDataConiter = null;

        if (File.Exists(fullPath))
        {
            try
            {
                // load the serialized data from the file
                string dataToLoad = "";
                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                // deserialize the data from Json back into the C# object
                loadedDataConiter = JsonUtility.FromJson<GameDataContiner>(dataToLoad);
            }
            catch (Exception e)
            {
                Debug.LogError("Error occured when trying to load file at path: " + fullPath + "\n" + e);
            }
        }
        return loadedDataConiter;
    }

    public void Save(ref GameDataContiner data)
    {
        // use Path.Combine to account for different OS's having different path separators
        string fullPath = Path.Combine(Application.persistentDataPath, _dataFileName);
        try
        {
            // create the directory the file will be written to if it doesn't already exist
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            // serialize the C# game data object into Json
            string dataToStore = JsonUtility.ToJson(data, true);


            // write the serialized data to the file
            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error occured when trying to save data to file: " + fullPath + "\n" + e);
        }
    }
}
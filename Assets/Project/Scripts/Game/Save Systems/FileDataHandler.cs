using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class FileDataHandler
{
    private string dataDirPath = " ";

    private string dataFileName = " ";

    public FileDataHandler(string dataDir, string dataFile)
    {
        this.dataDirPath = dataDir;
        this.dataFileName = dataFile;
    }

    public GameData Load()
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        GameData loadedData = null;
        if (File.Exists(fullPath))
        {
            try
            {
                string dataToLoad = " ";
                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }

                }

                // Chuyen data tu json ve cac C# objects
                loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
            }
            catch (Exception e)
            {
                Debug.LogError("Error " + e + " occured when trying to load: " + fullPath);
            }
        }

        return loadedData;
    }

    public void Save(GameData data)
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        try
        {
            // Tao ra thu muc cua file se duoc luu neu no chua duoc tao ra
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            // Chuyen game data ve dang json
            string dataToStore = JsonUtility.ToJson(data, true);

            // Viet data ra vao file
            using(FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using(StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }
        }

        catch (Exception e)
        {
            Debug.LogError("Error " + e + " occured when trying to save: " + fullPath);
        }
    }

}

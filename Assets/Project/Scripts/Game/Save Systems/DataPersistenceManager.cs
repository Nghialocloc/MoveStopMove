using System.Collections.Generic;
using UnityEngine;

public class DataPersistenceManager : Singleton<DataPersistenceManager>
{
    [Header("File Storage Config")]
    [SerializeField] private string fileName;

    private GameData gameData;

    private List<IDataPersistence> dataPersistencesObjects = new List<IDataPersistence>();

    private FileDataHandler dataHandler;

    private void Awake()
    {
        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
        LoadGame();
    }

    #region One Control

    // Load script rieng biet
    public void CallData(IDataPersistence data) 
    {
        if (!dataPersistencesObjects.Contains(data))
        {
            dataPersistencesObjects.Add(data);
        }
        data.LoadData(gameData);
    }

    public void SendData(IDataPersistence data)
    {
        if (!dataPersistencesObjects.Contains(data))
        {
            dataPersistencesObjects.Add(data);
        }
        data.SaveData(ref gameData);
        dataHandler.Save(gameData);
        Debug.Log("Luu du lieu rieng o C:Users|ADMIN|AppData|LocalLow|DefaultCompany|MoveStopMove");
    }

    // Luu thong so game khi thoat ra
    public void OnApplicationQuit()
    {
        SaveGame();
    }

    #endregion

    #region Full Control
    // Khoi tao Game Data moi
    public void NewGame()
    {
        this.gameData = new GameData();
    }

    // Load toan bo cac script data
    public void LoadGame()
    {
        // Load bat cu du lieu nao tu file dung data handler
        this.gameData = dataHandler.Load();

        // Neu khong co du lieu, khoi tao moi bang New Game
        if(this.gameData == null)
        {
            Debug.Log("No data was found. Initializing new default data");
            NewGame();
        }

        // day cac du lieu load dc len cac script khac
        foreach(IDataPersistence data in dataPersistencesObjects)
        {
            data.LoadData(gameData);
            Debug.Log("Load du lieu");
        }
    }

    // Luu toan bo data
    public void SaveGame()
    {
        // Pass du lieu tu cac script khac vao de update
        foreach (IDataPersistence data in dataPersistencesObjects)
        {
            data.SaveData(ref gameData);
            Debug.Log("Luu du lieu o C:Users|ADMIN|AppData|LocalLow|DefaultCompany|MoveStopMove");
        }
        // Pass du lieu vao data handler de luu vao file
        dataHandler.Save(gameData);
    }

    #endregion
}

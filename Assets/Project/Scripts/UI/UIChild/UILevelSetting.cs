using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UILevelSetting : UICanvas, IDataPersistence
{
    [Header("Component")]
    [SerializeField] private Transform contentView;
    [SerializeField] private LevelButton prefab;
    [SerializeField] private GameObject[] buttonObjects;
    [SerializeField] private LevelData data;

    private LevelButton currentLevel;
    private SerilizableDictionary<int, LevelState> curLevelList;
    private MiniPool<LevelButton> miniPool = new MiniPool<LevelButton>();

    #region Load and Save

    public void LoadData(GameData data)
    {
        curLevelList = data.levelList;

        // Khoi tao ban dau khi chua luu du lieu
        if (curLevelList.Keys.Count == 0)
        {
            for(int i = 0; i < LevelManager.Ins.levels.Length; i++) 
            { 
                curLevelList.Add(i, LevelState.Locked);
            }
            curLevelList[0] = LevelState.Loaded;
        }
    }

    public void SaveData(ref GameData data)
    {
        data.levelList = curLevelList;
    }

    #endregion

    private void Awake()
    {
        miniPool.OnInit(prefab, 1, contentView); 
    }

    public override void Open()
    {
        base.Open();
        GameManager.Ins.ChangeState(GameState.MainMenu);
        GameManager.Ins.CamControl(GameState.MainMenu);
        DataPersistenceManager.Ins.CallData(this);
        InitState();
        AudioManager.Ins.MusicControl();
    }

    public override void CloseDirectly()
    {
        base.CloseDirectly();
        UIManager.Ins.OpenUI<UIMainMenu>();
        DataPersistenceManager.Ins.SendData(this);
    }

    #region Set Level

    public void InitState()
    {
        miniPool.Collect();
        for (int i = 0; i < data.levelInfo.Count; i++)
        {
            LevelButton button = miniPool.Spawn();
            curLevelList.TryGetValue(data.levelInfo[i].levelNumber, out LevelState value);
            button.SetData(data.levelInfo[i].levelNumber, data.levelInfo[i].levelName, value,this);
        }

        if (currentLevel != null)
        {
            currentLevel.SetState(LevelState.Locked);
        }
        currentLevel = null;
        SetState(LevelState.None);
    }

    public void SetState(LevelState state)
    {
        for (int i = 0; i < buttonObjects.Length; i++)
        {
            buttonObjects[i].SetActive(false);
        }

        if ((int)state <= 3)
        {
            buttonObjects[(int)state].SetActive(true);
        }
    }

    public LevelState GetLevelState(int index)
    {
        curLevelList.TryGetValue(index, out LevelState state);
        return state;
    }

    #endregion

    #region Choose Level

    public void SelectLevel(LevelButton level)
    {
        if (currentLevel != null)
        {
            currentLevel.SetState(LevelState.Locked);
        }

        currentLevel=level;

        switch (GetLevelState(currentLevel.levelIndex))
        {
            case LevelState.Locked:
                SetState(LevelState.Locked);
                break;
            case LevelState.Load:
                SetState(LevelState.Load);
                break;
            case LevelState.Loaded:
                SetState(LevelState.Loaded);
                break;
            case LevelState.Selecting:
                break;
            default:
                break;
        }

        currentLevel.SetState(LevelState.Selecting);
        
    } 
    
    public void LoadLevel()
    {
        AudioManager.Ins.PlaySfx(Constants.EFF_CLICK);

        //Thay doi trang thai level cu
        curLevelList[LevelManager.Ins.levelIndex] = LevelState.Load;

        //Thay doi trang thai level moi
        curLevelList[currentLevel.levelIndex] = LevelState.Loaded;

        //Gan gia tri level vào Level Manager
        LevelManager.Ins.levelIndex = currentLevel.levelIndex;

        //Dat lai trang thai nut level
        currentLevel.SetState(LevelState.Loaded);

        //Luu du lieu lai
        DataPersistenceManager.Ins.SendData(this);

        //Dong UI và reset lai man choi
        UIManager.Ins.CloseAll();
        LevelManager.Ins.OnReset();
    }

    #endregion
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>, IDataPersistence
{
    [Header("Levels")]
    public Level[] levels;
    public Level currentLevel;
    public int levelIndex;
    //public LevelData levelData;
    private SerilizableDictionary<int, LevelState> curLevelList;

    [Header("Player")]
    [SerializeField] private Player player;
    public Player currentPlayer;
    public int Coin;

    [Header("SpawnBot")]
    [SerializeField] private Bot botPrefab;
    [SerializeField] private int curBotNumber = 0;
    [SerializeField] private int activeBotNumber;
    public int totalBotNumber;
    [SerializeField] private LayerMask collideLayer;
    [SerializeField] private int spawnSpeed;
    [SerializeField] private bool isInitSpawn;

    [Header("UI")]
    [SerializeField] private Transform canvasOverlay;

    private List<Bot> bots = new();

    #region Load and Save

    public void LoadData(GameData data)
    {
        levelIndex = data._Key_Level;
        if(levelIndex > levels.Length)
        {
            levelIndex--;
        }
        curLevelList = data.levelList;
    }

    public void SaveData(ref GameData data)
    {
        data._Key_Level = levelIndex;
        data._Key_Coin += Coin;
        data.levelList = curLevelList;
    }

    #endregion

    public void Start()
    {
        levels = Resources.LoadAll<Level>("Levels/");
        //levels = new Level[levelData.levelInfo.Count];
        //for (int i = 0; i < levelData.levelInfo.Count; i++)
        //{
        //    levels[i] = levelData.levelInfo[i].levelPrefab;
        //}
        OnInit();
    }

    #region Control State

    //khoi tao trang thai bat dau game
    public void OnInit()
    {
        DataPersistenceManager.Ins.CallData(this);
        isInitSpawn = true;
        OnLoadLevel(levelIndex);
        curBotNumber = 0;
        activeBotNumber = currentLevel.GetBotNumber();
        totalBotNumber = currentLevel.GetAllBotNumber();

        if (currentPlayer != null)
        {
            Destroy(currentPlayer.gameObject);
        }
        currentPlayer = Instantiate(player);
        currentPlayer.transform.position = currentLevel.GetPlayerSpawn().position;
        GameManager.Ins.camera.player = currentPlayer.transform;
        GameManager.Ins.OnInit();
        StartCoroutine(SpawnAllBot());
        SetTargetIndicatorAlpha(0);
    }

    //reset trang thai khi ket thuc va man choi
    public void OnReset()
    {
        DataPersistenceManager.Ins.SendData(this);
        currentPlayer.OnDespawn();
        for (int i = 0; i < bots.Count; i++)
        {
            bots[i].OnDespawn();
        }
        bots.Clear();
        SimplePool.CollectAll();
        AudioManager.Ins.ResetSound();
        OnInit();
    }

    //tao prefab level moi
    public void OnLoadLevel(int level)
    {
        if (currentLevel != null)
        {
            Destroy(currentLevel.gameObject);
        }

        currentLevel = Instantiate(levels[level]);
    }

    #endregion

    #region Control Bot

    public IEnumerator SpawnAllBot()
    {
        for (int i = 0; i < activeBotNumber; i++)
        {
            SpawnBot();
            yield return null;
        }
        isInitSpawn = false;
    }

    public void SpawnBot()
    {
        StartCoroutine(SetUpBot());
    }

    public IEnumerator SetUpBot()
    {
        WaitForSeconds wait = new WaitForSeconds(0.5f);
        bool hasSpawn = false;
        while (!hasSpawn)
        {
            if (TrySpawnBot())
            {
                hasSpawn = true;
                
            }

            yield return wait;
        }
    }

    public bool TrySpawnBot()
    {
        for(int i = 0; i < spawnSpeed; i++)
        {
            Vector3 pos = currentLevel.RandomPoint();
            Collider[] check = Physics.OverlapSphere(pos, 3f, collideLayer);
            if (check.Length <= 0)
            {
                curBotNumber++;
                Bot bot = SimplePool.Spawn<Bot>(botPrefab, pos, Quaternion.identity);
                if (isInitSpawn)
                {
                    bot.OnInit();
                }
                else
                {
                    bot.OnRespawn();
                }
                bots.Add(bot);
                return true;
            }    
        }
        return false;
    }

    #endregion

    #region Control Gameplay

    public void SetTargetIndicatorAlpha(float alpha)
    {
        HashSet<GameUnit> hash = SimplePool.GetAllUnitIsActive(PoolType.TargetIndicator);

        foreach(GameUnit unit in hash )
        {
            (unit as TargetIndicator).SetAlpha(alpha);
            unit.gameObject.transform.SetParent(canvasOverlay);
        }
    }

    public void CharecterDeath(Character c)
    {
        if (c is Player)
        {
            UIManager.Ins.CloseAll();
            
            if (!currentPlayer.isRevive)
            {
                currentPlayer.isRevive = true;
                UIManager.Ins.OpenUI<UIRevive>();
            }
            else
            {
                Coin = currentPlayer.score * 10;
                Lose();
            }
          
        }
        else if (c is Bot)
        {
            bots.Remove(c as Bot);
            curBotNumber--;

            if(curBotNumber < activeBotNumber && activeBotNumber < totalBotNumber)
            {
                totalBotNumber--;
                SpawnBot();
            }
            else if(curBotNumber < activeBotNumber && activeBotNumber >= totalBotNumber)
            {
                totalBotNumber--;
            }
        }

        UIManager.Ins.GetUI<UIGamplay>().UpdateNumberEnemy();
        if (totalBotNumber <= 0)
        {
            Coin = currentPlayer.score * 10;
            Win();
        }
    }

    private void Win()
    {
        if( (levelIndex) < levels.Length)
        {
            curLevelList[levelIndex] = LevelState.Load;
            levelIndex++;
            curLevelList[levelIndex] = LevelState.Loaded;
        }

        Coin = Coin + (levelIndex * 50);

        UIManager.Ins.CloseAll();
        UIManager.Ins.OpenUI<UIWin>().SetCoin(Coin);
        currentPlayer.StopPlayerMovement();
        currentPlayer.ChangeAnim(Constants.ANIM_WIN);
        Quaternion rotate = new Quaternion();
        rotate.eulerAngles = new Vector3(0, 180, 0);
        currentPlayer.TF.rotation = Quaternion.Lerp(currentPlayer.TF.rotation, rotate, 1);
    }

    public void Lose()
    {
        UIManager.Ins.CloseAll();
        UIManager.Ins.OpenUI<UILose>();
    }

    #endregion

}

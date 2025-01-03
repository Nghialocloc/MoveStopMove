using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UISkinShop : UICanvas, IDataPersistence
{
    [Header("Component")]
    [SerializeField] private TextMeshProUGUI playerCoinTxt;
    [SerializeField] ItemBar[] itemBars;
    [SerializeField] private Transform contentView;
    [SerializeField] private ItemContainer prefab;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private GameObject[] buttonObjects;

    [Header("Data")]
    [SerializeField] private SkinShopData data;

    public SkinShopCatergory currentCatergory => currentBar.Type;
    private ItemBar currentBar;
    private ItemContainer currentItem;
    private MiniPool<ItemContainer> miniPool = new MiniPool<ItemContainer>();

    //Du lieu item
    private int playerCoin;
    private SerilizableDictionary<HatType, BuyState> curHatList;
    private SerilizableDictionary<PantType, BuyState> curPantList;
    private SerilizableDictionary<AccessoryType, BuyState> curAccessoryList;
    private SerilizableDictionary<SkinType, BuyState> curSkinList;

    private HatType curEquipHat;
    private PantType curEquipPant;
    private AccessoryType curEquipAcc;
    private SkinType curEquipedSkin;

    #region Load and Save

    public void LoadData(GameData data)
    {
        playerCoin = data._Key_Coin;
        curHatList = data.hatList;
        curPantList = data.pantList;
        curAccessoryList = data.accessoryList;
        curSkinList = data.skinList;

        // Khoi tao ban dau khi chua luu du lieu
        if (curHatList.Keys.Count == 0)
        {
            foreach (HatData hat in this.data.hats)
            {
                curHatList.Add(hat.type, BuyState.Buy);
            }
            curHatList[HatType.HAT_None] = BuyState.Equipped;
        }

        if (curPantList.Keys.Count == 0)
        {
            foreach (PantData pant in this.data.pants)
            {
                curPantList.Add(pant.type, BuyState.Buy);
            }
            curPantList[PantType.PANT_None] = BuyState.Equipped;
        }

        if (curAccessoryList.Keys.Count == 0)
        {
            foreach (AccessoryData accessory in this.data.accessories)
            {
                curAccessoryList.Add(accessory.type, BuyState.Buy);
            }
            curAccessoryList[AccessoryType.ACC_None] = BuyState.Equipped;
        }

        if (curSkinList.Keys.Count == 0)
        {
            foreach (SkinData skin in this.data.skins)
            {
                curSkinList.Add(skin.type, BuyState.Buy);
            }
            curSkinList[SkinType.SKIN_None] = BuyState.Equipped;
        }

        curEquipHat = data.equipedHat;
        curEquipPant = data.equipedPant;
        curEquipAcc = data.equipedAcc;
        curEquipedSkin = data.equipedSkin;
    }

    public void SaveData(ref GameData data)
    {
        data._Key_Coin = playerCoin;

        data.hatList = curHatList;
        data.pantList = curPantList;
        data.accessoryList = curAccessoryList;
        data.skinList = curSkinList;

        data.equipedHat = curEquipHat;
        data.equipedPant = curEquipPant;
        data.equipedAcc = curEquipAcc;
        data.equipedSkin = curEquipedSkin;
    }

    #endregion

    private void Awake()
    {
        miniPool.OnInit(prefab, 1, contentView);

        for (int i = 0; i < itemBars.Length; i++)
        {
            itemBars[i].SetShop(this);
        }
    }

    public override void Open()
    {
        base.Open();
        GameManager.Ins.ChangeState(GameState.SkinShop);
        GameManager.Ins.CamControl(GameState.SkinShop);
        DataPersistenceManager.Ins.CallData(this);
        playerCoinTxt.text = playerCoin.ToString();
        SelectBar(itemBars[0]);
        AudioManager.Ins.MusicControl();
    }

    public override void CloseDirectly()
    {
        base.CloseDirectly();
        UIManager.Ins.OpenUI<UIMainMenu>();
    }

    #region Select Item

    // Kich hoat danh sach trang bi duoc chon va hien thi tren man hinh
    public void SelectBar(ItemBar itemBar)
    {
        if (currentBar != null)
        {
            currentBar.ActiveBar(false);
        }

        currentBar = itemBar;
        currentBar.ActiveBar(true);

        miniPool.Collect();

        switch (currentBar.Type)
        {
            case SkinShopCatergory.Hat:
                for (int i = 0; i < data.hats.Count; i++)
                {
                    ItemContainer item = miniPool.Spawn();
                    curHatList.TryGetValue(data.hats[i].type, out BuyState value);
                    item.SetHatData(data.hats[i], this, value);
                }
                break;
            case SkinShopCatergory.Pant:
                for (int i = 0; i < data.pants.Count; i++)
                {
                    ItemContainer item = miniPool.Spawn();
                    curPantList.TryGetValue(data.pants[i].type, out BuyState value);
                    item.SetPantData(data.pants[i], this, value);
                }
                break;
            case SkinShopCatergory.Accessory:
                for (int i = 0; i < data.accessories.Count; i++)
                {
                    ItemContainer item = miniPool.Spawn();
                    curAccessoryList.TryGetValue(data.accessories[i].type, out BuyState value);
                    item.SetAccessoryData(data.accessories[i], this, value);
                }
                break;
            case SkinShopCatergory.Skin:
                for (int i = 0; i < data.skins.Count; i++)
                {
                    ItemContainer item = miniPool.Spawn();
                    curSkinList.TryGetValue(data.skins[i].type, out BuyState value);
                    item.SetSkinData(data.skins[i], this, value);
                }
                break;
            default:
                break;
        }

        // Reset khi chuyen shop
        if (currentItem != null)
        {
            currentItem.SetState(BuyState.Buy);
        }
        currentItem = null;
        SetState(BuyState.None);
    }

    // Hien thi thong so item dc lua chon
    public void SelectItem(ItemContainer item)
    {
        if (currentItem != null)
        {
            currentItem.SetState(BuyState.Buy);
        }

        currentItem = item;

        switch (GetBuyState(currentItem.itemID))
        {
            case BuyState.Buy:
                SetState(BuyState.Buy);
                break;
            case BuyState.Bought:
                SetState(BuyState.Bought);
                break;
            case BuyState.Equipped:
                SetState(BuyState.Equipped);
                break;
            case BuyState.Selecting:
                break;
            default:
                break;
        }

        currentItem.SetState(BuyState.Selecting);
        priceText.text = item.cost.ToString();
    }

    // Tra ve trang thai mua cua Item
    public BuyState GetBuyState(Enum id)
    {
        BuyState state;
        switch (id)
        {
            case HatType:
                curHatList.TryGetValue((HatType)currentItem.itemID, out BuyState hatValue);
                state = hatValue;
                break;
            case PantType:
                curPantList.TryGetValue((PantType)currentItem.itemID, out BuyState pantValue);
                state = pantValue;
                break;
            case AccessoryType:
                curAccessoryList.TryGetValue((AccessoryType)currentItem.itemID, out BuyState accValue);
                state = accValue;
                break;
            case SkinType:
                curSkinList.TryGetValue((SkinType)currentItem.itemID, out BuyState skinValue);
                state = skinValue;
                break;
            default:
                state = currentItem.GetItemState();
                break;
        }

        return state;
    }


    // Chuyen nut mua o duoi man hinh theo trang thai item
    public void SetState(BuyState state)
    {
        for (int i = 0; i < buttonObjects.Length; i++)
        {
            buttonObjects[i].SetActive(false);
        }

        if ((int) state <= 3)
        {
            buttonObjects[(int)state].SetActive(true);
        }
    }

    #endregion

    #region Buy Item

    public void BuyButton()
    {
        AudioManager.Ins.PlaySfx(Constants.EFF_CLICK);
        //Kiem tra so tien cua nguoi choi co du ko
        if (playerCoin < currentItem.cost)
        {
            return;
        }
        else
        {
            playerCoin = playerCoin - currentItem.cost;
            playerCoinTxt.text = playerCoin.ToString();
        }

        switch (currentItem.itemID)
        {
            case HatType:
                curHatList[(HatType)currentItem.itemID] = BuyState.Bought;
                break;
            case PantType:
                curPantList[(PantType)currentItem.itemID] = BuyState.Bought;
                break;
            case AccessoryType:
                curAccessoryList[(AccessoryType)currentItem.itemID] = BuyState.Bought;
                break;
            case SkinType:
                curSkinList[(SkinType)currentItem.itemID] = BuyState.Bought;
                break;
            default:
                break;
        }
        SelectItem(currentItem);
        DataPersistenceManager.Ins.SendData(this);
    }

    public void EquipButton()
    {
        AudioManager.Ins.PlaySfx(Constants.EFF_CLICK);
        switch (currentItem.itemID)
        {
            case HatType:
                curHatList[curEquipHat] = BuyState.Bought;
                curHatList[(HatType)currentItem.itemID] = BuyState.Equipped;
                curEquipHat = (HatType)currentItem.itemID;
                LevelManager.Ins.currentPlayer.DespawnHat();
                LevelManager.Ins.currentPlayer.ChangeHat(curEquipHat);
                break;
            case PantType:
                curPantList[curEquipPant] = BuyState.Bought;
                curPantList[(PantType)currentItem.itemID] = BuyState.Equipped;
                curEquipPant = (PantType)currentItem.itemID;
                LevelManager.Ins.currentPlayer.ChangePant(curEquipPant);
                break;
            case AccessoryType:
                curAccessoryList[curEquipAcc] = BuyState.Bought;
                curAccessoryList[(AccessoryType)currentItem.itemID] = BuyState.Equipped;
                curEquipAcc = (AccessoryType)currentItem.itemID;
                LevelManager.Ins.currentPlayer.DespawnAccessory();
                LevelManager.Ins.currentPlayer.ChangeAccessory(curEquipAcc);
                break;
            case SkinType:
                curSkinList[curEquipedSkin] = BuyState.Bought;
                curSkinList[(SkinType)currentItem.itemID] = BuyState.Equipped;
                curEquipedSkin = (SkinType)currentItem.itemID;
                LevelManager.Ins.currentPlayer.TakeOffSkin();
                LevelManager.Ins.currentPlayer.curSkinType = curEquipedSkin;
                LevelManager.Ins.currentPlayer.ChangeClothes();
                break;
            default:
                break;
        }

        SelectItem(currentItem);
        DataPersistenceManager.Ins.SendData(this);
    }

    #endregion

}

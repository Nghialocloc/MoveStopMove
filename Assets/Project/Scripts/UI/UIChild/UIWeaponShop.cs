using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIWeaponShop : UICanvas, IDataPersistence
{
    [Header("Info")]
    [SerializeField] private TextMeshProUGUI playerCoinTxt;
    [SerializeField] private WeaponData weaponData;

    [Header("UI display")]
    public Transform weaponDisplay;
    [SerializeField] private GameObject[] buttonObjects;
    [SerializeField] private TextMeshProUGUI nameTxt;
    [SerializeField] private TextMeshProUGUI decriTxt;
    [SerializeField] private TextMeshProUGUI priceTxt;

    // Du lieu item
    private int playerCoin;
    private WeaponType weaponType;
    private WeaponBase curDisplay;
    private SerilizableDictionary<WeaponType, BuyState> curWeaponList;

    #region Load and Save

    public void LoadData(GameData data)
    {
        playerCoin = data._Key_Coin;
        curWeaponList = data.weaponList;

        // Khoi tao ban dau khi chua luu du lieu
        if (curWeaponList.Keys.Count == 0)
        {
            foreach (WeaponData.WeaponItemData weapon in weaponData.WeaponItems)
            {
                curWeaponList.Add(weapon.type, BuyState.Buy);
            }
            curWeaponList[WeaponType.W_Arrow] = BuyState.Equipped;
        }
    }

    public void SaveData(ref GameData data)
    {
        data._Key_Coin = playerCoin;
        data.weaponList = curWeaponList;
    }

    #endregion

    public override void Open()
    {
        base.Open();
        GameManager.Ins.ChangeState(GameState.WeaponShop);
        DataPersistenceManager.Ins.CallData(this);
        playerCoinTxt.text = playerCoin.ToString();
        weaponType = (WeaponType) 2000;
        ChangeWeapon(weaponType);
        AudioManager.Ins.MusicControl();
    }

    public override void CloseDirectly()
    {
        base.CloseDirectly();
        UIManager.Ins.OpenUI<UIMainMenu>();
    }

    #region Select Item

    public void NextWeapon()
    {
        AudioManager.Ins.PlaySfx(Constants.EFF_CLICK);
        ChangeWeapon(weaponData.NextType(weaponType));
    }

    public void PrevWeapon()
    {
        AudioManager.Ins.PlaySfx(Constants.EFF_CLICK);
        ChangeWeapon(weaponData.PrevType(weaponType));
    }

    public void ChangeWeapon(WeaponType weaponType)
    {
        // Chuyen loai vu khi
        this.weaponType = weaponType;

        if (curDisplay != null)
        {
            SimplePool.Despawn(curDisplay);
        }
        WeaponData.WeaponItemData item = weaponData.GetWeaponItem(weaponType);
        curDisplay = SimplePool.Spawn<WeaponBase>(item.prefab, Vector3.zero, Quaternion.identity, weaponDisplay);
        
        // Chuyen trang thai nut
        curWeaponList.TryGetValue(weaponType, out BuyState value);
        switch(value)
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
            default:
                break;
        }

        nameTxt.SetText(item.name);
        decriTxt.SetText(item.description);
        priceTxt.SetText(item.cost.ToString());
        
    }

    public void SetState(BuyState state)
    {
        for (int i = 0; i < buttonObjects.Length; i++)
        {
            buttonObjects[i].SetActive(false);
        }

        buttonObjects[(int)state].SetActive(true);
    }

    #endregion

    #region Buy Item

    public void BuyButton()
    {
        AudioManager.Ins.PlaySfx(Constants.EFF_CLICK);
        // Kiem tra xem co du tien mua vu khi ko
        int index = weaponData.WeaponItems.FindIndex(q => q.type == weaponType);
        if (playerCoin < weaponData.WeaponItems[index].cost)
        {
            return;
        }

        playerCoin = playerCoin - weaponData.WeaponItems[index].cost;
        playerCoinTxt.text = playerCoin.ToString();

        curWeaponList[weaponType] = BuyState.Bought;
        ChangeWeapon(weaponType);
        DataPersistenceManager.Ins.SendData(this);
    }

    public void EquipButton()
    {
        AudioManager.Ins.PlaySfx(Constants.EFF_CLICK);
        // Xoa vu khi cu , dat lai state, chuyen vu khi duoc chon la eqiupped, dat lai vu khi moi và sinh no ra
        LevelManager.Ins.currentPlayer.DespawnWeapon();
        curWeaponList[LevelManager.Ins.currentPlayer.curWeaponType] = BuyState.Bought;
        curWeaponList[weaponType] = BuyState.Equipped;
        LevelManager.Ins.currentPlayer.curWeaponType = weaponType;
        LevelManager.Ins.currentPlayer.ChangeWeapon(weaponType);
        ChangeWeapon(weaponType);
    }

    #endregion
}

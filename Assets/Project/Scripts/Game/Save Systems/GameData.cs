using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    //Main menu
    public int _Key_Level;
    public int _Key_Coin;
    public bool _Key_Vibrate;
    public bool _Key_RemoveAds;

    //Shop
    public SerilizableDictionary<WeaponType, BuyState> weaponList;
    public SerilizableDictionary<HatType, BuyState> hatList;
    public SerilizableDictionary<PantType, BuyState> pantList;
    public SerilizableDictionary<AccessoryType, BuyState> accessoryList;
    public SerilizableDictionary<SkinType, BuyState> skinList;

    public WeaponType equipedWeapon;
    public HatType equipedHat;
    public PantType equipedPant;
    public AccessoryType equipedAcc;
    public SkinType equipedSkin;

    //Sound
    public float _Value_Music;
    public float _Value_Sfx;

    //Level
    public SerilizableDictionary<int, LevelState> levelList;

    // Chi dinh gia tri ban dau cua game
    // Bat dau game khi khong co du lieu de load
    public GameData()
    {
        // Game Setting
        this._Key_Level = 0;
        this._Key_Coin = 0;
        this._Key_Vibrate = true;
        this._Key_RemoveAds = false;

        // Shop Data
        this.weaponList = new SerilizableDictionary<WeaponType, BuyState>();
        this.hatList = new SerilizableDictionary<HatType, BuyState>();
        this.pantList = new SerilizableDictionary<PantType, BuyState>();
        this.accessoryList = new SerilizableDictionary<AccessoryType, BuyState>();
        this.skinList = new SerilizableDictionary<SkinType, BuyState>();
        equipedWeapon = WeaponType.W_Arrow;
        equipedHat = HatType.HAT_None;
        equipedPant = PantType.PANT_None;
        equipedAcc = AccessoryType.ACC_None;
        equipedSkin = SkinType.SKIN_None;

        //Sound
        this._Value_Music = 1;
        this._Value_Sfx = 1;
        this.levelList = new SerilizableDictionary<int, LevelState>();
}
}

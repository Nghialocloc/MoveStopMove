using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constants
{
    //Animation cho nhan vat
    public const string ANIM_IDLE = "idle";
    public const string ANIM_RUN = "run";
    public const string ANIM_ATTACK = "attack";
    public const string ANIM_DANCE = "dance";
    public const string ANIM_WIN = "win";
    public const string ANIM_DIE = "die";

    //Tag cho GameObject
    public const string TAG_CHARACTER = "Player";
    public const string TAG_OBSTACLE = "Obstacle";

    //Animation cho UI
    public const string UI_ANIM_OPEN = "shift";
    public const string UI_ANIM_CLOSE = "shift_back";

    //Music
    public const string MENU_THEME = "Menu";
    public const string BATTLE_THEME = "Battle";

    //Efect
    public const string EFF_CLICK = "Click";
    public const string EFF_HIT = "Hit";
    public const string EFF_THROW = "Throw";
    public const string EFF_COUNTDOWN = "Countdown";
    public const string EFF_REVIVE = "Revive";
    public const string EFF_VICTORY = "Victory";
    public const string EFF_LOSE = "Lose";

}

// Trang thai cua game
public enum GameState
{
    MainMenu,
    Gameplay,
    Setting,
    WeaponShop,
    SkinShop,
    Win,
    Lose,
    Revive,
}

#region Pooling

// Hieu ung cho khi kich hoat pooling
public enum ParticleType
{
    None = 0,

    Hit_1 = 101,
    Hit_2 = 102,
    Hit_3 = 103,

    LevelUp_1 = 201,
    LevelUp_2 = 202,
    LevelUp_3 = 203,
}

// Loai pool
public enum PoolType
{
    None,
    Bot = 1,
    TargetIndicator = 3,

    // Dan nem ra cua nhan vat
    B_Arrow = 1000,
    B_Knife = 1001,
    B_LolipopCandy = 1002,
    B_ConeCandy = 1003,
    B_ShepreCandy = 1004,
    B_IceCream = 1005,
    B_SingleAxe = 1006,
    B_DoubleAxe = 1007,
    B_Hammer = 1008,
    B_Boomerang = 1009,
    B_Uzi = 1010,
    B_LetterZ = 1011,

    // Mo hinh vu khi nhan vat
    W_Arrow = 2000,
    W_Knife = 2001,
    W_LolipopCandy = 2002,
    W_ConeCandy = 2003,
    W_ShepreCandy = 2004,
    W_IceCream = 2005,
    W_SingleAxe = 2006,
    W_DoubleAxe = 2007,
    W_Hammer = 2008,
    W_Boomerang = 2009,
    W_Uzi = 2010,
    W_LetterZ = 2011,

    // Mu cua nhan vat
    HAT_Arrow = 3000,
    HAT_Cowboy = 3001,
    HAT_Crown = 3002,
    HAT_BunnyEar = 3003,
    HAT_Cap = 3005,
    HAT_StrawHat = 3006,
    HAT_Headphone = 3007,
    HAT_Horn = 3008,
    HAT_Police = 3009,
    HAT_Rau = 3010,

    // Do phu kien them cgo nguoi choi
    ACC_Shield = 4000,
    ACC_Captain = 4001,

    // Full set skin cho nhan vat
    SKIN_Normal = 5000,
    SKIN_Devil = 5001,
    SKIN_Angle = 5002,
    SKIN_Witch = 5003,
    SKIN_Deadpool = 5004,
    SKIN_Thor = 5005,

}

#endregion

#region Character Customize

//Dan ban ra dua theo vu khi nhan vat
public enum BulletType
{
    B_Arrow = PoolType.B_Arrow,
    B_Knife = PoolType.B_Knife,
    B_LolipopCandy = PoolType.B_LolipopCandy,
    B_ConeCandy = PoolType.B_ConeCandy,
    B_ShepreCandy = PoolType.B_ShepreCandy,
    B_IceCream = PoolType.B_IceCream,
    B_SingleAxe = PoolType.B_SingleAxe,
    B_DoubleAxe = PoolType.B_DoubleAxe,
    B_Hammer = PoolType.B_Hammer,
    B_Boomerang = PoolType.B_Boomerang,
    B_Uzi = PoolType.B_Uzi,
    B_LetterZ = PoolType.B_LetterZ,
}

// Weapon cho nhan vat
public enum WeaponType 
{
    W_Arrow = PoolType.W_Arrow,
    W_Knife = PoolType.W_Knife,
    W_LolipopCandy = PoolType.W_LolipopCandy,
    W_ConeCandy = PoolType.W_ConeCandy,
    W_ShepreCandy = PoolType.W_ShepreCandy,
    W_IceCream = PoolType.W_IceCream,
    W_SingleAxe = PoolType.W_SingleAxe,
    W_DoubleAxe = PoolType.W_DoubleAxe,
    W_Hammer = PoolType.W_Hammer,
    W_Boomerang = PoolType.W_Boomerang,
    W_Uzi = PoolType.W_Uzi,
    W_LetterZ = PoolType.W_LetterZ,
}

// Hat cho nhan vat
public enum HatType
{
    HAT_None = 0,
    HAT_Arrow = PoolType.HAT_Arrow,
    HAT_Cowboy = PoolType.HAT_Cowboy,
    HAT_Crown = PoolType.HAT_Crown,
    HAT_BunnyEar = PoolType.HAT_BunnyEar,
    HAT_Cap = PoolType.HAT_Cap,
    HAT_StrawHat = PoolType.HAT_StrawHat,
    HAT_Headphone = PoolType.HAT_Headphone,
    HAT_Horn = PoolType.HAT_Horn,
    HAT_Police = PoolType.HAT_Police,
    HAT_Rau = PoolType.HAT_Rau,
}

// Pant cho nhan vat
public enum PantType
{
    PANT_None = 0,
    PANT_SpotPink = 1,
    PANT_RedYellow = 2,
    PANT_BluePink = 3,
    PANT_Rainbow = 4,
    PANT_Panther = 5,
    PANT_RedDeath = 6,
    PANT_Pokeball = 7,
    PANT_BlackYellow = 8,
    PANT_America = 9,
}

// Accessory cho nhan vat
public enum AccessoryType
{
    ACC_None = 0,
    ACC_CaptainShield = PoolType.ACC_Captain,
    ACC_Shield = PoolType.ACC_Shield,
}

// Skin cho nhan vat
public enum SkinType
{
    SKIN_None = PoolType.SKIN_Normal,
    SKIN_Devil = PoolType.SKIN_Devil,
    SKIN_Angle = PoolType.SKIN_Angle,
    SKIN_Witch = PoolType.SKIN_Witch,
    SKIN_Deadpool = PoolType.SKIN_Deadpool,
    SKIN_Thor = PoolType.SKIN_Thor,
}

#endregion

#region Shop

//Lua chon trong Skin Shop
public enum SkinShopCatergory
{
    Hat,
    Pant,
    Accessory,
    Skin,
}

public enum BuyState 
{
    Buy = 0,
    Bought = 1, 
    Equipped = 2,
    Selecting = 3,
    None = 4,
}


#endregion

#region Level

public enum LevelState
{
    Locked = 0,
    Load = 1,
    Loaded = 2,
    Selecting = 3,
    None = 4,
}

#endregion
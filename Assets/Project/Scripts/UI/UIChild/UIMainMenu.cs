using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor.UI;
using UnityEngine.UI;

public class UIMainMenu : UICanvas, IDataPersistence
{
    [Header("Panel")]
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private GameObject soundPanel;

    [Header("Component")]
    [SerializeField] private TextMeshProUGUI playerCoinTxt;
    [SerializeField] private TextMeshProUGUI playerLevelTxt;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider effectSlider;
    [SerializeField] private Animator anim;

    [Header("Button Control")]
    [SerializeField] private GameObject offVib;
    [SerializeField] private GameObject onVib;
    [SerializeField] private GameObject offAds;
    [SerializeField] private GameObject onAds;

    private int playerCoin;
    private int playerLevel;
    private bool isVibOn;
    private bool isAdsOff;

    #region Load and Save

    public void LoadData(GameData data)
    {
        playerCoin = data._Key_Coin;
        playerLevel = data._Key_Level;
        isVibOn = data._Key_Vibrate;
        isAdsOff = data._Key_RemoveAds;
        musicSlider.value = data._Value_Music;
        effectSlider.value = data._Value_Sfx;
    }

    public void SaveData(ref GameData data)
    {
        data._Key_Coin = playerCoin;
        data._Key_Level = playerLevel;
        data._Key_Vibrate = isVibOn;
        data._Key_RemoveAds = isAdsOff;
        data._Value_Music = musicSlider.value;
        data._Value_Sfx = effectSlider.value;
    }

    #endregion

    public override void Open()
    {
        base.Open();
        mainPanel.SetActive(true);
        soundPanel.SetActive(false);
        anim.SetTrigger(Constants.UI_ANIM_OPEN);
        GameManager.Ins.ChangeState(GameState.MainMenu);
        GameManager.Ins.CamControl(GameState.MainMenu);

        // Goi du lieu len va cap nhat toan bo thong tin
        DataPersistenceManager.Ins.CallData(this);
        UpdateText();
        UpdateVib();
        UpdateAds();
        AudioManager.Ins.MusicControl();
    }

    #region Change Settings

    public void UpdateText()
    {
        playerCoinTxt.text = playerCoin.ToString();
        playerLevelTxt.text = "Level " + ( playerLevel + 1).ToString();
    }


    public void UpdateVib()
    {
        if (isVibOn)
        {
            offVib.SetActive(false);
            onVib.SetActive(true);
        }
        else
        {
            onVib.SetActive(false);
            offVib.SetActive(true);
        }
    }

    public void UpdateAds()
    {
        if (isAdsOff)
        {
            onAds.SetActive(false);
            offAds.SetActive(true);
        }
        else
        {
            offAds.SetActive(false);
            onAds.SetActive(true);
        }
    }

    public void ChangeVib()
    {
        isVibOn = !isVibOn;
        DataPersistenceManager.Ins.SendData(this);
        UpdateVib();
    }

    public void ChangeAds()
    {
        isAdsOff = !isAdsOff;
        DataPersistenceManager.Ins.SendData(this);
        UpdateAds();
    }

    #endregion

    #region Change UI

    public void SkinShopButton()
    {
        AudioManager.Ins.PlaySfx(Constants.EFF_CLICK);
        UIManager.Ins.OpenUI<UISkinShop>();
        UIManager.Ins.CloseUI<UIMainMenu>();
    }


    public void WeaponShopButton()
    {
        AudioManager.Ins.PlaySfx(Constants.EFF_CLICK);
        UIManager.Ins.OpenUI<UIWeaponShop>();
        UIManager.Ins.CloseUI<UIMainMenu>();
    }

    public void LevelButton()
    {
        AudioManager.Ins.PlaySfx(Constants.EFF_CLICK);
        UIManager.Ins.OpenUI<UILevelSetting>();
        UIManager.Ins.CloseUI<UIMainMenu>();
    }

    public void PlayButton()
    {
        AudioManager.Ins.PlaySfx(Constants.EFF_CLICK);
        anim.SetTrigger(Constants.UI_ANIM_CLOSE);
        StartCoroutine(UIShow());
    }

    public IEnumerator UIShow()
    {
        yield return new WaitForSeconds(0.5f);
        UIManager.Ins.OpenUI<UIGamplay>();
        UIManager.Ins.CloseUI<UIMainMenu>();
    }

    #endregion

    #region Change Sound

    public void MusicVolume()
    {
        AudioManager.Ins.MusicVolume(musicSlider.value);
        DataPersistenceManager.Ins.SendData(this);
    }

    public void EffectVolume()
    {
        AudioManager.Ins.SfxVolume(effectSlider.value);
        DataPersistenceManager.Ins.SendData(this);
    }

    #endregion

    #region Change Panel

    public void SoundToMainPanel()
    {
        soundPanel.SetActive(false);
        mainPanel.SetActive(true);
    }

    public void MainToSoundPanel()
    {
        mainPanel.SetActive(false);
        soundPanel.SetActive(true);   
    }

    #endregion

}

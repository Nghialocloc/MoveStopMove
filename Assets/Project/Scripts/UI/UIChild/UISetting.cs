using UnityEngine;
using UnityEngine.UI;

public class UISetting : UICanvas, IDataPersistence
{
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider effectSlider;

    #region Load and Save

    public void LoadData(GameData data)
    {
        musicSlider.value = data._Value_Music;
        effectSlider.value = data._Value_Sfx;

    }

    public void SaveData(ref GameData data)
    {
        data._Value_Music = musicSlider.value;
        data._Value_Sfx = effectSlider.value;
    }

    #endregion

    public override void Open()
    {
        base.Open();
        GameManager.Ins.ChangeState(GameState.Setting);
        GameManager.Ins.CamControl(GameState.Setting);
        DataPersistenceManager.Ins.CallData(this);
        AudioManager.Ins.MusicControl();
    }

    public void ContinueButton()
    {
        AudioManager.Ins.PlaySfx(Constants.EFF_CLICK);
        UIManager.Ins.CloseUI<UISetting>();
        UIManager.Ins.OpenUI<UIGamplay>();
    }

    public void HomeButton()
    {
        AudioManager.Ins.PlaySfx(Constants.EFF_CLICK);
        UIManager.Ins.CloseAll();
        LevelManager.Ins.OnReset();
    }

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
}

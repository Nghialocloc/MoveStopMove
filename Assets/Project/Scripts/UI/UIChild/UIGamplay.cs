using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIGamplay : UICanvas
{
    public TextMeshProUGUI characterAmountTxt;

    public override void Setup()
    {
        base.Setup();
        UpdateNumberEnemy()
;    }

    public override void Open()
    {
        base.Open();
        GameManager.Ins.ChangeState(GameState.Gameplay);
        GameManager.Ins.CamControl(GameState.Gameplay);
        LevelManager.Ins.SetTargetIndicatorAlpha(1);
        AudioManager.Ins.MusicControl();
    }

    public override void CloseDirectly()
    {
        base.CloseDirectly();
        LevelManager.Ins.SetTargetIndicatorAlpha(0);
    }

    public void SettingButton()
    {
        UIManager.Ins.OpenUI<UISetting>();
        UIManager.Ins.CloseUI<UIGamplay>();
    }

    public void UpdateNumberEnemy()
    {
        characterAmountTxt.text = LevelManager.Ins.totalBotNumber.ToString();
    }
}

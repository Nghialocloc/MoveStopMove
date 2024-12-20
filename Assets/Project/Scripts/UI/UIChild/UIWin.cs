using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIWin : UICanvas
{
    [SerializeField] private TextMeshProUGUI coinTxt;

    public override void Open()
    {
        base.Open();
        GameManager.Ins.ChangeState(GameState.Win);
        GameManager.Ins.CamControl(GameState.Win);
        AudioManager.Ins.MusicControl();
        AudioManager.Ins.PlaySfx(Constants.EFF_VICTORY);
    }

    public void HomeButton()
    {
        UIManager.Ins.CloseAll();
        LevelManager.Ins.OnReset();
    }

    public void SetCoin(int coin)
    {
        coinTxt.SetText(coin.ToString());
    }
}

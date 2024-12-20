using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UILose : UICanvas
{
    [SerializeField] private TextMeshProUGUI coinTxt;
    [SerializeField] private TextMeshProUGUI rankTxt;

    public override void Open()
    {
        base.Open();
        GameManager.Ins.ChangeState(GameState.Lose);
        GameManager.Ins.CamControl(GameState.Lose);
        SetCoin(LevelManager.Ins.Coin);
        SetRank(LevelManager.Ins.totalBotNumber);
        AudioManager.Ins.MusicControl();
        AudioManager.Ins.PlaySfx(Constants.EFF_LOSE);
    }

    public void HomeButton()
    {
        UIManager.Ins.CloseAll();
        LevelManager.Ins.OnReset();
    }

    public void SetRank(int rank)
    {
        rankTxt.SetText("Rank #" + rank);
    }

    public void SetCoin(int coin)
    {
        coinTxt.SetText(coin.ToString());
    }
}

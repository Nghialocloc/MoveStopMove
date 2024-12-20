using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIRevive : UICanvas
{
    [SerializeField] private TextMeshProUGUI counterTxt;
    [SerializeField] private float time = 5;
     private float counter;

    public override void Open()
    {
        base.Open();
        GameManager.Ins.ChangeState(GameState.Revive);
        GameManager.Ins.CamControl(GameState.Revive);
        counter = time;
        counterTxt.SetText(counter.ToString());
        AudioManager.Ins.MusicControl();
    }

    private void Update()
    {
        if (counter > 0)
        {
            counter -= Time.deltaTime;
            counterTxt.SetText(counter.ToString("F0"));
            AudioManager.Ins.PlaySfx(Constants.EFF_COUNTDOWN);

            if (counter <= 0)
            {
                CloseButton();
            }
        }
    }

    public void ReviveButton()
    {
        AudioManager.Ins.PlaySfx(Constants.EFF_CLICK);
        UIManager.Ins.CloseUI<UIRevive>();
        UIManager.Ins.OpenUI<UIGamplay>();
        LevelManager.Ins.currentPlayer.OnRespawn();
    }

    public void CloseButton()
    {
        UIManager.Ins.CloseUI<UIRevive>();
        UIManager.Ins.OpenUI<UILose>();
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : Singleton<GameManager>
{
    private static GameState gameState;
    public new CameraFollower camera;
    public Camera screen;

    // Start is called before the first frame update
    protected void Awake()
    {
        //base.Awake();
        Input.multiTouchEnabled = false;
        Application.targetFrameRate = 60;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        int maxScreenHeight = 1280;
        float ratio = (float)Screen.currentResolution.width / (float)Screen.currentResolution.height;
        if (Screen.currentResolution.height > maxScreenHeight)
        {
            Screen.SetResolution(Mathf.RoundToInt(ratio * (float)maxScreenHeight), maxScreenHeight, true);
        }
    }

    public void OnInit()
    {
        ChangeState(GameState.MainMenu);
        CamControl(GameState.MainMenu);
        UIManager.Ins.CloseAll();
        UIManager.Ins.OpenUI<UILoading>();
    }

    #region State

    public void ChangeState(GameState state)
    {
        gameState = state;
    }

    public bool IsState(GameState state)
    {
        return gameState == state;
    }

    #endregion

    #region Camera

    public void CamControl(GameState state)
    {
        switch (state)
        {
            case GameState.MainMenu or GameState.WeaponShop :
                camera.MenuCam();
                break;
            case GameState.Gameplay or GameState.Lose or GameState.Setting or GameState.Revive:
                camera.GameCam();
                break;
            case GameState.Win:
                camera.WinCam();
                break;
            case GameState.SkinShop:
                camera.ShopCam();
                break;
            default:
                break;
        }
    }

    #endregion
}

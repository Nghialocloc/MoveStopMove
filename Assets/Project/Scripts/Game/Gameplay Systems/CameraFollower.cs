using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    public Transform player;
    [SerializeField] private float smoothCamera = 5f;
    [SerializeField] private Vector3 menuView;
    [SerializeField] private Vector3 shopView;
    [SerializeField] private Vector3 gameplayView;
    [SerializeField] private Vector3 winView;

    [Header("Offset")]
    [SerializeField] private Vector3 offset;
    [SerializeField] Vector3 offsetMax;

    private Quaternion targetRotate;

    // Start is called before the first frame update
    void Awake()
    {
        menuView = new Vector3(0,3f,-7f);
        shopView = new Vector3(0,3f,-9f);
        gameplayView = new Vector3(0,14,-9);
        winView = new Vector3(0,2,-8);
        offsetMax = new Vector3(0,29,-17);
    }

    // Update is called once per frame
    private void LateUpdate()
    {
        if (player != null)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotate, smoothCamera);
            transform.position = Vector3.Slerp(transform.position, player.position + offset, smoothCamera);
        }
            
    }

    #region Cam Position

    public void MenuCam()
    {
        offset = menuView;
        targetRotate.eulerAngles = new Vector3(20, 0, 0);
    }

    public void ShopCam()
    {
        offset = shopView;
        targetRotate.eulerAngles = new Vector3(30, 0, 0);
    }

    public void GameCam()
    {
        offset = gameplayView;
        targetRotate.eulerAngles = new Vector3(60, 0, 0);
    }

    public void WinCam()
    {
        offset = winView;
        targetRotate.eulerAngles = new Vector3(0, 0, 0);
    }

    #endregion

    public void SetRateOffset(float rate)
    {
        offset = Vector3.Lerp(offset, offsetMax, rate);
    }
}

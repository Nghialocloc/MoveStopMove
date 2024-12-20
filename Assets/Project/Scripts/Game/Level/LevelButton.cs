using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class LevelButton : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Color[] bgColor;
    [SerializeField] private Image bgIcon;
    [SerializeField] private TextMeshProUGUI nameTxt;
    [SerializeField] private LevelState state;
    public int levelIndex;

    private UILevelSetting ui;

    public void Select()
    {
        ui.SelectLevel(this);
    }

    // Kich hoat background hien thi trang thai item
    public void SetState(LevelState state)
    {
        bgIcon.color = bgColor[(int)state];
    }

    public LevelState GetState()
    {
        return state;
    }

    public void SetData(int index, string name , LevelState value,UILevelSetting ui)
    {
        this.ui = ui;
        levelIndex = index;
        nameTxt.SetText("Level " + (index + 1) + " : " + name);
        state = value;
    }
}

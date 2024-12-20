using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILoading : UICanvas
{

    [SerializeField] private Animator anim;

    public override void Open()
    {
        base.Open();
        AudioManager.Ins.ResetSound();
        anim.SetTrigger("fade");
        StartCoroutine(UIShow());
    }

    public IEnumerator UIShow()
    {
        yield return new WaitForSeconds(2f);
        UIManager.Ins.OpenUI<UIMainMenu>();
        StartCoroutine(Close());
    }

    public IEnumerator Close()
    {
        yield return new WaitForSeconds(0.5f);
        CloseDirectly();
    }

}

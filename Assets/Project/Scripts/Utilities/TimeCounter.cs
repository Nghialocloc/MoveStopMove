using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TimeCounter
{
    // Bo dem thoi gian cho cac event Unity
    private UnityAction activateAction;
    private float countTime;
    public bool IsAlreadyAssign => countTime > 0;

    public void Assign(UnityAction action, float time)
    {
        activateAction = action;
        countTime = time;
    }

    public void Count()
    {
        if (countTime > 0)
        {
            countTime -= Time.deltaTime;
            if (countTime <= 0)
            {
                Execute();
            }
        }
    }

    public void Execute()
    {
        activateAction?.Invoke();
    }

    public void Cancel()
    {
        activateAction = null;
        countTime = -1;
    }
}

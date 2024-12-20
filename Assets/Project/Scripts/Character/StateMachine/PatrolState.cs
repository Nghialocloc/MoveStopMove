using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : IState<Bot>
{
    public void OnEnter(Bot bot)
    {
        bot.SetDestination(LevelManager.Ins.currentLevel.RandomPoint());
    }

    public void OnExecute(Bot bot)
    {
        if (bot.IsDestination)
        {
            bot.ChangeState(new IdleState());
        }
    }

    public void OnExit(Bot bot)
    {

    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IState<Bot>
{
    public void OnEnter(Bot bot)
    {
        bot.StopMovement();
        bot.ChangeAnim(Constants.ANIM_IDLE);
        if(LevelManager.Ins.currentPlayer.IsDead != true)
        {
            bot.botCounter.Assign(
            () => bot.ChangeState(new PatrolState()),
            Random.Range(0, 2f)
            );
        }
    }

    public void OnExecute(Bot bot)
    {
        bot.botCounter.Count();
    }

    public void OnExit(Bot bot)
    {

    }

}

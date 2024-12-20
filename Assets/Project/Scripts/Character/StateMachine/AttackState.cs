using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IState<Bot>
{
    public void OnEnter(Bot bot)
    {
        bot.StopMovement();
        bot.OnAttack();
        if (bot.IsCanAttack)
        {
            bot.botCounter.Assign(
                () =>
                {
                    bot.ThrowWeapon();
                    bot.botCounter.Assign(
                        () => bot.ChangeState(new PatrolState()),
                        bot.GetAttackCountdown()
                        );
                },
                bot.GetAttackCountdown()
                );
        }
        else
        {
            bot.ChangeState(new PatrolState());
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

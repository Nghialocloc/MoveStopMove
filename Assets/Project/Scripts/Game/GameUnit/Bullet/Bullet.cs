using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : GameUnit
{
    protected Character character;
    protected float moveSpeed = 5f;
    protected bool isFlying;
    [SerializeField] protected Transform model;

    public virtual void OnInit(Character character, Vector3 target, float size, float speed)
    {
        this.character = character;
        TF.forward = (target - TF.position).normalized;
        TF.localScale = size * Vector3.one;
        moveSpeed = moveSpeed * speed;
        isFlying = true;
    }

    public virtual void OnDespawn()
    {
        SimplePool.Despawn(this);
    }

    protected virtual void OnStop()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Constants.TAG_CHARACTER))
        {
            Character hit = Cache.GetCharacter(other);
            if(hit == character)
            {
                Physics.IgnoreCollision(hit.GetCollider(), character.GetCollider(), true);
            }
            else
            {
                if (!hit.IsDead)
                {
                    hit.OnHit();
                    character.AddScore(1);
                    character.RemoveTarget(hit);
                    ParticlePool.Play(Utilities.RandomInMember(ParticleType.Hit_1, ParticleType.Hit_2, ParticleType.Hit_3), TF.position, TF.rotation);  
                }
                SimplePool.Despawn(this);
            }
        }

        if (other.CompareTag(Constants.TAG_OBSTACLE))
        {
            OnStop();
        }

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBoomerang : Bullet
{
    private enum State 
    { 
        Forward, 
        Backward, 
        Stop 
    }
    private float flyingTime;
    private Vector3 target;
    private State state;

    public override void OnInit(Character character, Vector3 target, float size, float speed)
    {
        base.OnInit(character, target, size, speed);
        flyingTime = (size / speed);
        this.target = (target - character.TF.position).normalized * 5f * size + character.TF.position;
        state = State.Forward;
    }

    protected override void OnStop()
    {
        base.OnStop();
        state = State.Stop;
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case State.Forward:
                TF.position = Vector3.MoveTowards(TF.position, this.target, moveSpeed * Time.deltaTime);
                if (Vector3.Distance(TF.position, target) < 0.1f)
                {
                    state = State.Backward;
                    Invoke(nameof(OnDespawn), flyingTime);
                }
                model.Rotate(Vector3.up * -6, Space.Self);
                break;

            case State.Backward:
                TF.position = Vector3.MoveTowards(TF.position, this.character.TF.position, moveSpeed * Time.deltaTime);
                if (character.IsDead || Vector3.Distance(TF.position, this.character.TF.position) < 0.1f)
                {
                    OnDespawn();
                }
                model.Rotate(Vector3.up * -6, Space.Self);

                break;
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletRotate : Bullet
{
    private float flyingTime;
    TimeCounter counter = new TimeCounter();

    public override void OnInit(Character character, Vector3 target, float size, float speed)
    {
        base.OnInit(character, target, size, speed);
        flyingTime = (size / speed);
        counter.Assign(OnDespawn, flyingTime);
    }


    protected override void OnStop()
    {
        base.OnStop();
        isFlying = false;
    }

    // Update is called once per frame
    private void Update()
    {
        counter.Count();
        if (isFlying)
        {
            TF.Translate(moveSpeed * Time.deltaTime * TF.forward, Space.World);
            model.Rotate(Vector3.up * -6, Space.Self);
        }
    }

}

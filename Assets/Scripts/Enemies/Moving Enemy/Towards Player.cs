using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowardsPlayer : MovingEnemy
{
    private Vector3 upOrDown;

    protected override void Start()
    {
        base.Start();
        upOrDown = Vector3.down;
    }

    protected override void Moving()
    {
        base.Moving();
        transform.position = Vector3.Lerp(transform.position, transform.position + upOrDown, 0.75f * Time.deltaTime);
        if (transform.position.y < -5)
        {
            upOrDown = Vector3.up;
        }
        else if (transform.position.y > 5)
        {
            upOrDown = Vector3.down;
        }
    }
}

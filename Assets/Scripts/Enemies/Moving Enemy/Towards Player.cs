using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowardsPlayer : MovingEnemy
{
    protected override void Moving()
    {
        base.Moving();
        transform.position = Vector3.Lerp(transform.position, transform.position + Vector3.down, 1 * Time.deltaTime);
        if (transform.position.y < -7)
        {
            Destroy(gameObject);
        }
    }
}

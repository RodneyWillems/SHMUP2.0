using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowardsDirection : ShootingEnemy
{
    protected override void Start()
    {
        base.Start();
    }

    protected override IEnumerator Shooting()
    {
        while (true)
        {
            yield return base.Shooting();
        }
    }

    protected override void Update()
    {
        base.Update();
    }
}

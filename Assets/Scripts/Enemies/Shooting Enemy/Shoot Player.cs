using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootPlayer : ShootingEnemy
{

    protected override void Start()
    {
        base.Start();
    }

    protected override IEnumerator Shooting()
    {
        while (true && m_player != null)
        {
            m_shotDirection = m_player.transform.position - transform.position;
            yield return base.Shooting();
        }
    }

    protected override void Update()
    {
        base.Update();
        if (m_inPosition && m_player != null)
        {
            transform.up = transform.position - m_player.transform.position;
        }
    }
}

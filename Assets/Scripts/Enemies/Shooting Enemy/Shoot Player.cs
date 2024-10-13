using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootPlayer : ShootingEnemy
{
    protected Transform m_player;

    protected override void Start()
    {
        base.Start();
        m_player = FindObjectOfType<PlayerMovement>().transform;
    }

    protected override IEnumerator Shooting()
    {
        while (true)
        {
            Vector3 playerPosition = m_player.position;
            playerPosition.z = 0;

            Vector3 myPos = transform.position;
            playerPosition.x = playerPosition.x - myPos.x;
            playerPosition.y = playerPosition.y - myPos.y;

            float angle = Mathf.Atan2(playerPosition.y, playerPosition.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
            m_shotDirection = transform.forward;
            return base.Shooting();
        }
    }

    protected override void Update()
    {
        base.Update();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootPlayer : ShootingEnemy
{
    public float m_cooldown;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    protected IEnumerator Shooting()
    {
        yield return new WaitForSeconds(m_cooldown);
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
}

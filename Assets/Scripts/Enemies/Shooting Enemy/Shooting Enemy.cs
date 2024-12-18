using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingEnemy : BaseEnemy
{
    public float m_cooldown;
    public GameObject m_bulletPrefab;

    [SerializeField] protected Transform m_bulletSpawnPoint;

    protected Coroutine m_coroutine;
    protected Vector2 m_shotDirection;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        m_bulletSpawnPoint = transform.GetChild(0);
        m_shotDirection = Vector2.down;
    }

    protected virtual IEnumerator Shooting()
    {
        GameObject bullet = Instantiate(m_bulletPrefab, m_bulletSpawnPoint.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(m_shotDirection.x, m_shotDirection.y).normalized * 10;
        bullet.transform.up = m_shotDirection;
        bullet.tag = transform.tag;
        bullet.layer = gameObject.layer;
        Destroy(bullet, 3);
        yield return new WaitForSeconds(m_cooldown);
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (m_inPosition && m_coroutine == null) 
        {
            m_coroutine = StartCoroutine(Shooting());
        }
    }
}

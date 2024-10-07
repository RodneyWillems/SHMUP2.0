using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingEnemy : BaseEnemy
{
    [SerializeField] protected float m_moveSpeed;
    [SerializeField] protected Vector2 m_direction;

    protected Rigidbody2D m_rb;

    protected override void Start()
    {
        base.Start();
        m_rb = GetComponent<Rigidbody2D>();
    }

    protected virtual void Update()
    {
        if (!m_inPosition)
        {
            transform.position = Vector3.Lerp(transform.position, m_startPositon, 3 * Time.deltaTime);
            m_inPosition = Vector3.Distance(transform.position, m_startPositon) <= 0.5f;
        }
    }
}

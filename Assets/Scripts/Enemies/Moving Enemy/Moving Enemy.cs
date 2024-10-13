using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingEnemy : BaseEnemy
{
    public float m_moveSpeed;

    [SerializeField] protected Vector2 m_direction;

    protected Rigidbody2D m_rb;

    protected override void Start()
    {
        base.Start();
        m_rb = GetComponent<Rigidbody2D>();
        int randomDirection = Random.Range(0, 2);
        if (randomDirection == 0)
        {
            m_direction = Vector2.left;
        }
        else
        {
            m_direction = Vector2.right;
        }
    }

    protected virtual void Moving()
    {
        // Screen wrapper
        float rightScreenEdge = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height)).x - 0.5f;
        float leftScreenEdge = -rightScreenEdge + 0.35f;

        if (transform.position.x < leftScreenEdge)
        {
            m_rb.velocity = Vector2.zero;
            m_direction = Vector2.right;
        }
        else if (transform.position.x > rightScreenEdge)
        {
            m_rb.velocity = Vector2.zero;
            m_direction = Vector2.left;
        }

        m_rb.AddForce(m_direction * m_moveSpeed, ForceMode2D.Force);
    }

    protected virtual void SpeedControl()
    {
        // Limit speed when needed
        Vector2 flatVel = new Vector2(m_rb.velocity.x, 0);

        if (flatVel.magnitude > m_moveSpeed)
        {
            Vector2 limitedVel = flatVel.normalized * m_moveSpeed;
            m_rb.velocity = new Vector2(limitedVel.x, m_rb.velocity.y);
        }
    }

    protected override void Update()
    {
        base.Update();
        if(m_inPosition)
        {
            Moving();
            SpeedControl();
        }
    }
}

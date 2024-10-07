using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WithinBounds : MovingEnemy
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        m_direction = Vector2.right;
    }

    private void SpeedControl()
    {
        // Limit speed when needed
        Vector2 flatVel = new Vector2(m_rb.velocity.x, 0);

        if (flatVel.magnitude > m_moveSpeed)
        {
            Vector2 limitedVel = flatVel.normalized * m_moveSpeed;
            m_rb.velocity = new Vector2(limitedVel.x, m_rb.velocity.y);
        }
    }

    private void Moving()
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

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (m_inPosition)
        {
            Moving();
            SpeedControl();
        }
    }
}

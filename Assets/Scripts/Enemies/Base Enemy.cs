using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : MonoBehaviour
{
    public Vector2 m_startPositon;
    public BasePickup m_powerUp;

    protected int m_hitPoints;
    protected bool m_inPosition;

    protected virtual void Start()
    {
        m_hitPoints = 3;
        m_inPosition = false;
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            m_hitPoints--;
            if (m_hitPoints <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}

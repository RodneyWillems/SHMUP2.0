using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingEnemy : BaseEnemy
{
    [SerializeField] private protected float m_moveSpeed;
    [SerializeField] private protected Vector2 m_direction;

    private protected Rigidbody2D m_rb;

    private protected virtual void Start()
    {
        m_rb = GetComponent<Rigidbody2D>();
    }
}

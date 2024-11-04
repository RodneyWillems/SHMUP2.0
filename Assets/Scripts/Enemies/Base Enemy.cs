using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : MonoBehaviour
{
    public Vector2 m_startPositon;
    public BasePickup m_powerUp;
    public bool m_inPosition;
    public int m_score;

    protected int m_hitPoints;
    protected PlayerMovement m_player;

    protected virtual void Start()
    {
        m_hitPoints = 3;
        m_score = 50;
        m_inPosition = false;
        m_player = FindObjectOfType<PlayerMovement>();
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
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
        else if (collision.gameObject.CompareTag("Power"))
        {
            Destroy(gameObject);
        }
    }

    protected virtual void OnDestroy()
    {
        if (GameManager.instance.ContainsEnemy(this))
        {
            SpawnPickup();
            GameManager.instance.RemoveEnemy(this);
        }
    }

    protected virtual void SpawnPickup()
    {
        // Spawn pickup
    }

    protected virtual void Update()
    {
        if (!m_inPosition)
        {
            transform.position = Vector3.Lerp(transform.position, m_startPositon, 2 * Time.deltaTime);
            m_inPosition = Vector3.Distance(transform.position, m_startPositon) <= 0.5f;
        }
    }
}

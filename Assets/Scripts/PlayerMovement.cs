using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    #region Variables
    [Header("Ship")]
    [SerializeField] private GameObject[] m_damageParts;
    [SerializeField] private int m_health, m_movementSpeed;

    [Header("Shooting")]
    [SerializeField] private GameObject m_bulletSpawnPoint;
    [SerializeField] private GameObject m_bulletPrefab, m_powerPrefab;
    [SerializeField] private GameObject m_chargeEffect;

    // Player Controls
    private PlayerControls m_controls;
    private Coroutine m_shooting;
    private float m_baseCooldown;
    private bool m_paused, m_charged;
    private Coroutine m_iFrames;

    // Miscellaneous
    private Rigidbody2D m_rb;
    private Animator m_animator;

    #endregion

    #region Controls
    private void Awake()
    {
        // Setting all controls
        m_controls = new PlayerControls();
        m_controls.Default.Pause.started += OnPause;
        m_controls.Default.Shoot.started += StartShoot;
        m_controls.Default.Shoot.canceled += StopShoot;
        m_controls.Default.PowerShoot.started += ChargeShot;
        m_controls.Default.PowerShoot.performed += OnPowerShoot;
        m_controls.Default.PowerShoot.canceled += StopCharging;
    }

    private void OnEnable()
    {
        // Enabling all controls
        m_controls.Default.Shoot.Enable();
        m_controls.Default.Pause.Enable();
        m_controls.Default.PowerShoot.Enable();
        m_controls.Default.Movement.Enable();
    }

    private void OnDisable()
    {
        // Enabling all controls
        m_controls.Default.Shoot.Disable();
        m_controls.Default.Pause.Disable();
        m_controls.Default.PowerShoot.Disable();
        m_controls.Default.Movement.Disable();
    }
    #endregion

    void Start()
    {
        m_rb = GetComponent<Rigidbody2D>();
        m_animator = GetComponent<Animator>();

        m_baseCooldown = 0.3f;

        foreach (GameObject go in m_damageParts)
        {
            go.SetActive(false);
        }

        m_iFrames = null;
    }

    #region Damage
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Enemy"))
        {
            TakeDamage();
        }
    }

    private void TakeDamage()
    {
        if (m_iFrames == null)
        {
            m_health--;
            m_damageParts[m_health].SetActive(true);
            m_iFrames = StartCoroutine(IFrames());
        }
    }

    private IEnumerator IFrames()
    {
        if (m_health <= 0)
        {
            Destroy(gameObject);
        }
        for (int i = 0; i < 15; i++)
        {
            GetComponentInChildren<SpriteRenderer>().enabled = !GetComponentInChildren<SpriteRenderer>().enabled;
            float time = 0;
            yield return new WaitUntil(() =>
            {
                time += Time.deltaTime;
                return time >= 0.1f;
            });
        }
        GetComponentInChildren<SpriteRenderer>().enabled = true;
        m_iFrames = null;
        yield return null;
    }
    #endregion

    #region Inputs
    private void OnPause(InputAction.CallbackContext _context)
    {
        print("Pausing");
        m_paused = !m_paused;
        if (m_paused)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    private void StartShoot(InputAction.CallbackContext _context)
    {
        Debug.Log("Shooting !!!!!");
        m_shooting = StartCoroutine(Shooting());
    }

    private void StopShoot(InputAction.CallbackContext _context)
    {
        Debug.Log("No longer shooting !!!!");
        StopCoroutine(m_shooting);
    }

    private IEnumerator Shooting()
    {
        while (true)
        {
            if (m_iFrames == null)
            {
                GameObject newBullet = Instantiate(m_bulletPrefab, m_bulletSpawnPoint.transform.position, Quaternion.identity);
                newBullet.tag = transform.tag;
                newBullet.layer = gameObject.layer;
                newBullet.GetComponent<Rigidbody2D>().velocity = Vector2.up * 10;
                Destroy(newBullet, 3);
            }
            yield return new WaitForSeconds(m_baseCooldown);
        }
    }

    private void ChargeShot(InputAction.CallbackContext _context)
    {
        m_chargeEffect.SetActive(true);
    }

    private void OnPowerShoot(InputAction.CallbackContext _context)
    {
        print("Powerful shot charged");
        m_charged = true;
        ActualStopCharging();
    }

    private void StopCharging(InputAction.CallbackContext _context)
    {
        if (m_charged)
        {
            for (int i = 0; i < 20; i++)
            {
                GameObject powerShot = Instantiate(m_powerPrefab, new Vector3(-8 + i, -1.85f), Quaternion.identity);
                powerShot.layer = gameObject.layer;
                powerShot.GetComponent<Rigidbody2D>().velocity = Vector2.up * 20;
                Destroy(powerShot, 0.5f);
                m_charged = false;
            }
        }
        else
        {
            ActualStopCharging();
        }
    }

    private void ActualStopCharging()
    {
        print("NO");
        m_chargeEffect.SetActive(false);
    }
    #endregion

    #region Movement
    private void Moving()
    {
        // Movement logic
        Vector2 moveInput = m_controls.Default.Movement.ReadValue<Vector2>();
        m_rb.velocity = (moveInput * m_movementSpeed);

        // Screen wrapper
        float rightScreenEdge = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height)).x + 0.15f;
        float leftScreenEdge = -rightScreenEdge - 0.15f;

        if (transform.position.x < leftScreenEdge)
        {
            transform.position = new Vector2(rightScreenEdge, transform.position.y);
        }
        else if (transform.position.x > rightScreenEdge)
        {
            transform.position = new Vector2(leftScreenEdge, transform.position.y);
        }
    }

    private void SpeedControl()
    {
        // Limit speed when needed
        Vector2 flatVel = new Vector2(m_rb.velocity.x, 0);

        if (flatVel.magnitude > m_movementSpeed)
        {
            Vector2 limitedVel = flatVel.normalized * m_movementSpeed;
            m_rb.velocity = new Vector2(limitedVel.x, m_rb.velocity.y);
        }
    }

    private void FixedUpdate()
    {
        Moving();
        SpeedControl();
    }
    #endregion
}

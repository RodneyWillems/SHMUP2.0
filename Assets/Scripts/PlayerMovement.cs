using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float m_health, m_movementSpeed;
    [SerializeField] private GameObject m_bulletSpawnPoint;
    [SerializeField] private GameObject m_bulletPrefab;
    [SerializeField] private GameObject m_chargeEffect;

    // Player Controls
    private PlayerControls m_controls;
    private Coroutine m_shooting;
    private float m_baseCooldown;
    private bool m_paused, m_charged;

    // Miscellaneous
    private Rigidbody2D m_rb;
    private Animator m_animator;

    private void Awake()
    {
        // Enabling all controls
        m_controls = new PlayerControls();
        m_controls.Default.Pause.started += OnPause;
        m_controls.Default.Pause.Enable();
        m_controls.Default.Shoot.started += StartShoot;
        m_controls.Default.Shoot.canceled += StopShoot;
        m_controls.Default.Shoot.Enable();
        m_controls.Default.PowerShoot.started += ChargeShot;
        m_controls.Default.PowerShoot.performed += OnPowerShoot;
        m_controls.Default.PowerShoot.canceled += StopCharging;
        m_controls.Default.PowerShoot.Enable();
        m_controls.Default.Movement.Enable();
    }

    void Start()
    {
        m_rb = GetComponent<Rigidbody2D>();
        m_animator = GetComponent<Animator>();

        m_baseCooldown = 0.3f;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Enemy"))
        {
            TakeDamage();
        }
    }

    private void TakeDamage()
    {
        m_health--;
        print("Oh no!");
        //m_animator.Play("Hit");
    }

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
            GameObject newBullet = Instantiate(m_bulletPrefab, transform.position + Vector3.up, Quaternion.identity);
            newBullet.tag = transform.tag;
            newBullet.GetComponent<Rigidbody2D>().velocity = Vector2.up * 10;
            Destroy(newBullet, 3);
            yield return new WaitForSeconds(m_baseCooldown);
        }
    }

    private void ChargeShot(InputAction.CallbackContext _context)
    {
        print("Charging my balls");
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
            print("POWER SHOT GO BRRRRR");
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

    void Update()
    {

    }

    private void FixedUpdate()
    {
        Moving();
        SpeedControl();
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    // Enemies
    [SerializeField] private GameObject[] m_shootingEnemies, m_movingEnemies, m_bossPrefabs;
    [SerializeField] private GameObject m_bulletPrefab;

    [SerializeField] private List<BaseEnemy> m_enemies;
    private Coroutine m_waveCoroutine;
    private int m_currentWave, m_currentGroup;

    // Score
    [SerializeField] private TextMeshProUGUI m_scoreText;
    private int m_score;

    void Start()
    {
        instance = this;
        m_currentWave = 0;
        m_currentGroup = 0;
        NextWave();
    }

    public bool ContainsEnemy(BaseEnemy enemy)
    {
        if (m_enemies.Contains(enemy)) return true;
        else return false;
    }

    public void RemoveEnemy(BaseEnemy enemy)
    {
        m_enemies.Remove(enemy);
        if (m_enemies.Count <= 0)
        {
            m_currentGroup++;
            if (m_currentGroup >= 3)
            {
                SpawnBoss();
                m_currentGroup = 0;
            }
            else
            {
                NextWave();
            }
        }
        m_score += enemy.m_score;
        UpdateScore();
    }

    private void NextWave()
    {
        m_enemies = new List<BaseEnemy>();
        m_currentWave++;
        m_waveCoroutine = StartCoroutine(SpawnWave());
    }

    private IEnumerator SpawnWave()
    {
        while (m_enemies.Count < 3)
        {
            // Randomly deciding which type of enemy to spawn until there's 7 in total
            int randomEnemySprite = Random.Range(0, 5);
            int randomEnemyType = Random.Range(0, 3);
            if (randomEnemyType == 0)
            {
                // Spawning a moving enemy and randomly deciding which type of moving enemy it is
                GameObject newEnemy = Instantiate(m_movingEnemies[randomEnemySprite]);
                if (Random.Range(0, 2) == 0)
                {
                    m_enemies.Add(newEnemy.AddComponent<WithinBounds>());
                }
                else
                {
                    m_enemies.Add(newEnemy.AddComponent<TowardsPlayer>());
                }
                // Adjusting the script
                MovingEnemy newEnemyScript = newEnemy.GetComponent<MovingEnemy>();
                newEnemyScript.m_startPositon = new Vector2(Random.Range(-5, 5), Random.Range(-1, 5));
                newEnemyScript.m_moveSpeed = 1 + (m_currentWave / 4);
            }
            else if (randomEnemyType == 1) 
            {
                // Spawning a shooting enemy and randomly deciding which type of shooting enemy it is
                GameObject newEnemy = Instantiate(m_shootingEnemies[randomEnemySprite]);
                if (Random.Range(0, 2) == 0)
                {
                    m_enemies.Add(newEnemy.AddComponent<TowardsDirection>());
                }
                else
                {
                    m_enemies.Add(newEnemy.AddComponent<ShootPlayer>());
                }
                // Adjusting the script
                ShootingEnemy newEnemyScript = newEnemy.GetComponent<ShootingEnemy>();
                newEnemyScript.m_bulletPrefab = m_bulletPrefab;
                newEnemyScript.m_startPositon = new Vector2(Random.Range(-5, 5), Random.Range(-1, 5));
                newEnemyScript.m_cooldown = 1 + (m_currentWave / 2);
            }
            else
            {
                // Spawning enemy and giving the scripts
                GameObject newEnemy = Instantiate(m_shootingEnemies[randomEnemySprite]);
                m_enemies.Add(newEnemy.AddComponent<ShootPlayer>());
                newEnemy.AddComponent<WithinBounds>();

                // Adjusting the shooting script
                ShootingEnemy newEnemyShootScript = newEnemy.GetComponent<ShootingEnemy>();
                newEnemyShootScript.m_bulletPrefab = m_bulletPrefab;
                newEnemyShootScript.m_startPositon = new Vector2(Random.Range(-5, 5), Random.Range(-1, 5));
                newEnemyShootScript.m_cooldown = 1 + (m_currentWave / 2);

                // Adjusting the moving script
                MovingEnemy newEnemyMovingScript = newEnemy.GetComponent<MovingEnemy>();
                newEnemyMovingScript.m_startPositon = newEnemyShootScript.m_startPositon;
                newEnemyMovingScript.m_moveSpeed = 1 + (m_currentWave / 4);
            }
            float time = 0;
            yield return new WaitUntil(() =>
            {
                time += Time.deltaTime;
                return time >= 0.15f;
            });
        }
        m_waveCoroutine = null;
    }

    private void SpawnBoss()
    {
        BossDeath();
    }

    public void BossDeath()
    {
        NextWave();
    }

    private void UpdateScore()
    {
        m_scoreText.text = "Score: " + m_score.ToString();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public delegate void LoseALifeGM(int lives);
    public event LoseALifeGM OnLifeLostGM;
    public delegate void AllEnemiesKilled(float waveDelay);
    public event AllEnemiesKilled AllEnemiesKilledInWave;

    #region Variables
    [Header("Enemy Spawner Variables")]
    public Enemy enemyPrefab;
    public List<Enemy> enemyList;
    private Scaling scaling;
    private Vector3 enemySpawnpoint;
    private Coroutine spawnEnemyCoroutine;
    private int enemiesSpawned;
    private int maxEnemies;
    private float spawnDelay;
    private float waveDelay;
    private GameManager.Difficulty difficulty;
    private int[] firstSpawns = { 5, 7, 10, 15 };
    private int[] firstSpawnDelay = { 4, 3, 2, 1 };
    private int[] firstWaveDelay = { 15, 13, 10, 6 };
    #endregion

    #region Monobehaviour
    private void Awake()
    {
        enemySpawnpoint = transform.position;
        scaling = FindObjectOfType<Scaling>();
    }

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.OnGameEnded += GameManager_Instance_OnGameEnded;
        GameManager.Instance.SendWaveNum += GameManger_Instance_SendWaveNum;
        GameManager.Instance.OnDifficultySent += GameManager_Instance_OnDifficultySent;
    }

    
    #endregion

    #region Coroutines
    IEnumerator SpawnEnemy()
    {
        if (enemiesSpawned < maxEnemies)
        {
            Enemy enemy = Instantiate(enemyPrefab, enemySpawnpoint, Quaternion.identity);
            enemy.OnLifeLost += Enemy_OnLifeLost;
            enemy.JustDied += Enemy_JustDied;
            enemyList.Add(enemy);
            enemiesSpawned++;
            yield return new WaitForSeconds(spawnDelay);
            spawnEnemyCoroutine = StartCoroutine(SpawnEnemy());
        }
        else
        {
            if (AllEnemiesKilledInWave != null)
            {
                AllEnemiesKilledInWave(waveDelay);
                
            }
        }
    }

    #endregion

    #region Functions
    private void GameManager_Instance_OnDifficultySent(GameManager.Difficulty difficulty)
    {
        this.difficulty = difficulty;
    }
    private void GameManger_Instance_SendWaveNum(int num)
    {
        if (num == 1)
        {
            maxEnemies = firstSpawns[(int)difficulty];
            spawnDelay = firstSpawnDelay[(int)difficulty];
            waveDelay = firstWaveDelay[(int)waveDelay];
        }
        else
        {
            maxEnemies = Mathf.RoundToInt(firstSpawns[(int)difficulty] * Mathf.Pow(scaling.GetScaling(), num));
            spawnDelay = firstSpawnDelay[(int)difficulty] / Mathf.Pow(scaling.GetScaling(), num);
            waveDelay = firstWaveDelay[(int)difficulty] / Mathf.Pow(scaling.GetScaling(), num);
        }

        enemiesSpawned = 0;
        spawnEnemyCoroutine = StartCoroutine(SpawnEnemy());
    }
    

    private void Enemy_JustDied(Enemy enemy)
    {
        enemyList.Remove(enemy);
        enemy.OnLifeLost -= Enemy_OnLifeLost;
        enemy.JustDied -= Enemy_JustDied;

        if (enemyList.Capacity == 0)
        {
            if (AllEnemiesKilledInWave != null)
            {
                AllEnemiesKilledInWave(waveDelay);
            }
        }
    }

    private void Enemy_OnLifeLost(int lives, Enemy enemy)
    {
        enemy.OnLifeLost -= Enemy_OnLifeLost;
        enemy.JustDied -= Enemy_JustDied;
        enemyList.Remove(enemy);
        if (OnLifeLostGM != null)
        {
            OnLifeLostGM(lives);
        }
    }

    private void GameManager_Instance_OnGameEnded()
    {
        StopCoroutine(spawnEnemyCoroutine);
    }

    #endregion
}

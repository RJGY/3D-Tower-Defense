using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public delegate void LoseALifeGM(int lives);
    public event LoseALifeGM OnLifeLostGM;

    #region Variables
    [Header("Enemy Spawner Variables")]
    public Enemy enemyPrefab;
    private Vector3 enemySpawnpoint;
    private Coroutine spawnEnemyCoroutine;

    #endregion

    #region Monobehaviour
    private void Awake()
    {
        enemySpawnpoint = transform.position;
    }

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.OnGameEnded += GameManager_Instance_OnGameEnded;
        spawnEnemyCoroutine = StartCoroutine(SpawnEnemy());
    }

    #endregion

    #region Coroutines
    IEnumerator SpawnEnemy()
    {
        Enemy enemy = Instantiate(enemyPrefab, enemySpawnpoint, Quaternion.identity);
        enemy.OnLifeLost += Enemy_OnLifeLost;
        yield return new WaitForSeconds(2);
        spawnEnemyCoroutine = StartCoroutine(SpawnEnemy());
    }

    #endregion

    #region Functions

    private void Enemy_OnLifeLost(int lives, Enemy enemy)
    {
        enemy.OnLifeLost -= Enemy_OnLifeLost;
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

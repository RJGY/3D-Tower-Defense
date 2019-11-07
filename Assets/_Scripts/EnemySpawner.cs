using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    
    #region Variables
    [Header("Enemy Spawner Variables")]
    public Enemy enemyPrefab;
    private Vector3 enemySpawnpoint;
    private Coroutine spawnEnemyCoroutine;

    #endregion

    #region Monobehaviour
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.OnGameEnded += GameManager_Instance_OnGameEnded;
        enemySpawnpoint = transform.position;
        spawnEnemyCoroutine = StartCoroutine(SpawnEnemy());
    }

    

    // Update is called once per frame
    void Update()
    {
        
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
        GameManager.Instance.lives -= lives;
        GameManager.Instance.UpdateLives();
        if (GameManager.Instance.lives <= 0)
        {
            GameManager.Instance.GameOver();
        }
    }

    private void GameManager_Instance_OnGameEnded()
    {
        StopCoroutine(spawnEnemyCoroutine);
    }

    #endregion
}

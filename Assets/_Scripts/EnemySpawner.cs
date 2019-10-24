using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // Start is called before the first frame update
    #region Variables
    public Enemy enemyPrefab;
    public Transform enemySpawnpoint;
    public Coroutine spawnEnemyCoroutine;
    #endregion
    void Start()
    {
        enemySpawnpoint = this.transform;
        spawnEnemyCoroutine = StartCoroutine(SpawnEnemy());
    }

    // Update is called once per frame
    void Update()
    {
    
        
    }

    IEnumerator SpawnEnemy()
    {
        Enemy enemy = Instantiate(enemyPrefab, enemySpawnpoint.position, Quaternion.identity);
        enemy.OnLifeLost += Enemy_OnLifeLost;
        yield return new WaitForSeconds(2);
        spawnEnemyCoroutine = StartCoroutine(SpawnEnemy());
    }

    private void Enemy_OnLifeLost(int lives)
    {
        GameManager.Instance.lives -= lives;
    }
}

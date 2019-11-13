﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public delegate void LoseALifeGM(int lives);
    public event LoseALifeGM OnLifeLostGM;

    #region Variables
    [Header("Enemy Spawner Variables")]
    public Enemy enemyPrefab;
    public List<Enemy> enemyList;
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
        enemy.JustDied += Enemy_JustDied;
        enemyList.Add(enemy);
        yield return new WaitForSeconds(0.66f);
        spawnEnemyCoroutine = StartCoroutine(SpawnEnemy());
    }

    private void Enemy_JustDied(Enemy enemy)
    {
        enemyList.Remove(enemy);
        enemy.OnLifeLost -= Enemy_OnLifeLost;
        enemy.JustDied -= Enemy_JustDied;
    }

    #endregion

    #region Functions

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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EnemySpawner : MonoBehaviour
{

    #region Events
    public delegate void SpawnEnemy(Transform endPathTransform);
    public event SpawnEnemy EnemySpawned;
    #endregion

    #region Variables
    [Header("Enemy Spawner Variables")]
    private Transform endPath;
    public Enemy enemyPrefab;
    private Vector3 enemySpawnpoint;
    private Coroutine spawnEnemyCoroutine;
    private int enemiesSpawned = 0;
    private int maxEnemies;
    private float spawnDelay;
    private float waveDelay;
    #endregion

    #region Monobehaviour
    #region Singleton
    public static EnemySpawner instance = null;
    private void Awake()
    {
        if (instance != null)
            Debug.LogError("More than one EndPath");
        else
            instance = this;

        endPath = FindObjectOfType<EndPath>().GetComponent<Transform>();
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Update()
    {
        // Temp test
        var keyboard = Keyboard.current;
        if (keyboard == null)
            return; // No keyboard connected.

        if (keyboard.enterKey.wasPressedThisFrame)
        {
            SpawnWave();
        }
    }

    #endregion

    #region Coroutines
    // Test SpawnWave Coroutine.
    void SpawnWave()
    {
        enemiesSpawned++;
        Debug.Log(name + ": Testing. Enemies Spawned = " + enemiesSpawned, this);
        Enemy enemy = Instantiate(enemyPrefab, transform);
        EnemySpawned?.Invoke(endPath);
    }

    #endregion

    #region Functions

    #endregion
}

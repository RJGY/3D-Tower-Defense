using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    #region Events

    #endregion

    #region Variables

    [Header("Game Variables")]
    private int lives;
    private float money;
    private int currentWave;
    private int maxWaves;
    private Coroutine waveCounter;
    private EnemySpawner enemySpawner;
    private Difficulty difficulty;

    // [Header("UI Variables")]

    public enum Difficulty
    {
        Easy,
        Medium,
        Hard,
        Impossible,
        Undefined
    }

    #endregion

    #region Singleton
    public static GameManager Instance = null;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("Multiple GameManagers in scene.");
        }
        
    }
    #endregion

    #region Monobehaviour
    // Start is called before the first frame update
    void Start()
    {
        

        
    }

    #endregion

    #region Functions
    public void StartGame()
    {

    }
    #endregion

    #region Coroutines
    
    #endregion
}

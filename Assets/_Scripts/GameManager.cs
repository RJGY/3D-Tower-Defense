using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public delegate void GameEnded();
    public event GameEnded OnGameEnded;

    public delegate void SendDifficulty(Difficulty difficulty);
    public event SendDifficulty OnDifficultySent;

    public delegate void OnWave(int num);
    public event OnWave SendWaveNum;

    #region Variables
    [Header("Game Variables")]
    private int lives;
    private float money;
    private int currentWave;
    private Coroutine waveCounter;
    private EnemySpawner enemySpawner;
    private Difficulty difficulty;

    [Header("UI Variables")]

    private Text livesText;
    private Text moneyText;

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

        livesText = FindObjectOfType<LivesText>().GetComponent<Text>();
        moneyText = FindObjectOfType<MoneyText>().GetComponent<Text>();
        enemySpawner = FindObjectOfType<EnemySpawner>();
    }
    #endregion

    #region Monobehaviour
    // Start is called before the first frame update
    void Start()
    {
        enemySpawner.OnLifeLostGM += EnemySpawner_OnLifeLostGM;
        enemySpawner.AllEnemiesKilledInWave += EnemySpawner_OnEnemiesKilled;

        lives = 20; // TEMP
        money = 100; // TEMP
        difficulty = Difficulty.Medium; // TEMP
        currentWave = 0;

        UpdateLives();
        UpdateMoney();

        StartCoroutine(SendDifficultyCoroutine());
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            EnemySpawner_OnEnemiesKilled(2f);
            Debug.Log("HowManyTimes");
        }
    }
    private void EnemySpawner_OnEnemiesKilled(float waveDelay)
    {
        waveCounter = StartCoroutine(WaveCounter(waveDelay));
    }

    #endregion

    #region Functions
    private void EnemySpawner_OnLifeLostGM(int lives)
    {
        this.lives -= lives;
        UpdateLives();

        if (this.lives <= 0)
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        if (OnGameEnded != null)
        {
            OnGameEnded();
        }
    }

    private void AddGold(float gold)
    {
        money += gold;
        UpdateMoney();
    }

    private void UpdateLives()
    {
        livesText.text = "Lives: " + lives.ToString();
    }

    private void UpdateMoney()
    {
        moneyText.text = "Money: " + money.ToString();
    }
    #endregion

    #region Coroutines
    IEnumerator SendDifficultyCoroutine()
    {
        yield return new WaitForEndOfFrame();
        if (OnDifficultySent != null && difficulty != Difficulty.Undefined)
        {
            OnDifficultySent(difficulty);
        }
        else
        {
            Debug.Log("Noone is taking the difficulty or difficulty is Undefined.");
        }
    }
    IEnumerator WaveCounter(float waveDelay)
    {
        yield return new WaitForSeconds(waveDelay);
        currentWave++;
        if (SendWaveNum != null)
        {
            SendWaveNum(currentWave);
        }
        else
        {
            Debug.Log("Noone is taking the wave number");
        }
    }
    #endregion
}

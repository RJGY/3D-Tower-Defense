using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    #region Events
    public delegate void GameEnded();
    public event GameEnded OnGameEnded;

    public delegate void SendDifficulty(Difficulty difficulty);
    public event SendDifficulty OnDifficultySent;

    public delegate void OnWave(int num);
    public event OnWave SendWaveNum;
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
    private Shop shop;
    private UpgradeManager upgrade;

    [Header("UI Variables")]

    private Text livesText;
    private Text moneyText;
    private Text waveText;

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

        shop = FindObjectOfType<Shop>();
        livesText = FindObjectOfType<LivesText>().GetComponent<Text>();
        moneyText = FindObjectOfType<MoneyText>().GetComponent<Text>();
        waveText = FindObjectOfType<WaveText>().GetComponent<Text>();
        enemySpawner = FindObjectOfType<EnemySpawner>();
        upgrade = FindObjectOfType<UpgradeManager>();
    }
    #endregion

    #region Monobehaviour
    // Start is called before the first frame update
    void Start()
    {
        enemySpawner.OnLifeLostGM += EnemySpawner_OnLifeLostGM;
        enemySpawner.AllEnemiesKilledInWave += EnemySpawner_OnEnemiesKilled;
        shop.OnPay += Shop_OnPurchase;
        upgrade.OnPay += Upgrade_OnPay;

        lives = 20; // TEMP
        money = 120; // TEMP
        difficulty = Difficulty.Medium; // TEMP
        currentWave = 0;
        maxWaves = 50;

        UpdateLives();
        UpdateMoney();
        UpdateWave();

        
    }

    #endregion

    #region Functions
    public void StartGame()
    {
        EnemySpawner_OnEnemiesKilled(2f);
        difficulty = (Difficulty)FindObjectOfType<DifficultyDropdown>().GetComponent<Dropdown>().value;
        StartCoroutine(SendDifficultyCoroutine());
    }

    private void EnemySpawner_OnEnemiesKilled(float waveDelay)
    {
        waveCounter = StartCoroutine(WaveCounter(waveDelay));
    }

    private void Shop_OnPurchase(float cost)
    {
        money -= cost;
        UpdateMoney();
    }

    private void Upgrade_OnPay(float cost)
    {
        money -= cost;
        UpdateMoney();
    }

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

    public void AddGold(float gold)
    {
        money += gold;
        UpdateMoney();
    }

    public float GetGold()
    {
        return money;
    }

    private void UpdateLives()
    {
        livesText.text = "Lives: " + lives.ToString();
    }

    private void UpdateMoney()
    {
        moneyText.text = "Money: " + money.ToString();
    }

    private void UpdateWave()
    {
        waveText.text = "Wave: " + currentWave.ToString() + " / " + maxWaves;
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
        currentWave++;
        UpdateWave();
        yield return new WaitForSeconds(waveDelay);
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

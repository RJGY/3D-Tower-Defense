using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public delegate void GameEnded();
    public event GameEnded OnGameEnded;

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

    #region Variables
    [Header("Game Variables")]
    public int lives;
    public int money;
    public int currentWave;
    private EnemySpawner enemySpawner;

    [Header("UI Variables")]
    
    private Text livesText;
    private Text moneyText;

    #endregion

    #region Monobehaviour
    // Start is called before the first frame update
    void Start()
    {
        enemySpawner.OnGameEnded += EnemySpawner_OnGameEnded;

        lives = 20;
        money = 100;
        livesText.text = lives.ToString();
        moneyText.text = money.ToString();
    }

    #endregion

    #region Functions
    private void EnemySpawner_OnGameEnded()
    {
        if (OnGameEnded != null)
        {
            OnGameEnded();
        }
    }
    public void UpdateLives()
    {
        livesText.text = lives.ToString();
    }

    public void UpdateMoney()
    {
        moneyText.text = money.ToString();
    }
    #endregion

    #region Coroutines
    IEnumerator WaveCounter()
    {
        yield return new WaitForSeconds(0);
    }
    #endregion
}

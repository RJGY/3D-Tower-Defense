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
    private int lives;
    private int money;
    private int currentWave;
    private EnemySpawner enemySpawner;

    [Header("UI Variables")]
    
    private Text livesText;
    private Text moneyText;

    #endregion

    #region Monobehaviour
    // Start is called before the first frame update
    void Start()
    {
        enemySpawner.OnLifeLostGM += EnemySpawner_OnLifeLostGM;
        lives = 20;
        money = 100;
        livesText.text = lives.ToString();
        moneyText.text = money.ToString();
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
    private void UpdateLives()
    {
        livesText.text = lives.ToString();
    }

    private void UpdateMoney()
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

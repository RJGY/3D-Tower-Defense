using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    
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

    #region Variables
    [Header("UI Attributes")]
    public int lives;
    public int money;
    public Text livesText;
    public Text moneyText;

    #endregion
    // Start is called before the first frame update
    void Start()
    {
        lives = 20;
        money = 100;
    }

    // Update is called once per frame
    void Update()
    {

    }
}

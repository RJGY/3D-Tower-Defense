using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scaling : MonoBehaviour
{
    // Start is called before the first frame update
    private GameManager.Difficulty difficulty;
    private float[] scaling = { 1.05f, 1.07f, 1.1f, 1.15f };
    void Start()
    {
        GameManager.Instance.OnDifficultySent += GameManager_Instance_OnDifficultySent;
    }

    void GameManager_Instance_OnDifficultySent(GameManager.Difficulty difficulty)
    {
        this.difficulty = difficulty;
    }

    public float GetScaling()
    {
        return scaling[(int)difficulty];
    }
}

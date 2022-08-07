using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreScript : MonoBehaviour
{
    public TMP_Text scoreText;
    public int score = 0;

    private bool canSpawnTier2 = true;
    private bool canSpawnAir = true;
    private bool canSpawnMid = true;

    //Singleton
    public static ScoreScript Instance { get; set; }

    private void Awake()
    {
        //Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void LateUpdate()
    {
        scoreText.text = score.ToString();

        if (score == 4)
        {
            if (canSpawnTier2)
            {
                GameStateScript.Instance.canSpawnTier2_1 = true;
                GameStateScript.Instance.canSpawnTier2_2 = true;
                canSpawnTier2 = false;
            }
        }
        else if (score == 8)
        {
            if (canSpawnAir)
            {
                GameStateScript.Instance.canSpawnAir = true;
                canSpawnAir = false;
            }
        }
        else if (score == 12)
        {
            if (canSpawnMid)
            {
                GameStateScript.Instance.canSpawnMid = true;
                canSpawnMid = false;
            }
        }
    }

    public void ResetScore()
    {
        score = 0;
        canSpawnTier2 = true;
        canSpawnAir = true;
        canSpawnMid = true;
    }
}

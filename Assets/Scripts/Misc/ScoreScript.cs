using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreScript : MonoBehaviour
{
    public TMP_Text scoreText;
    public int score = 0;

    private bool runOnce = true;

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

        if (score == 5)
        {
            if (runOnce)
            {
                GameStateScript.Instance.canSpawnTier2_1 = true;
                GameStateScript.Instance.canSpawnTier2_2 = true;
                runOnce = false;
            }
        }
        else if (score == 7)
        {
            if (runOnce)
            {
                GameStateScript.Instance.canSpawnAir = true;
                runOnce = false;
            }
        }
        else if (score == 10)
        {
            if (runOnce)
            {
                GameStateScript.Instance.canSpawnMid = true;
                runOnce = false;
            }
        }
        else
        {
            runOnce = true;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartScript : MonoBehaviour
{
    public Transform defaultSpawn;

    public static RestartScript Instance { get; set; }

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

    public void RestartScore()
    {
        ScoreScript.Instance.score = 0;
    }

    public void RestartPlayer()
    {
        ControlsManager.Instance.currentCharacter.SetActive(true);
        ControlsManager.Instance.currentCharacter.transform.position = defaultSpawn.position;
        CameraScript.Instance.target = ControlsManager.Instance.currentCharacter.transform;
    }

    public void RestartEnemies()
    {
        //In the enemy spawn script
    }

    public void RestartFridge()
    {
        GameObject.FindGameObjectWithTag("VegFridge").GetComponent<FridgeScript>().RestartFridge();
    }
}

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
        ScoreScript.Instance.ResetScore();
    }

    public void RestartPlayer()
    {
        ControlsManager.Instance.EndAttack();
        ControlsManager.Instance.currentCharacter.SetActive(true);
        ControlsManager.Instance.currentCharacter.transform.position = defaultSpawn.position;
        CameraScript.Instance.ChangeTarget();

        GameObject[] riceBalls = GameObject.FindGameObjectsWithTag("RiceBall");
        for (int i = 0; i < riceBalls.Length; i++)
        {
            Destroy(riceBalls[i]);
        }
    }

    public void RestartEnemies()
    {
        EnemySpawn.Instance.StopSpawning();

        GameObject[] vegies = GameObject.FindGameObjectsWithTag("Vegetable");
        for (int i = 0; i < vegies.Length; i++)
        {
            Destroy(vegies[i]);
        }

        GameObject[] meats = GameObject.FindGameObjectsWithTag("Meat");
        for (int i = 0; i < meats.Length; i++)
        {
            Destroy(meats[i]);
        }
    }

    public void RestartFridge()
    {
        GameObject.FindGameObjectWithTag("VegFridge").GetComponent<FridgeScript>().RestartFridge();
        GameObject.FindGameObjectWithTag("MeatFridge").GetComponent<FridgeScript>().RestartFridge();
    }
}

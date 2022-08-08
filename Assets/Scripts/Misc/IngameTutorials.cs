using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngameTutorials : MonoBehaviour
{
    public bool enemyTutorial = true;
    public GameObject enemyText;
    public Sprite vegAllyText;
    public Sprite vegEnemyText;
    public Sprite meatAlleyText;
    public Sprite meatEnemyText;

    public GameObject vegFridgeTutorial;
    public GameObject meatFridgeTutorial;

    public GameObject moveTutorial;
    public GameObject attackTutorial;
    public GameObject jumpTutorial;
    public GameObject joystick;

    public bool firstTouch = true; //For the joystick only

    //Singleton
    public static IngameTutorials Instance { get; set; }

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

    public void ShowEnemyText(GameObject enemy)
    {
        GameObject newText = Instantiate(enemyText, enemy.transform);
    }

    public void TurnOffText(GameObject textObject)
    {
        textObject.SetActive(false);
    }

    public void TurnOnAllText()
    {
        vegFridgeTutorial.SetActive(true);
        meatFridgeTutorial.SetActive(true);

        moveTutorial.SetActive(true);
        attackTutorial.SetActive(true);
        jumpTutorial.SetActive(true);

        firstTouch = true;
        joystick.SetActive(true);
    }
}

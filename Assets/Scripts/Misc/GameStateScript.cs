using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStateScript : MonoBehaviour
{
    //Singleton
    public static GameStateScript Instance { get; set; }

    //Menu canvas
    public GameObject menuCanvas;
    public GameObject menuButtons;

    //Tutorial canvas
    public GameObject tutorialCanvas;
    public Animator tutorialAnim;

    //Gameplay canvas
    public GameObject gameplayCanvas;
    public GameObject controlCanvas;

    //Loading canvas
    public Animator blackOverlayAnim;
    public GameObject loadingToaster;

    //End screen canvas
    public Animator endAnim;
    public GameObject endCanvas;

    //Pause screen canvas
    public GameObject pauseCanvas;

    //Settings screen canvas
    public GameObject settingsCanvas;

    //Tutorial toggle
    public GameObject tutorialToggle;
    public Sprite emptyBox;
    public Sprite tickBox;

    //Number to keep track of what screen to show next
    //0 = Menu, 1 = Tutorial, 2 = Gameplay, 3 = End Screen, 4 = Pause screen
    private int nextScreen = 0;
    public bool startGame = false;
    private bool firstTime = true;
    private bool showTutorial = true;

    public bool canSpawnTier1_1 = true;
    public bool canSpawnTier1_2 = true;
    public bool canSpawnTier2_1 = false;
    public bool canSpawnTier2_2 = false;
    public bool canSpawnMid = false;
    public bool canSpawnAir = false;

    private const float NORMAL_MIN_TIME = 8;
    private const float NORMAL_MAX_TIME = 12;
    private const float MID_MIN_TIME = 10;
    private const float MID_MAX_TIME = 17;
    private const float AIR_MIN_TIME = 11;
    private const float AIR_MAX_TIME = 18;

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

    private void Update()
    {
        if (startGame)
        {
            if (canSpawnTier1_1 && canSpawnTier1_2)
            {
                if (firstTime) //First spawn is 5 seconds sooner than subsequent spawns
                {
                    float rnd1 = Random.Range(NORMAL_MIN_TIME - 5, NORMAL_MAX_TIME - 5);
                    EnemySpawn.Instance.Tier1(0, rnd1);

                    float rnd2 = Random.Range(NORMAL_MIN_TIME - 5, NORMAL_MAX_TIME - 5);
                    EnemySpawn.Instance.Tier1(1, rnd2);

                    firstTime = false;
                }
                else
                {
                    float rnd1 = Random.Range(NORMAL_MIN_TIME, NORMAL_MAX_TIME);
                    EnemySpawn.Instance.Tier1(0, rnd1);

                    float rnd2 = Random.Range(NORMAL_MIN_TIME, NORMAL_MAX_TIME);
                    EnemySpawn.Instance.Tier1(1, rnd2);
                }

                canSpawnTier1_1 = false;
                canSpawnTier1_2 = false;
            }
            else if (canSpawnTier2_1 && canSpawnTier2_2)
            {
                float rnd1 = Random.Range(NORMAL_MIN_TIME, NORMAL_MAX_TIME);
                EnemySpawn.Instance.BothTier(0, rnd1);

                float rnd2 = Random.Range(NORMAL_MIN_TIME, NORMAL_MAX_TIME);
                EnemySpawn.Instance.BothTier(1, rnd2);

                canSpawnTier2_1 = false;
                canSpawnTier2_2 = false;
            }

            if (canSpawnMid)
            {
                float rnd = Random.Range(MID_MIN_TIME, MID_MAX_TIME);
                EnemySpawn.Instance.Mid(rnd);

                canSpawnMid = false;
            }

            if (canSpawnAir)
            {
                float rnd = Random.Range(AIR_MIN_TIME, AIR_MAX_TIME);
                EnemySpawn.Instance.Mid(rnd);

                canSpawnAir = false;
            }
        }
    }

    public IEnumerator Loading()
    {
        //Fade into loading toaster
        blackOverlayAnim.SetBool("isFadeOut", true);
        yield return new WaitForSeconds(1.0f);
        
        loadingToaster.SetActive(true);
        
        blackOverlayAnim.SetBool("isFadeIn", true);
        blackOverlayAnim.SetBool("isFadeOut", false);
        yield return new WaitForSeconds(1.0f);
        blackOverlayAnim.SetBool("isFadeIn", false);

        if (nextScreen == 0)
        {
            yield return HomeMenu();
        }
        else if (nextScreen == 1)
        {
            yield return Tutorial();
        }
        else if (nextScreen == 2)
        {
            yield return Gameplay();
        }
    }

    public IEnumerator HomeMenu()
    {
        //Fade into the home menu
        blackOverlayAnim.SetBool("isFadeOut", true);
        yield return new WaitForSeconds(1.0f);

        blackOverlayAnim.SetBool("isFadeIn", true);
        blackOverlayAnim.SetBool("isFadeOut", false);

        menuCanvas.SetActive(true);

        loadingToaster.SetActive(false);
        gameplayCanvas.SetActive(false);
        controlCanvas.SetActive(false);
        endCanvas.SetActive(false);
        pauseCanvas.SetActive(false);

        yield return new WaitForSeconds(1.0f);
        blackOverlayAnim.SetBool("isFadeIn", false);

        ResetGame();

        yield return null;
    }

    public IEnumerator Tutorial()
    {
        //Fade into tutorial
        blackOverlayAnim.SetBool("isFadeOut", true);
        yield return new WaitForSeconds(1.0f);

        blackOverlayAnim.SetBool("isFadeIn", true);
        blackOverlayAnim.SetBool("isFadeOut", false);

        gameplayCanvas.SetActive(true);
        tutorialCanvas.SetActive(true);

        loadingToaster.SetActive(false);
        menuCanvas.SetActive(false);

        yield return new WaitForSeconds(1.0f);
        blackOverlayAnim.SetBool("isFadeIn", false);

        yield return null;
    }

    public IEnumerator Gameplay()
    {
        //Fade into gameplay
        blackOverlayAnim.SetBool("isFadeOut", true);
        yield return new WaitForSeconds(1.0f);

        blackOverlayAnim.SetBool("isFadeIn", true);
        blackOverlayAnim.SetBool("isFadeOut", false);

        gameplayCanvas.SetActive(true);
        controlCanvas.SetActive(true);

        tutorialCanvas.SetActive(false);
        loadingToaster.SetActive(false);
        menuCanvas.SetActive(false);

        yield return new WaitForSeconds(1.0f);
        blackOverlayAnim.SetBool("isFadeIn", false);

        ControlsManager.Instance.BasicControl();
        startGame = true;
        firstTime = true;

        yield return null;
    }

    public IEnumerator GameOver()
    {
        yield return new WaitForSeconds(1.0f);

        controlCanvas.SetActive(false);
        endCanvas.SetActive(true);

        EnemySpawn.Instance.StopSpawning();

        yield return null;
    }

    //When the play button is pressed
    public void PlayButton()
    {
        if (showTutorial)
        {
            nextScreen = 1;
        }
        else
        {
            nextScreen = 2;
        }
        StartCoroutine(Loading());
    }

    //When the player closes the tutorial to start the game
    public void CloseTutorial()
    {
        nextScreen = 2;
        startGame = true;
        tutorialAnim.SetBool("isClose", true);
    }

    //When the player dies
    public void EndScreen()
    {
        nextScreen = 3;
        startGame = false;
        StartCoroutine(GameOver());
    }

    //When the player pauses the game
    public void PauseButton()
    {
        nextScreen = 4;
        startGame = false;
        controlCanvas.SetActive(false);
        pauseCanvas.SetActive(true);
        Time.timeScale = 0;
    }

    //When the player resumes the game
    public void ResumeGame()
    {
        nextScreen = 2;
        startGame = true;
        controlCanvas.SetActive(true);
        pauseCanvas.SetActive(false);
        Time.timeScale = 1;
    }

    //When the player presses the Home button
    public void HomeButton()
    {
        Time.timeScale = 1;
        nextScreen = 0;
        startGame = false;
        StartCoroutine(Loading());
    }

    //When the player presses the Replay button
    public void Replay()
    {
        ResetGame();

        controlCanvas.SetActive(true);
        ControlsManager.Instance.BasicControl();
        startGame = true;
        firstTime = true;
    }

    private void ResetGame()
    {
        RestartScript.Instance.RestartPlayer();
        RestartScript.Instance.RestartEnemies();
        RestartScript.Instance.RestartScore();
        RestartScript.Instance.RestartFridge();

        endCanvas.SetActive(false);

        canSpawnTier1_1 = true;
        canSpawnTier1_2 = true;
        canSpawnTier2_1 = false;
        canSpawnTier2_2 = false;
        canSpawnMid = false;
        canSpawnAir = false;
}

    //When the player presses the Settings button
    public void SettingsButton()
    {
        settingsCanvas.SetActive(true);

        if (nextScreen == 0)
        {
            menuButtons.SetActive(false);
        }
    }

    //When the player closes the Settings menu
    public void CloseSettings()
    {
        settingsCanvas.SetActive(false);

        if (nextScreen == 0)
        {
            menuButtons.SetActive(true);
        }
    }

    //When the player clicks on the toggle to show/hide the tutorial
    public void TutorialToggle()
    {
        showTutorial = !showTutorial;

        if (showTutorial)
        {
            tutorialToggle.GetComponent<Image>().sprite = tickBox;
        }
        else
        {
            tutorialToggle.GetComponent<Image>().sprite = emptyBox;
        }
    }

    //Quit the game
    public void QuitGame()
    {
        Application.Quit();
    }
}

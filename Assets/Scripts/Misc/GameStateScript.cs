using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateScript : MonoBehaviour
{
    //Singleton
    public static GameStateScript Instance { get; set; }

    //Menu canvas
    public GameObject menuCanvas;

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

    //Number to keep track of what screen to show next
    //0 = Menu, 1 = Tutorial, 2 = Gameplay, 3 = End Screen, 4 = Pause screen
    private int nextScreen = 0;

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
        if (nextScreen == 2)
        {
            if (canSpawnTier1_1 && canSpawnTier1_2)
            {
                float rnd1 = Random.Range(NORMAL_MIN_TIME, NORMAL_MAX_TIME);
                EnemySpawn.Instance.Tier1(0, rnd1);

                float rnd2 = Random.Range(NORMAL_MIN_TIME, NORMAL_MAX_TIME);
                EnemySpawn.Instance.Tier1(1, rnd2);

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
            }

            if (canSpawnAir)
            {
                float rnd = Random.Range(AIR_MIN_TIME, AIR_MAX_TIME);
                EnemySpawn.Instance.Mid(rnd);
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
    }

    public IEnumerator HomeMenu()
    {
        //Fade into the home menu
        blackOverlayAnim.SetBool("isFadeOut", true);
        yield return new WaitForSeconds(1.0f);

        menuCanvas.SetActive(true);

        loadingToaster.SetActive(false);
        gameplayCanvas.SetActive(false);
        controlCanvas.SetActive(false);
        endCanvas.SetActive(false);
        pauseCanvas.SetActive(false);

        blackOverlayAnim.SetBool("isFadeIn", true);
        blackOverlayAnim.SetBool("isFadeOut", false);
        yield return new WaitForSeconds(1.0f);
        blackOverlayAnim.SetBool("isFadeIn", false);

        Replay();

        yield return null;
    }

    public IEnumerator Tutorial()
    {
        //Fade into gameplay/tutorial
        blackOverlayAnim.SetBool("isFadeOut", true);
        yield return new WaitForSeconds(1.0f);

        gameplayCanvas.SetActive(true);
        tutorialCanvas.SetActive(true);

        loadingToaster.SetActive(false);
        menuCanvas.SetActive(false);

        blackOverlayAnim.SetBool("isFadeIn", true);
        blackOverlayAnim.SetBool("isFadeOut", false);
        yield return new WaitForSeconds(1.0f);
        blackOverlayAnim.SetBool("isFadeIn", false);

        yield return null;
    }

    public IEnumerator GameOver()
    {
        yield return new WaitForSeconds(1.0f);

        controlCanvas.SetActive(false);
        endCanvas.SetActive(true);

        yield return null;
    }

    //When the play button is pressed
    public void PlayButton()
    {
        nextScreen = 1;
        StartCoroutine(Loading());
    }

    //When the player closes the tutorial to start the game
    public void CloseTutorial()
    {
        nextScreen = 2;
        tutorialAnim.SetBool("isClose", true);
    }

    //When the player dies
    public void EndScreen()
    {
        nextScreen = 3;
        StartCoroutine(GameOver());
    }

    //When the player pauses the game
    public void PauseButton()
    {
        nextScreen = 4;
        controlCanvas.SetActive(false);
        pauseCanvas.SetActive(true);
        Time.timeScale = 0;
    }

    //When the player resumes the game
    public void ResumeGame()
    {
        nextScreen = 2;
        controlCanvas.SetActive(true);
        pauseCanvas.SetActive(false);
        Time.timeScale = 1;
    }

    //When the player presses the Home button
    public void HomeButton()
    {
        Time.timeScale = 1;
        nextScreen = 0;
        StartCoroutine(Loading());
    }

    //When the player presses the Replay button
    public void Replay()
    {
        RestartScript.Instance.RestartPlayer();
        RestartScript.Instance.RestartEnemies();
        RestartScript.Instance.RestartScore();
        RestartScript.Instance.RestartFridge();

        endCanvas.SetActive(false);
        
        nextScreen = 2;
        canSpawnTier1_1 = true;
        canSpawnTier1_2 = true;
    }

    //When the player presses the Settings button
    public void SettingsButton()
    {
        settingsCanvas.SetActive(true);
    }

    public void CloseSettings()
    {
        settingsCanvas.SetActive(false);
    }

    //Quit the game
    public void QuitGame()
    {
        Application.Quit();
    }
}

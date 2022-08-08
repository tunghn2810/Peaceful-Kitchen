using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlsManager : MonoBehaviour
{
    //Singleton
    public static ControlsManager Instance { get; set; }

    //Lists of control scripts for all characters
    private List<SwipeDetection> swipeDetections = new List<SwipeDetection>();
    private List<PlayerControl> playerControls = new List<PlayerControl>();

    //For switching controls
    public int currentMode = 1; //0 is Swipe, 1 is Basic
    public GameObject OnScreenControls;
    public GameObject pauseButton;

    //For switching characters
    public Transform startPos;
    private GameObject[] playerCharacters;
    public GameObject currentCharacter;
    private int currentCharacterIndex;
    private Vector3 lastPos;
    private Vector3 lastVel;

    public int currentLayer;
    public bool isAttacking;

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

    private void Start()
    {
        PlayerInit();
        ControlInit();

        //Testing only
        //BasicControl();
    }

    private void PlayerInit()
    {
        playerCharacters = GameObject.FindGameObjectsWithTag("Player");
        
        currentCharacterIndex = Random.Range(0, playerCharacters.Length);
        
        currentCharacter = playerCharacters[currentCharacterIndex];

        for (int i = 0; i < playerCharacters.Length; i++)
        {
            if (i != currentCharacterIndex)
            {
                TempDisable(playerCharacters[i]);
            }
        }

        currentCharacter.transform.position = startPos.position;
    }

    public void ControlInit()
    {
        //Add characters control scripts to the list
        for (int i = 0; i < playerCharacters.Length; i++)
        {
            swipeDetections.Add(playerCharacters[i].GetComponent<SwipeDetection>());
            playerControls.Add(playerCharacters[i].GetComponent<PlayerControl>());
        }
    }

    public void SwipeControl()
    {
        //Turn on swipe control for all characters
        for (int i = 0; i < playerCharacters.Length; i++)
        {
            swipeDetections[i].SwipeModeOn();
            playerControls[i].BasicModeOff();
        }

        OnScreenControls.SetActive(false);
        currentMode = 0;
    }

    public void BasicControl()
    {
        //Turn on basic control for all characters
        for (int i = 0; i < playerCharacters.Length; i++)
        {
            swipeDetections[i].SwipeModeOff();
            playerControls[i].BasicModeOn();
        }

        OnScreenControls.SetActive(true);
        currentMode = 1;
    }

    public void NoControl()
    {
        for (int i = 0; i < playerCharacters.Length; i++)
        {
            swipeDetections[i].SwipeModeOff();
            playerControls[i].BasicModeOff();
        }

        OnScreenControls.SetActive(false);
    }

    //Transform into another character
    public void Transformation()
    {
        //Record the last position and velocity (mainly y-axis) to bring to the next character
        lastPos = currentCharacter.transform.position;
        lastVel = currentCharacter.GetComponent<Rigidbody2D>().velocity;

        while (true)
        {
            int rnd = Random.Range(0, playerCharacters.Length);
            
            //Avoid repeating same character
            if (rnd != currentCharacterIndex)
            {
                currentCharacterIndex = rnd;
                break;
            }
        }

        TempDisable(currentCharacter); //Disable current character
        currentCharacter = playerCharacters[currentCharacterIndex]; //Set next character
        TempEnable(currentCharacter); //Enable next character

        currentCharacter.transform.position = lastPos; //Bring last position to the new one
        currentCharacter.GetComponent<Rigidbody2D>().velocity = lastVel; //Bring last velocity to the new one

        //Change side for all the player controls
        for (int i = 0; i < playerCharacters.Length; i++)
        {
            playerControls[i].ChangeSide();
            playerControls[i].TransformReset();
            playerControls[i].ChangeAura();
        }

        CameraScript.Instance.ChangeTarget(currentCharacter.transform);
    }

    //Temporarily disable some components of the inactive characters
    private void TempDisable(GameObject character)
    {
        for (int i = 0; i < character.transform.childCount; i++)
        {
            character.transform.GetChild(i).gameObject.SetActive(false);
        }
        character.GetComponent<Rigidbody2D>().simulated = false;
        character.GetComponent<PlayerControl>().isCurrent = false;
    }

    //Temporarily enable components the character being transformed into
    private void TempEnable(GameObject character)
    {
        for (int i = 0; i < character.transform.childCount; i++)
        {
            if (character.transform.GetChild(i).tag == "Ladle" || character.transform.GetChild(i).tag == "Board")
            {
                continue;
            }

            character.transform.GetChild(i).gameObject.SetActive(true);
        }

        character.GetComponent<Rigidbody2D>().simulated = true;
        character.GetComponent<PlayerControl>().isCurrent = true;
    }

    //When the attack ends - is called at the end of attack animation
    public void EndAttack()
    {
        isAttacking = false;
        
        if (currentCharacter.GetComponent<PotScript>() != null)
        {
            currentCharacter.GetComponent<PotScript>().BonkEnd();
        }
        else if (currentCharacter.GetComponent<CBScript>() != null)
        {
            currentCharacter.GetComponent<CBScript>().SlamEnd();
        }
        else if (currentCharacter.GetComponent<RCScript>() != null)
        {
            currentCharacter.GetComponent<RCScript>().CookRiceEnd();
        }
    }

    public void StartAura()
    {
        for (int i = 0; i < playerCharacters.Length; i++)
        {
            playerControls[i].StartAura();
        }
    }
}

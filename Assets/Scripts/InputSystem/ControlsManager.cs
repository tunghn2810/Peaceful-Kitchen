using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlsManager : MonoBehaviour
{
    //Singleton
    public static ControlsManager Instance { get; set; }

    //References
    //PlayerControl playerControl;
    //SwipeDetection swipeDetection;

    //Test - Control all characters
    List<SwipeDetection> swipeDetections = new List<SwipeDetection>();
    List<PlayerControl> playerControls = new List<PlayerControl>();

    public int currentMode = -1; //-1 is nothing, 0 is Swipe, 1 is Basic
    public GameObject OnScreenControls;

    public Transform startPos;
    [SerializeField] private GameObject[] playerCharacters;
    [SerializeField] private GameObject currentCharacter;
    [SerializeField] private int currentCharacterIndex;
    private Vector3 lastPos;
    private Vector3 lastVel;

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

        PlayerInit();
        ControlInit();
    }

    private void PlayerInit()
    {
        playerCharacters = GameObject.FindGameObjectsWithTag("Player");
        currentCharacterIndex = 0;
        currentCharacter = playerCharacters[currentCharacterIndex];

        for (int i = 1; i < playerCharacters.Length; i++)
        {
            playerCharacters[i].SetActive(false);
        }

        currentCharacter.transform.position = startPos.position;
    }

    public void ControlInit()
    {
        //playerControl = currentCharacter.GetComponent<PlayerControl>();
        //swipeDetection = currentCharacter.GetComponent<SwipeDetection>();

        //Test - Control all characters
        for (int i = 0; i < playerCharacters.Length; i++)
        {
            //swipeDetections[i] = playerCharacters[i].GetComponent<SwipeDetection>();
            //playerControls[i] = playerCharacters[i].GetComponent<PlayerControl>();

            swipeDetections.Add(playerCharacters[i].GetComponent<SwipeDetection>());
            playerControls.Add(playerCharacters[i].GetComponent<PlayerControl>());
        }

        //Only runs when player transforms
        if (currentMode == 0)
        {
            SwipeControl();
        }
        else if (currentMode == 1)
        {
            BasicControl();
        }
    }

    public void SwipeControl()
    {
        //swipeDetection.SwipeModeOn();
        //playerControl.BasicModeOff();

        //Test - Control all characters
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
        //swipeDetection.SwipeModeOff();
        //playerControl.BasicModeOn();

        //Test - Control all characters
        for (int i = 0; i < playerCharacters.Length; i++)
        {
            swipeDetections[i].SwipeModeOff();
            playerControls[i].BasicModeOn();
        }

        OnScreenControls.SetActive(true);
        currentMode = 1;
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
            
            //Avoid repeating position
            if (rnd != currentCharacterIndex)
            {
                currentCharacterIndex = rnd;
                break;
            }
        }

        currentCharacter.SetActive(false); //Disable last character
        currentCharacter = playerCharacters[currentCharacterIndex]; //Set next character
        currentCharacter.SetActive(true); //Enable next character
        currentCharacter.transform.position = lastPos; //Bring last position to the new one
        currentCharacter.GetComponent<Rigidbody2D>().velocity = lastVel; //Bring last velocity to the new one

        CameraScript.Instance.target = currentCharacter.transform; //Set camera to point at the new one
    }
}

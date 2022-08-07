using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialScript : MonoBehaviour
{
    public Sprite gamepadTut; //0
    public Sprite touchTut; //1
    public Sprite transformTut; //2
    public Sprite protectTut; //3

    public GameObject tutorialBoard;
    public GameObject nextButton;
    public GameObject prevButton;
    public GameObject confirmButton;

    private int currentPage = 0;

    private Sprite[] tutorialArray = new Sprite[3];

    private void Start()
    {
        tutorialArray[0] = gamepadTut;
        tutorialArray[1] = transformTut;
        tutorialArray[2] = protectTut;
    }

    private void Update()
    {
        if (ControlsManager.Instance.currentMode == 0)
        {
            tutorialArray[0] = touchTut;
        }
        else
        {
            tutorialArray[0] = gamepadTut;
        }
    }

    public void TutorialClosed()
    {
        GameStateScript.Instance.controlCanvas.SetActive(true);

        if (ControlsManager.Instance.currentMode == 0)
        {
            ControlsManager.Instance.SwipeControl();
        }
        else if (ControlsManager.Instance.currentMode == 1)
        {
            ControlsManager.Instance.BasicControl();
        }

        gameObject.transform.parent.gameObject.SetActive(false);
    }

    public void NextPage()
    {
        currentPage++;
        tutorialBoard.GetComponent<Image>().sprite = tutorialArray[currentPage];

        if (currentPage == 2)
        {
            nextButton.SetActive(false);
            confirmButton.SetActive(true);
        }

        if (!prevButton.activeSelf)
        {
            prevButton.SetActive(true);
        }
    }

    public void PrevPage()
    {
        currentPage--;
        tutorialBoard.GetComponent<Image>().sprite = tutorialArray[currentPage];

        if (currentPage == 0)
        {
            prevButton.SetActive(false);
        }

        if (!nextButton.activeSelf)
        {
            nextButton.SetActive(true);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DetailedTutorialScript : MonoBehaviour
{
    public GameObject tutorialBoard;
    public GameObject nextButton;
    public GameObject prevButton;

    private int currentPage = 0;

    public Sprite[] tutorialSprites;

    public void TutorialClosed()
    {
        //currentPage = 0;
        //tutorialBoard.GetComponent<Image>().sprite = tutorialSprites[currentPage];

        gameObject.GetComponent<RectTransform>().localScale = Vector3.one;
        gameObject.transform.parent.gameObject.SetActive(false);
    }

    public void NextPage()
    {
        currentPage++;
        tutorialBoard.GetComponent<Image>().sprite = tutorialSprites[currentPage];

        if (currentPage == tutorialSprites.Length - 1)
        {
            nextButton.SetActive(false);
        }

        if (!prevButton.activeSelf)
        {
            prevButton.SetActive(true);
        }
    }

    public void PrevPage()
    {
        currentPage--;
        tutorialBoard.GetComponent<Image>().sprite = tutorialSprites[currentPage];

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

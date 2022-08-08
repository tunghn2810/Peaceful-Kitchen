using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarningAnimScript : MonoBehaviour
{
    //private Animator anim;
    public GameObject darkBG;

    private void Awake()
    {
        //anim = GetComponent<Animator>();
    }

    public void WarningClosed()
    {
        gameObject.GetComponent<RectTransform>().localScale = Vector3.one;
        darkBG.SetActive(false);
    }

    public void OpenWarning()
    {
        darkBG.SetActive(true);
    }

    public void CloseWarning()
    {
        //anim.SetBool("isClose", true);
        darkBG.SetActive(false);
        gameObject.SetActive(false);
    }
}

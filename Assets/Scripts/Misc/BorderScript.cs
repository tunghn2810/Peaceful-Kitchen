using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BorderScript : MonoBehaviour
{
    public Sprite borderVegie;
    public Sprite borderMeat;

    public Image border;

    //0 = vegie, 1 = meat
    private int currentBorder = 0;

    //Singleton
    public static BorderScript Instance { get; set; }

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

    public void ChangeBorder()
    {
        currentBorder = currentBorder == 0 ? 1 : 0;
        if (currentBorder == 0)
        {
            border.sprite = borderVegie;
        }
        else
        {
            border.sprite = borderMeat;
        }
    }
}

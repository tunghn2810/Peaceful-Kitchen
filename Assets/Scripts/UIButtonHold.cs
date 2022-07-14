using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIButtonHold : MonoBehaviour
{
    public bool isPressed;
    private int leftRight;


    private PlayerControl playerControl;

    private void Awake()
    {
        playerControl = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>();
    }

    public void Update()
    {
        if (isPressed)
        {
            if (leftRight == 0)
            {
                playerControl.isMoveLeft = true;
            }
            else if (leftRight == 1)
            {
                playerControl.isMoveRight = true;
            }
        }
        else
        {
            playerControl.isMoveLeft = false;
            playerControl.isMoveRight = false;
        }
    }

    // 0 is left, 1 is right
    public void HoldDown(int num)
    {
        isPressed = true;
        leftRight = num;
    }
    public void Release()
    {
        isPressed = false;
    }
}

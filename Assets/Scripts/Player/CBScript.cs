using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CBScript : MonoBehaviour
{
    private GameObject hitBoxSlam;
    private PlayerControl playerControl;

    private void Awake()
    {
        hitBoxSlam = transform.GetChild(2).gameObject;
        playerControl = GetComponent<PlayerControl>();
        playerControl.moveSpeed = 15;
    }

    private void Slam()
    {
        if (playerControl.isCurrent)
        {
            hitBoxSlam.SetActive(true);
            playerControl.isSlaming = true;
            gameObject.layer = Layer.Invincible;
        }
    }

    private void Recover()
    {
        hitBoxSlam.SetActive(false);
        playerControl.isRecovering = true;
    }

    private void SlamEnd()
    {
        playerControl.isSlaming = false;
        gameObject.layer = Layer.Player;
        playerControl.isRecovering = false;
    }
}

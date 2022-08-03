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

    private void SlamWindUp()
    {
        if (playerControl.isCurrent)
        {
            gameObject.layer = Layer.Invincible;
            playerControl.GetComponent<Rigidbody2D>().constraints |= RigidbodyConstraints2D.FreezePositionY;
        }
    }

    private void Slam()
    {
        if (playerControl.isCurrent)
        {
            hitBoxSlam.SetActive(true);
            playerControl.isSlaming = true;
            playerControl.GetComponent<Rigidbody2D>().constraints &= ~RigidbodyConstraints2D.FreezePositionY;
        }
    }

    private void Recover()
    {
        hitBoxSlam.SetActive(false);
        playerControl.isRecovering = true;
    }

    public void SlamEnd()
    {
        playerControl.isSlaming = false;
        gameObject.layer = ControlsManager.Instance.currentLayer;
        playerControl.isRecovering = false;
        playerControl.slamShakeOnce = false;
    }
}

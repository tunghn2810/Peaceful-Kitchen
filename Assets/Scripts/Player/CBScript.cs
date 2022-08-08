using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CBScript : MonoBehaviour
{
    private GameObject hitBoxSlam;
    private GameObject hitBoxShock;
    private PlayerControl playerControl;

    private Animator anim;

    private void Awake()
    {
        hitBoxSlam = transform.GetChild(3).gameObject;
        hitBoxShock = transform.GetChild(4).gameObject;
        playerControl = GetComponent<PlayerControl>();

        anim = GetComponent<Animator>();
    }

    private void SlamWindUp()
    {
        if (playerControl.isCurrent)
        {
            gameObject.layer = Layer.Invincible;
            playerControl.GetComponent<Rigidbody2D>().constraints |= RigidbodyConstraints2D.FreezePositionY;
        }
    }

    public void SlamStart()
    {
        anim.SetBool("isSlamming", true);
    }

    private void Slam()
    {
        if (playerControl.isCurrent)
        {
            hitBoxSlam.SetActive(true);
            playerControl.isSlamming = true;
            playerControl.GetComponent<Rigidbody2D>().constraints &= ~RigidbodyConstraints2D.FreezePositionY;
        }
    }

    public void ShockStart()
    {
        hitBoxShock.SetActive(true);
    }

    private void SlamDone()
    {
        anim.SetBool("isSlamming", false);
    }

    private void Recover()
    {
        hitBoxSlam.SetActive(false);
        playerControl.isRecovering = true;
    }

    public void SlamEnd()
    {
        playerControl.isSlamming = false;
        gameObject.layer = ControlsManager.Instance.currentLayer;
        playerControl.isRecovering = false;
        playerControl.slamShakeOnce = false;

        hitBoxShock.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToastScript : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rgbd;
    private float moveSpeed = 25f;

    private PlayerControl playerControl;
    int flip;

    bool isFadeOut;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        rgbd = GetComponent<Rigidbody2D>();
        playerControl = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>();
        flip = playerControl.isFacingRight ? 1 : -1;

        rgbd.velocity = new Vector2(moveSpeed * flip, rgbd.velocity.y);
    }

    private void Disappear()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 6) //6 is Enemy layer
        {
            isFadeOut = true;
            anim.SetBool("isFadeOut", isFadeOut);
        }
    }
}

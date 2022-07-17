using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToastScript : MonoBehaviour
{
    //References
    private Animator anim;
    private Rigidbody2D rgbd;
    private PlayerControl playerControl;

    //Stats
    private float moveSpeed = 25f;

    //Bool checks
    private float flip;
    bool isFadeOut;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        rgbd = GetComponent<Rigidbody2D>();
        playerControl = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>();

        flip = playerControl.flip;
        gameObject.transform.localScale = new Vector3(transform.localScale.x * flip, transform.localScale.y, transform.localScale.z);
        rgbd.velocity = new Vector2(moveSpeed * flip, rgbd.velocity.y);

        Destroy(gameObject, 5f);
    }

    private void Disappear()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == Layer.Enemy)
        {
            isFadeOut = true;
            anim.SetBool("isFadeOut", isFadeOut);
        }
    }
}

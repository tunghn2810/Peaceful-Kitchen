using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiceBallGroundCheck : MonoBehaviour
{
    private Animator anim;

    private void Awake()
    {
        anim = GetComponentInParent<Animator>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == Layer.Platform)
        {
            anim.SetBool("isGrounded", true);
        }
    }
}

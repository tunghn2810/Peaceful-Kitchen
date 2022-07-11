using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rgbd;
    private float moveSpeed = 5f;

    bool isDead = false;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        rgbd = GetComponent<Rigidbody2D>();

        if (gameObject.tag == "Vegetable")
        {
            rgbd.velocity = new Vector2(moveSpeed, rgbd.velocity.y);
        }
        else if (gameObject.tag == "Meat")
        {
            rgbd.velocity = new Vector2(moveSpeed * -1.0f, rgbd.velocity.y);
        }
    }

    private void FixedUpdate()
    {
        if (!isDead)
        {
            //if (gameObject.tag == "Vegetable")
            //{
            //    rgbd.velocity = new Vector2(moveSpeed, rgbd.velocity.y);
            //}
            //else if (gameObject.tag == "Meat")
            //{
            //    rgbd.velocity = new Vector2(moveSpeed * -1.0f, rgbd.velocity.y);
            //}
        }
        else
        {
            rgbd.velocity = new Vector2(0, rgbd.velocity.y);
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 9) //9 is Weapon layer
        {
            isDead = true;
            anim.SetBool("isDead", isDead);
        }
    }
}

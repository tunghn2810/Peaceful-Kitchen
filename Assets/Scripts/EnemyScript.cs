using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rgbd;
    private float moveSpeed = 5f;
    private int flip;

    bool isDead = false;
    bool isJump = false;
    bool isFridge = false;
    bool activeOnce = true;
    private float jumpSpeed = 105f;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        rgbd = GetComponent<Rigidbody2D>();

        if (gameObject.tag == "Vegetable")
        {
            flip = 1;
        }
        else if (gameObject.tag == "Meat")
        {
            flip = -1;
        }

        rgbd.velocity = new Vector2(moveSpeed * flip, rgbd.velocity.y);
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

        if (isJump)
        {
            rgbd.AddForce(Vector3.up * jumpSpeed, ForceMode2D.Impulse);
            isJump = false;
        }
        
        if (isFridge && activeOnce)
        {
            Debug.Log("ABCBBB");
            rgbd.AddForce(new Vector2(flip, 1) * jumpSpeed, ForceMode2D.Impulse);

            activeOnce = false;

            Destroy(gameObject, 5);
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 11) //10 is JumpPad layer
        {
            isJump = true;
        }
        else if (collision.gameObject.layer == 10) //10 is Fridge layer
        {
            isFridge = true;
        }
    }
}

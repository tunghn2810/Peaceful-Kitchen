using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    //References
    private Animator anim;
    private Rigidbody2D rgbd;
    public GameObject dieParticle;

    //Stats
    private float moveSpeed = 5f;
    private float currVelocity;
    private float flip;

    //Bool checks
    private bool isDead = false;
    private bool isJump = false;
    private bool isFridge = false;
    private bool activeOnce = true;
    private float jumpSpeed = 105f;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        rgbd = GetComponent<Rigidbody2D>();

        //if (gameObject.tag == "Vegetable")
        //{
        //    flip = 1;
        //}
        //else if (gameObject.tag == "Meat")
        //{
        //    flip = -1;
        //}

        //rgbd.velocity = new Vector2(moveSpeed * flip, rgbd.velocity.y);
    }

    //Set the speed of the enemy when it is spawned
    public void SetSpeed(int p_flip)
    {
        transform.localScale = new Vector3(transform.localScale.x * p_flip, transform.localScale.y, transform.localScale.z);
        rgbd.velocity = new Vector2(moveSpeed * p_flip, rgbd.velocity.y);
        currVelocity = rgbd.velocity.x;
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
            //Stop moving horizontally when it dies
            rgbd.velocity = new Vector2(0, rgbd.velocity.y);
        }

        //Jump when prompted to
        if (isJump)
        {
            rgbd.AddForce(Vector3.up * jumpSpeed, ForceMode2D.Impulse);
            isJump = false;
        }
        
        //When it reaches the fridge - TODO
        if (isFridge && activeOnce)
        {
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
        if (collision.gameObject.layer == Layer.Weapon)
        {
            Instantiate(dieParticle, gameObject.transform.position, Quaternion.identity);
            isDead = true;
            anim.SetBool("isDead", isDead);
            gameObject.layer = Layer.Default;
        }

        if (collision.gameObject.layer == Layer.Platform)
        {
            if (collision.gameObject.tag == "Wall")
            {
                rgbd.velocity = new Vector2(-currVelocity, rgbd.velocity.y);
                currVelocity = rgbd.velocity.x;
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == Layer.Weapon)
        {
            Instantiate(dieParticle, gameObject.transform.position, Quaternion.identity);
            isDead = true;
            anim.SetBool("isDead", isDead);
            gameObject.layer = Layer.Default;

        }

        if (collision.gameObject.layer == Layer.JumpPad)
        {
            isJump = true;
        }
        
        if (collision.gameObject.layer == Layer.Fridge)
        {
            isFridge = true;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//Parent class for all player characters
public class PlayerControl : MonoBehaviour
{
    //References
    private Rigidbody2D rgbd;
    private Animator anim;
    public GameObject dieEffect;

    //Bool checks
    public bool isGrounded;
    private bool isAttacking;
    private bool isMoving; //When the move button is being held
    private bool isJumping; //When the jump button is being held
    private float moveDirection; //Move direction updated by input direction (up, down, left, right)

    //Special bool checks for slamming with cutting board
    public bool isSlaming = false;
    public bool isRecovering = false;

    //Flip - x of move direction
    public float flip = 1; //1 is not flip, -1 is flip

    //General stats
    private float jumpSpeed = 22f;
    public float moveSpeed = 10f; //Base speed. Characters may change this

    //Currently active
    public bool isCurrent = true;

    private void Awake()
    {
        //References
        rgbd = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    public void BasicModeOn()
    {
        InputManager.Instance.OnMoveStart += MoveStart;
        InputManager.Instance.OnMoveEnd += MoveEnd;
        InputManager.Instance.OnJump += Jump;
        InputManager.Instance.OnAttack += Attack;
    }

    public void BasicModeOff()
    {
        InputManager.Instance.OnMoveStart -= MoveStart;
        InputManager.Instance.OnMoveEnd -= MoveEnd;
        InputManager.Instance.OnJump -= Jump;
        InputManager.Instance.OnAttack -= Attack;
    }

    private void Update()
    {
        //Update animations
        anim.SetBool("isMoving", isMoving);
        anim.SetFloat("VelocityY", rgbd.velocity.y);
        anim.SetBool("isGrounded", isGrounded);
        anim.SetBool("isAttacking", isAttacking);
    }

    private void FixedUpdate()
    {
        //For Swipe control
        if (ControlsManager.Instance.currentMode == 0)
        {
            //TODO
        }

        //For Basic control & Dpad control
        if (ControlsManager.Instance.currentMode == 1)
        {
            if (!isSlaming)
            {
                //Update velocity every frame
                if (isMoving)
                {
                    rgbd.velocity = new Vector2(moveDirection * moveSpeed, rgbd.velocity.y);
                }
                else
                {
                    rgbd.velocity = new Vector2(0, rgbd.velocity.y);
                }

                //Jump if the Jump button is held and the character is grounded
                if (isJumping && isGrounded)
                {
                    rgbd.velocity = new Vector2(rgbd.velocity.x, 0); //Reset velocity to preven forces of opposite directions
                    rgbd.AddForce(Vector3.up * jumpSpeed, ForceMode2D.Impulse);
                }
            }
            else //Only happens when slamming
            {
                if (isGrounded)
                {
                    if (isRecovering)
                    {
                        rgbd.velocity = new Vector2(0, 0);
                    }
                    else
                    {
                        rgbd.velocity = new Vector2(moveDirection * moveSpeed, 0);
                    }
                }
                else
                {
                    rgbd.velocity = new Vector2(moveDirection * moveSpeed, -25f);
                }
            }
        }
    }

    //Jump when Jump button is pressed
    public void Jump(bool isHeld)
    {
        isJumping = isHeld;
    }

    //Jump when swipe - Swipe control only
    public void JumpSwipe()
    {
        //Jump if the character is grounded
        if (isGrounded)
        {
            rgbd.velocity = new Vector2(rgbd.velocity.x, 0); //Reset velocity to preven forces of opposite directions
            rgbd.AddForce(Vector3.up * jumpSpeed, ForceMode2D.Impulse);
        }
    }

    //When the Move button is pressed
    public void MoveStart(Vector2 direction)
    {
        isMoving = true;
        moveDirection = flip = direction.x;
        transform.localScale = new Vector3(transform.localScale.x * Flip(flip, transform.localScale.x), transform.localScale.y, transform.localScale.z);
    }

    //When the Move button is released
    public void MoveEnd()
    {
        isMoving = false;
        moveDirection = 0;
    }

    //Supplement function to check if the object needs to be flipped
    public float Flip(float currentFlip, float toFlip)
    {
        if ((currentFlip * toFlip) < 0)
        {
            return -1;
        }
        else
        {
            return 1;
        }
    }

    //When the Attack button is pressed
    public void Attack()
    {
        if (!isAttacking)
        {
            isAttacking = true;
        }
    }

    //When the attack ends - is called at the end of attack animation
    public void EndAttack()
    {
        isAttacking = false;
    }

    public void TapTest(Vector2 position)
    {
        if (position.x < gameObject.transform.position.x)
        {
            flip = -1;
        }
        else if (position.x > gameObject.transform.position.x)
        {
            flip = 1;
        }
        Attack();
    }

    public void SlideAttack(bool isLookingRight)
    {
        if (isLookingRight)
        {
            flip = 1;
            rgbd.velocity = new Vector2(0, rgbd.velocity.y);
            rgbd.AddForce(Vector2.right * 20f, ForceMode2D.Impulse);
            transform.localScale = new Vector3(transform.localScale.x * Flip(flip, transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else
        {
            flip = -1;
            rgbd.velocity = new Vector2(0, rgbd.velocity.y);
            rgbd.AddForce(Vector2.left * 20f, ForceMode2D.Impulse);
            transform.localScale = new Vector3(transform.localScale.x * Flip(flip, transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        Attack();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == Layer.Enemy)
        {
            Die();
        }
    }

    private void Die()
    {
        //Spawn the particle effect
        GameObject particle = Instantiate(dieEffect, gameObject.transform.position, dieEffect.transform.rotation);
        Destroy(particle, 3.0f);

        gameObject.SetActive(false);
    }
}

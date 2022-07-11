using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{
    //Components for input
    private Rigidbody2D rgbd;
    private PlayerInput playerInput;
    
    //Components for animation
    private Animator anim;
    private SpriteRenderer[] sprites;

    private bool isGrounded;
    public bool isFacingRight;
    private bool isAttacking;

    private float jumpSpeed = 22f;
    private float moveSpeed = 10f;

    //Projectile prefabs
    public GameObject toast;

    //Projectile start positions
    public GameObject toastPos;
    
    [SerializeField]
    private Transform[] shootPos;

    private void Awake()
    {
        rgbd = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();

        anim = GetComponent<Animator>();
        sprites = GetComponentsInChildren<SpriteRenderer>();

        shootPos = toastPos.GetComponentsInChildren<Transform>();
    }

    private void FixedUpdate()
    {
        Vector2 inputVector = playerInput.actions["Move"].ReadValue<Vector2>();
        //rgbd.AddForce(new Vector3(inputVector.x, 0, inputVector.y) * moveSpeed, ForceMode2D.Force);

        if (inputVector.x > 0)
        {
            isFacingRight = true;
        }
        else if (inputVector.x < 0)
        {
            isFacingRight = false;
        }

        for (int i = 0; i<sprites.Length; i++)
        {
            sprites[i].flipX = isFacingRight;
        }

        if (inputVector.x != 0)
        {
            anim.SetBool("isMoving", true);
        }
        else
        {
            anim.SetBool("isMoving", false);
        }

        rgbd.velocity = new Vector2(inputVector.x * moveSpeed, rgbd.velocity.y);

        anim.SetFloat("VelocityY", rgbd.velocity.y);

        if (rgbd.velocity.y < 0.001f && rgbd.velocity.y > -0.001f)
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
        anim.SetBool("isGrounded", isGrounded);

        anim.SetBool("isAttacking", isAttacking);
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (isGrounded)
            {
                rgbd.AddForce(Vector3.up * jumpSpeed, ForceMode2D.Impulse);
            }
        }
    }

    public void Attack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (!isAttacking)
            {
                isAttacking = true;
                //ShootToasts();
            }
        }
    }

    private void ShootToasts()
    {
        for (int i = 1; i < shootPos.Length; i++)
        {
            GameObject newToast = Instantiate(toast, shootPos[i].position, Quaternion.identity);

            if (isFacingRight)
            {
                newToast.GetComponent<SpriteRenderer>().flipX = true;

                toastPos.transform.localRotation = Quaternion.Euler(0, 180, 0);
            }
        }

    }

    public void EndAttack()
    {
        isAttacking = false;
    }
}

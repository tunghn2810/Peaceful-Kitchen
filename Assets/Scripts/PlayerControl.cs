using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{
    //Components for input
    private Rigidbody2D rgbd;
    private PlayerInputActions playerInputActions;

    //Components for animation
    private Animator anim;
    private SpriteRenderer[] sprites;

    private bool isGrounded;
    private bool isAttacking;
    public bool isFacingRight;

    private float jumpSpeed = 22f;
    private float moveSpeed = 10f;

    //Projectile prefabs
    public GameObject toast;

    //Projectile start positions
    public GameObject toastPos;
    
    [SerializeField]
    private Transform[] shootPos;

    //For singleton
    private InputManager inputManager;

    //For UI buttons
    public bool isMoveLeft;
    public bool isMoveRight;

    private void Awake()
    {
        //For singleton
        inputManager = InputManager.Instance;

        rgbd = GetComponent<Rigidbody2D>();

        //playerInputActions = new PlayerInputActions();
        //playerInputActions.Player.Enable();
        //playerInputActions.Player.Jump.performed += Jump;
        //playerInputActions.Player.Attack.performed += Attack;

        anim = GetComponent<Animator>();
        sprites = GetComponentsInChildren<SpriteRenderer>();

        shootPos = toastPos.GetComponentsInChildren<Transform>();
    }

    private void OnEnable()
    {
        inputManager.OnJumpButton += Jump;
        inputManager.OnTap += TapTest;
    }

    private void OnDisable()
    {
        inputManager.OnJumpButton -= Jump;
        inputManager.OnTap -= TapTest;
    }

    private void FixedUpdate()
    {
        if (ControlsManager.Instance.OnScreenControls.activeSelf == true)
        {
            Move();
        }
        //Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();
        //
        //if (inputVector.x > 0)
        //{
        //    isFacingRight = true;
        //}
        //else if (inputVector.x < 0)
        //{
        //    isFacingRight = false;
        //}
        
        for (int i = 0; i<sprites.Length; i++)
        {
            sprites[i].flipX = isFacingRight;
        }
        
        //if (inputVector.x != 0)
        //{
        //    anim.SetBool("isMoving", true);
        //}
        //else
        //{
        //    anim.SetBool("isMoving", false);
        //}
        //
        //rgbd.velocity = new Vector2(inputVector.x * moveSpeed, rgbd.velocity.y);
        //
        //anim.SetFloat("VelocityY", rgbd.velocity.y);

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

    //public void Jump(InputAction.CallbackContext context)
    //{
    //    if (isGrounded)
    //    {
    //        rgbd.AddForce(Vector3.up * jumpSpeed, ForceMode2D.Impulse);
    //    }
    //}

    public void Jump()
    {
        if (isGrounded)
        {
            rgbd.AddForce(Vector3.up * jumpSpeed, ForceMode2D.Impulse);
        }
    }

    //public void Attack(InputAction.CallbackContext context)
    //{
    //    if (!isAttacking)
    //    {
    //        isAttacking = true;
    //        //ShootToasts();
    //    }
    //}

    private void Move()
    {
        int moveDirection = 1;

        if (isMoveLeft || isMoveRight)
        {
            if (isMoveLeft)
            {
                isFacingRight = false;
                moveDirection = -1;
            }
            else if (isMoveRight)
            {
                isFacingRight = true;
                moveDirection = 1;
            }

            anim.SetBool("isMoving", true);
        }
        else
        {
            moveDirection = 0;
            anim.SetBool("isMoving", false);
        }
        rgbd.velocity = new Vector2(moveDirection * moveSpeed, rgbd.velocity.y);
        anim.SetFloat("VelocityY", rgbd.velocity.y);
    }

    public void Attack()
    {
        if (!isAttacking)
        {
            isAttacking = true;
            //ShootToasts();
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


    private void Bonk()
    {
        //TODO: Bonking stuffs
    }

    public void EndAttack()
    {
        isAttacking = false;
    }

    public void TapTest(Vector2 position)
    {
        Debug.Log("Tap");
        if (position.x < gameObject.transform.position.x)
        {
            isFacingRight = false;
        }
        else if (position.x > gameObject.transform.position.x)
        {
            isFacingRight = true;
        }
        Attack();
    }

    public void SlideAttack(bool isLookingRight)
    {
        if (isLookingRight)
        {
            Debug.Log("Slide Right");
            isFacingRight = true;
            rgbd.velocity = new Vector2(0, rgbd.velocity.y);
            rgbd.AddForce(Vector2.right * 20f, ForceMode2D.Impulse);
        }
        else
        {
            Debug.Log("Slide Left");
            isFacingRight = false;
            rgbd.velocity = new Vector2(0, rgbd.velocity.y);
            rgbd.AddForce(Vector2.left * 20f, ForceMode2D.Impulse);
        }
        Attack();
    }
}

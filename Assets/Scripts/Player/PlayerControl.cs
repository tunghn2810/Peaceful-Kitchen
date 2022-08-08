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
    private SpriteRenderer sprite;
    [SerializeField] private AuraScript aura;
    [SerializeField] private WeaponScript[] weapons; //Only for pot and rice cooker

    //Bool checks
    public bool isGrounded;
    private bool isMoving; //When the move button is being held
    private bool isJumping; //When the jump button is being held
    private float moveDirection; //Move direction updated by input direction (up, down, left, right)

    //Special bool checks for slamming with cutting board
    public bool isSlamming = false;
    public bool isRecovering = false;
    public bool slamShakeOnce = false;

    //Special bool checks for rice cooker
    public bool isCooking = false;

    //Flip - x of move direction
    public float flip = 1; //1 is not flip, -1 is flip

    //General stats
    private float jumpSpeed = 25f;
    public float moveSpeed; //Base speed. Characters may change this

    //Currently active
    public bool isCurrent = true;

    //Next layer - for invincibility after transforming
    [SerializeField] private int nextLayer;

    private void Awake()
    {
        //References
        rgbd = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = transform.GetChild(0).GetComponent<SpriteRenderer>();

        aura = GetComponentInChildren<AuraScript>();

        weapons = GetComponentsInChildren<WeaponScript>();
        if (weapons.Length > 0)
        {
            for (int i = 0; i < weapons.Length; i++)
            {
                weapons[i].gameObject.SetActive(false);
            }
        }
    }

    private void Start()
    {
        ControlsManager.Instance.currentLayer = gameObject.layer;
        nextLayer = gameObject.layer;
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
        anim.SetBool("isAttacking", ControlsManager.Instance.isAttacking);
    }

    private void FixedUpdate()
    {
        if (isSlamming) //Only happens when slamming
        {
            if (isGrounded)
            {
                rgbd.velocity = new Vector2(0, 0);

                if (!slamShakeOnce)
                {
                    CameraShake.Instance.ShakeCamera();
                    slamShakeOnce = true;
                }

                ControlsManager.Instance.currentCharacter.GetComponent<CBScript>().ShockStart();
            }
            else
            {
                rgbd.velocity = new Vector2(moveDirection * moveSpeed / 3, -30f);
            }
        }
        else if (isCooking)
        {
            rgbd.velocity = new Vector2(0, rgbd.velocity.y);
        }
        else
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
                JumpEffect();
                rgbd.velocity = new Vector2(rgbd.velocity.x, 0); //Reset velocity to preven forces of opposite directions
                rgbd.AddForce(Vector3.up * jumpSpeed, ForceMode2D.Impulse);
            }
        }

        #region split control, but basically the same
        /*
        //For Swipe control
        if (ControlsManager.Instance.currentMode == 0)
        {
            if (isSlaming) //Only happens when slamming
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
                        if (!slamShakeOnce)
                        {
                            CameraShake.Instance.ShakeCamera();
                            slamShakeOnce = true;
                        }
                    }
                }
                else
                {
                    rgbd.velocity = new Vector2(moveDirection * moveSpeed, -30f);
                }
            }
            else if (isCooking)
            {
                rgbd.velocity = new Vector2(0, rgbd.velocity.y);
            }
            else
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
                    JumpEffect();
                    rgbd.velocity = new Vector2(rgbd.velocity.x, 0); //Reset velocity to preven forces of opposite directions
                    rgbd.AddForce(Vector3.up * jumpSpeed, ForceMode2D.Impulse);
                }
            }
        }

        //For Basic control & Dpad control
        else if (ControlsManager.Instance.currentMode == 1)
        {
            if (isSlaming) //Only happens when slamming
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
                        if (!slamShakeOnce)
                        {
                            CameraShake.Instance.ShakeCamera();
                            slamShakeOnce = true;
                        }
                    }
                }
                else
                {
                    rgbd.velocity = new Vector2(moveDirection * moveSpeed, -30f);
                }
            }
            else if (isCooking)
            {
                rgbd.velocity = new Vector2(0, rgbd.velocity.y);
            }
            else
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
                    JumpEffect();
                    rgbd.velocity = new Vector2(rgbd.velocity.x, 0); //Reset velocity to preven forces of opposite directions
                    rgbd.AddForce(Vector3.up * jumpSpeed, ForceMode2D.Impulse);
                }
            }
        }
        */
        #endregion
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
        if (!ControlsManager.Instance.isAttacking)
        {
            ControlsManager.Instance.isAttacking = true;
            
            if (ControlsManager.Instance.currentCharacter.GetComponent<CBScript>() != null)
            {
                ControlsManager.Instance.currentCharacter.GetComponent<CBScript>().SlamStart();
            }

            if (ControlsManager.Instance.currentCharacter.GetComponent<PlayerControl>().weapons.Length > 0)
            {
                for (int i = 0; i < ControlsManager.Instance.currentCharacter.GetComponent<PlayerControl>().weapons.Length; i++)
                {
                    ControlsManager.Instance.currentCharacter.GetComponent<PlayerControl>().weapons[i].ChangeLayer();
                }
            }
        }
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

    public void TapAttack(bool isLookingRight)
    {
        if (isLookingRight)
        {
            flip = 1;
            transform.localScale = new Vector3(transform.localScale.x * Flip(flip, transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else
        {
            flip = -1;
            transform.localScale = new Vector3(transform.localScale.x * Flip(flip, transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        Attack();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == Layer.Enemy_Veg && gameObject.layer == Layer.Player_Meat)
        {
            Die();
        }
        if (collision.gameObject.layer == Layer.Enemy_Meat && gameObject.layer == Layer.Player_Veg)
        {
            Die();
        }
    }

    private void Die()
    {
        //Spawn the effect game object
        GameObject effect = Instantiate(EffectReferences.Instance.playerDie, gameObject.transform.position, EffectReferences.Instance.playerDie.transform.rotation);
        Destroy(effect, 3.0f);

        GameStateScript.Instance.EndScreen();

        gameObject.SetActive(false);
    }

    private void JumpEffect()
    {
        GameObject effect = Instantiate(EffectReferences.Instance.playerJump, gameObject.transform.GetChild(1).position - new Vector3(0, 1, 0), EffectReferences.Instance.playerJump.transform.rotation);
        Destroy(effect, 3.0f);
    }

    public void ChangeSide()
    {
        nextLayer = nextLayer == Layer.Player_Veg ? Layer.Player_Meat : Layer.Player_Veg;
        gameObject.layer = Layer.Invincible;
        BlinkingSprite();
        Invoke("EndInvincibility", 1f);
        ControlsManager.Instance.currentLayer = nextLayer;
    }

    private void EndInvincibility()
    {
        gameObject.layer = nextLayer;
    }

    private void BlinkingSprite()
    {
        StartCoroutine(Blinking());
    }

    private IEnumerator Blinking()
    {
        Color currentColor = Color.white;
        Color lowOpac = new Color(currentColor.r, currentColor.g, currentColor.b, currentColor.a / 2);
        float time = 0.5f;
        float timeStep = 0.1f;

        for (float i = 0; i < time; i += timeStep)
        {
            sprite.color = lowOpac;

            yield return new WaitForSeconds(timeStep);

            sprite.color = currentColor;

            yield return new WaitForSeconds(timeStep);
        }

        sprite.color = currentColor;
    }

    public void TransformReset()
    {
        ControlsManager.Instance.isAttacking = false;
        isSlamming = false;
        isRecovering = false;
        slamShakeOnce = false;
        isCooking = false;

        anim.SetBool("isReset", true);
        rgbd.constraints &= ~RigidbodyConstraints2D.FreezePositionY; //Reset the freeze Y when cutting board slams
    }

    public void ChangeAura()
    {
        aura.ChangeAura();
    }

    public void StartAura()
    {
        aura.StartAura();
    }
}

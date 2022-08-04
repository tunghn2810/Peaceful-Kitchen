using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFlyingScript : MonoBehaviour
{
    //References
    private Animator anim;
    private Rigidbody2D rgbd;
    public LayerMask layerMask;
    public GameObject dieParticle;

    //Stats
    private float moveSpeed = 3f;
    [SerializeField] private Vector2 currVelocity;
    private Vector3 directionToMove = Vector3.right;
    private float flip;
    private float health = 30f;

    //Bool checks
    private bool isDead = false;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        rgbd = GetComponent<Rigidbody2D>();
    }

    //Set the speed of the enemy when it is spawned
    public void SetSpeed(int p_flip)
    {
        transform.localScale = new Vector3(transform.localScale.x * p_flip, transform.localScale.y, transform.localScale.z);
        rgbd.velocity = new Vector2(directionToMove.x * moveSpeed * p_flip, directionToMove.y);
        currVelocity = rgbd.velocity;
        flip = p_flip;
    }

    private void Start()
    {
        SetSpeed(1);
    }

    private void FixedUpdate()
    {
        if (GameStateScript.Instance.startGame)
        {
            if (!isDead)
            {
                Vector3 directionToPlayer = ControlsManager.Instance.currentCharacter.transform.position - gameObject.transform.position;

                RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlayer, directionToPlayer.magnitude, layerMask);
                if (hit.collider != null)
                {
                    if (hit.collider.gameObject.layer == Layer.Player_Veg || hit.collider.gameObject.layer == Layer.Player_Meat)
                    {
                        directionToMove = directionToPlayer.normalized;

                        int toFlip = directionToMove.x > 0 ? 1 : -1;
                        transform.localScale = new Vector3(transform.localScale.x * Flip(toFlip, transform.localScale.x), transform.localScale.y, transform.localScale.z);

                        rgbd.velocity = directionToMove * moveSpeed;
                        currVelocity = rgbd.velocity;
                    }
                }
            }
            else
            {
                //Stop moving when it dies
                rgbd.velocity = new Vector2(0, 0);
            }
        }
        else
        {
            rgbd.velocity = new Vector2(0, 0);
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    private void Explode()
    {
        Instantiate(dieParticle, gameObject.transform.position, Quaternion.identity);
        isDead = true;
        anim.SetBool("isDead", isDead);
        gameObject.layer = Layer.NoCol;
    }

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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == Layer.Weapon_Veg || collision.gameObject.layer == Layer.Weapon_Meat)
        {
            if (collision.gameObject.tag == "Toast")
            {
                collision.gameObject.GetComponent<ToastScript>().Die();
            }

            if (collision.gameObject.tag == "RiceBall")
            {
                collision.gameObject.GetComponent<RiceScript>().Explode();
            }

            health -= collision.gameObject.GetComponent<WeaponScript>().Damage();

            if (health <= 0)
            {
                Explode();
            }

            int rndEffect = Random.Range(0, EffectReferences.Instance.hitEffects.Length);
            GameObject hitEffect = Instantiate(EffectReferences.Instance.hitEffects[rndEffect], gameObject.transform.position, EffectReferences.Instance.hitEffects[rndEffect].transform.rotation);
            Destroy(hitEffect, 1.0f);
        }

        if (collision.gameObject.layer == Layer.Platform)
        {
            if (collision.gameObject.tag == "Wall")
            {
                rgbd.velocity = new Vector2(-currVelocity.x, currVelocity.y);
                currVelocity = rgbd.velocity;
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                flip *= -1;
            }
            else
            {
                rgbd.velocity = new Vector2(currVelocity.x, -currVelocity.y);
                currVelocity = rgbd.velocity;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == Layer.Weapon_Veg || collision.gameObject.layer == Layer.Weapon_Meat)
        {
            health -= collision.gameObject.GetComponent<WeaponScript>().Damage();

            if (health <= 0)
            {
                Explode();
            }

            int rndEffect = Random.Range(0, EffectReferences.Instance.hitEffects.Length);
            GameObject hitEffect = Instantiate(EffectReferences.Instance.hitEffects[rndEffect], gameObject.transform.position, EffectReferences.Instance.hitEffects[rndEffect].transform.rotation);
            Destroy(hitEffect, 1.0f);
        }
    }
}

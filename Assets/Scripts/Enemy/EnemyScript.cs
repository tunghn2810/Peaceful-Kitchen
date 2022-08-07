using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    //References
    private Animator anim;
    private Rigidbody2D rgbd;

    //Stats
    private float moveSpeed = 5f;
    private float currVelocity;
    private float flip;
    [SerializeField] private float health = 30f;

    //Bool checks
    private bool isDead = false;
    private bool isJump = false;
    private float jumpSpeed = 50f;

    private int hitCols;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        rgbd = GetComponent<Rigidbody2D>();
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
        if (GameStateScript.Instance.startGame)
        {
            if (!isDead)
            {
                rgbd.velocity = new Vector2(currVelocity, rgbd.velocity.y);
            }
            else
            {
                //Stop moving horizontally when it dies
                rgbd.velocity = new Vector2(0, rgbd.velocity.y);
            }
        }
        else
        {
            rgbd.velocity = new Vector2(0, 0);
        }

        //Jump when prompted to
        if (isJump)
        {
            rgbd.AddForce(Vector3.up * jumpSpeed, ForceMode2D.Impulse);
            isJump = false;
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    private void Explode()
    {
        //Instantiate(EffectReferences.Instance.enemyDie, gameObject.transform.position, Quaternion.identity);
        isDead = true;
        anim.SetBool("isDead", isDead);
        gameObject.layer = Layer.NoCol;
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
                rgbd.velocity = new Vector2(-currVelocity, rgbd.velocity.y);
                currVelocity = rgbd.velocity.x;
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hitCols <= 0)
        {
            if (collision.gameObject.layer == Layer.Weapon_Veg || collision.gameObject.layer == Layer.Weapon_Meat)
            {
                health -= collision.gameObject.GetComponent<WeaponScript>().Damage();

                if (health <= 0)
                {
                    Explode();
                }

                hitCols++;

                int rndEffect = Random.Range(0, EffectReferences.Instance.hitEffects.Length);
                GameObject hitEffect = Instantiate(EffectReferences.Instance.hitEffects[rndEffect], gameObject.transform.position, EffectReferences.Instance.hitEffects[rndEffect].transform.rotation);
                Destroy(hitEffect, 1.0f);
            }
        }

        if (collision.gameObject.layer == Layer.JumpPad)
        {
            isJump = true;
        }
        
        if (collision.gameObject.layer == Layer.Fridge)
        {
            if (gameObject.layer == Layer.Enemy_Veg && collision.gameObject.tag == "MeatFridge")
            {
                GameObject.FindGameObjectWithTag("MeatFridge").GetComponent<FridgeScript>().TakeDamage();
            }
            else if (gameObject.layer == Layer.Enemy_Meat && collision.gameObject.tag == "VegFridge")
            {
                GameObject.FindGameObjectWithTag("VegFridge").GetComponent<FridgeScript>().TakeDamage();
            }

            int rndEffect = Random.Range(0, EffectReferences.Instance.fridgeHitEffects.Length);
            GameObject fridgeHitEffect = Instantiate(EffectReferences.Instance.fridgeHitEffects[rndEffect], gameObject.transform.position, EffectReferences.Instance.fridgeHitEffects[rndEffect].transform.rotation);
            Destroy(fridgeHitEffect, 1.0f);

            Explode();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (hitCols > 0)
        {
            if (collision.gameObject.layer == Layer.Weapon_Veg || collision.gameObject.layer == Layer.Weapon_Meat)
            {
                hitCols--;
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FridgeScript : MonoBehaviour
{
    public GameObject dieEffect;
    private Animator anim;

    private float health = 30f;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void TakeDamage()
    {
        if (health > 0)
        {
            health -= 10f;
        }
        else
        {
            CameraScript.Instance.target = gameObject.transform;
            ControlsManager.Instance.currentCharacter.SetActive(false);

            //Spawn the particle effect
            GameObject particle = Instantiate(dieEffect, gameObject.transform.position, dieEffect.transform.rotation);
            Destroy(particle, 3.0f);

            anim.SetBool("isBroken", true);
        }
    }

    public void RestartFridge()
    {
        anim.SetBool("isBroken", false);
        health = 30;
    }
}

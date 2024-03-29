using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FridgeScript : MonoBehaviour
{
    private Animator anim;
    private float health = 30f;
    private int layer;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        layer = gameObject.layer;
    }

    public void TakeDamage()
    {
        if (health > 0)
        {
            health -= 10f;
        }

        if (health <= 0)
        {
            CameraScript.Instance.ChangeTarget(gameObject.transform);
            ControlsManager.Instance.currentCharacter.SetActive(false);

            //Spawn the particle effect
            GameObject particle = Instantiate(EffectReferences.Instance.fridgeExplode, gameObject.transform.position, EffectReferences.Instance.fridgeExplode.transform.rotation);
            Destroy(particle, 3.0f);

            //No more collision
            gameObject.layer = Layer.NoCol;

            //End screen
            GameStateScript.Instance.EndScreen();

            anim.SetBool("isBroken", true);
        }
    }

    public void RestartFridge()
    {
        anim.SetBool("isBroken", false);
        health = 30;
        gameObject.layer = layer;
    }
}

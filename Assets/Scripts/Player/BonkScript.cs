using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonkScript : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == Layer.Enemy_Veg || collision.gameObject.layer == Layer.Enemy_Meat) //6 is Enemy layer
        {
            //TODO: Particle effects
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SugarScript : MonoBehaviour
{
    //References
    public GameObject transformEffect;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //When the player touches a cube
        if (collision.gameObject.layer == Layer.Player || collision.gameObject.layer == Layer.Invincible)
        {
            //Spawn the particle effect
            GameObject particle = Instantiate(transformEffect, gameObject.transform.position, transformEffect.transform.rotation);
            Destroy(particle, 3.0f);

            //Transform the player
            ControlsManager.Instance.Transformation();

            //Switch control - Currently not needed
            ControlsManager.Instance.ControlInit();

            //Create a new cube
            SugarSpawnScript.Instance.SpawnCube();

            //Increase score by 1
            ScoreScript.Instance.score++;

            //Destroy the old cube
            Destroy(gameObject);
        }
    }
}

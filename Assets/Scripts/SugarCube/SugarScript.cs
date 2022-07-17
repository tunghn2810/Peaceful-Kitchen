using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SugarScript : MonoBehaviour
{
    //For singleton
    private ControlsManager controlsManager;

    //References
    public GameObject transformEffect;

    private void Awake()
    {
        //For singleton
        controlsManager = ControlsManager.Instance;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //When the player touches a cube
        if (collision.gameObject.layer == Layer.Player)
        {
            //Run the particle effect
            GameObject particle = Instantiate(transformEffect, gameObject.transform.position, transformEffect.transform.rotation);
            Destroy(particle, 3.0f);

            //Transform the player
            controlsManager.Transformation();

            //Switch control - Currently not needed
            controlsManager.ControlInit();

            //Create a new cube
            Debug.Log(collision.gameObject.name);
            SugarSpawnScript.Instance.SpawnCube();

            //Destroy the old cube
            Destroy(gameObject);
        }
    }
}

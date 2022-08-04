using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SugarScript : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //When the player touches a cube
        if (collision.gameObject.layer == Layer.Player_Veg || collision.gameObject.layer == Layer.Player_Meat || collision.gameObject.layer == Layer.Invincible)
        {
            //Spawn the particle effect
            GameObject particle = Instantiate(EffectReferences.Instance.playerTransform, gameObject.transform.position, EffectReferences.Instance.playerTransform.transform.rotation);
            Destroy(particle, 3.0f);

            //Transform the player
            ControlsManager.Instance.Transformation();

            //Reset variables
            

            //Switch control - Currently not needed
            ControlsManager.Instance.ControlInit();

            //Create a new cube
            SugarSpawnScript.Instance.SpawnCube();

            //Increase score by 1
            ScoreScript.Instance.score++;

            //Change side
            BorderScript.Instance.ChangeBorder();

            //Change pointer target
            SugarPointer.Instance.ChangeTarget();

            //Destroy the old cube
            Destroy(gameObject);
        }
    }
}

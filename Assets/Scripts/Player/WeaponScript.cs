using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponScript : MonoBehaviour
{
    private string type;

    private void Start()
    {
        type = gameObject.tag;
        ChangeLayer();
    }

    public float Damage()
    {
        if (type == "Toast")
        {
            return 15f;
        }
        else if (type == "Ladle")
        {
            return 30f;
        }
        else if (type == "Board")
        {
            return 100f;
        }
        else if (type == "RiceBall")
        {
            return 100f;
        }
        //ADD NEW WEAPON HERE
        return 0; //Never happens
    }

    public void ChangeLayer()
    {
        if (ControlsManager.Instance.currentLayer == Layer.Player_Veg)
        {
            gameObject.layer = Layer.Weapon_Veg;
        }
        else
        {
            gameObject.layer = Layer.Weapon_Meat;
        }

        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            if (gameObject.transform.GetChild(i).gameObject.layer != Layer.GroundCheck)
            {
                gameObject.transform.GetChild(i).gameObject.layer = gameObject.layer;
            }
        }
    }

    public void SpawnEffect(Vector3 spawnPos)
    {
        //Spawn the particle effect
        //GameObject particle = Instantiate(transformEffect, spawnPos, transformEffect.transform.rotation);
        //Destroy(particle, 1.0f);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponScript : MonoBehaviour
{
    private string type;

    private void Awake()
    {
        type = gameObject.tag;
    }

    public float Damage()
    {
        if (type == "Toast")
        {
            return 10f;
        }
        else if (type == "Ladle")
        {
            return 15f;
        }
        else if (type == "Board")
        {
            return 30f;
        }
        return 0; //Never happens
    }
}

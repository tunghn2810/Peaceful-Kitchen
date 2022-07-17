using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToasterScript : MonoBehaviour
{
    //Projectile start positions
    public GameObject toastPos;
    private Transform[] shootPos;

    //Projectile prefabs
    public GameObject toast;

    private void Awake()
    {
        shootPos = toastPos.GetComponentsInChildren<Transform>();
    }

    private void ShootToasts()
    {
        for (int i = 1; i < shootPos.Length; i++)
        {
            GameObject newToast = Instantiate(toast, shootPos[i].position, Quaternion.identity);
        }
    }
}

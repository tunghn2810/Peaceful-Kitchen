using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToasterScript : MonoBehaviour
{
    //Projectile start positions
    public GameObject toastPos;
    private Transform[] shootPos;
    private PlayerControl playerControl;

    //Projectile prefabs
    public GameObject toast;

    private void Awake()
    {
        shootPos = toastPos.GetComponentsInChildren<Transform>();
        playerControl = GetComponent<PlayerControl>();
        playerControl.moveSpeed = 10;
    }

    private void ShootToasts()
    {
        if (GetComponentInParent<PlayerControl>().isCurrent)
        {
            for (int i = 1; i < shootPos.Length; i++)
            {
                GameObject newToast = Instantiate(toast, shootPos[i].position, Quaternion.identity);
            }
        }
    }
}

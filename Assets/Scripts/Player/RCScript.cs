using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RCScript : MonoBehaviour
{
    //Projectile start positions
    public Transform ricePos;
    private PlayerControl playerControl;

    //Projectile prefabs
    public GameObject rice;

    private void Awake()
    {
        playerControl = GetComponent<PlayerControl>();
        playerControl.moveSpeed = 10;
    }

    private void SpawnRice()
    {
        if (GetComponentInParent<PlayerControl>().isCurrent)
        {
            GameObject newRice = Instantiate(rice, ricePos.position, Quaternion.identity);
        }
    }
}

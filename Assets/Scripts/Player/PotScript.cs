using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotScript : MonoBehaviour
{
    private GameObject hitBox;
    private PlayerControl playerControl;

    private void Awake()
    {
        hitBox = transform.GetChild(2).gameObject;
        playerControl = GetComponent<PlayerControl>();
    }

    private void Bonk()
    {
        if (playerControl.isCurrent)
        {
            hitBox.SetActive(true);
        }
    }

    public void BonkEnd()
    {
        hitBox.SetActive(false);
    }
}

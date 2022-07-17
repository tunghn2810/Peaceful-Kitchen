using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotScript : MonoBehaviour
{
    private GameObject hitBox;

    private void Awake()
    {
        hitBox = transform.GetChild(2).gameObject;
    }

    private void Bonk()
    {
        hitBox.SetActive(true);
    }

    private void BonkEnd()
    {
        hitBox.SetActive(false);
    }
}

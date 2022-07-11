using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public GameObject cameraPos;

    private void Start()
    {

    }

    private void Update()
    {
        gameObject.transform.position = cameraPos.transform.position;
    }
}

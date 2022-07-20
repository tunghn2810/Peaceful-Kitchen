using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    //Singleton
    public static CameraScript Instance { get; set; }

    //References
    public Transform target;
    private Vector3 offset;

    public float smoothSpeed = 0.01f;

    private void Awake()
    {
        //Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        offset = new Vector3(0, 5.0f, 0);
    }

    private void LateUpdate()
    {
        SmoothFollow();
    }

    //Smoothly follow the player
    private void SmoothFollow()
    {
        if (target != null)
        {
            Vector3 targetPos = target.position + offset;

            //Vector3 smoothFollow = Vector3.Lerp(transform.position, targetPos, smoothSpeed);
            //transform.position = smoothFollow;
            transform.position = targetPos;
        }
    }
}

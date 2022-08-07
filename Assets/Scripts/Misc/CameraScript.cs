using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraScript : MonoBehaviour
{
    //Singleton
    public static CameraScript Instance { get; set; }

    //References
    public Transform target;
    private CinemachineVirtualCamera cinemachine;

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
        //target = ControlsManager.Instance.currentCharacter.transform;
        cinemachine = GetComponent<CinemachineVirtualCamera>();

        ChangeTarget();
    }

    public void ChangeTarget()
    {
        cinemachine.LookAt = target;
        cinemachine.Follow = target;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraScript : MonoBehaviour
{
    //Singleton
    public static CameraScript Instance { get; set; }

    //References
    public Transform camTarget;
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
        camTarget = ControlsManager.Instance.currentCharacter.transform;
        cinemachine = GetComponent<CinemachineVirtualCamera>();

        ChangeTarget(camTarget);
    }

    public void ChangeTarget(Transform target)
    {
        cinemachine.LookAt = target;
        cinemachine.Follow = target;
    }
}

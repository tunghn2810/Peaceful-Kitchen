using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlsManager : MonoBehaviour
{
    //Singleton
    public static ControlsManager Instance { get; set; }

    public GameObject OnScreenControls;

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

    public void SwipeControl()
    {
        InputManager.Instance.Enable();
        OnScreenControls.SetActive(false);
    }

    public void BasicControl()
    {
        InputManager.Instance.Disable();
        OnScreenControls.SetActive(true);
    }
}

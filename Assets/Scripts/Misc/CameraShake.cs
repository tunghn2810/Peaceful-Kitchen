using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    //Singleton
    public static CameraShake Instance { get; set; }

    private CinemachineVirtualCamera cVCam;

    private float intensity = 5f;
    private float time = 0.2f;
    private float shakeTimer;
    private float shakeTimerTotal;
    private float startIntensity;

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

        cVCam = GetComponent<CinemachineVirtualCamera>();
    }

    private void Update()
    {
        if (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;
            if (shakeTimer <= 0f)
            {
                CinemachineBasicMultiChannelPerlin multiPerlin = cVCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

                multiPerlin.m_AmplitudeGain = Mathf.Lerp(startIntensity, 0f, 1 - (shakeTimer / shakeTimerTotal));
            }
        }
    }

    public void ShakeCamera()
    {
        CinemachineBasicMultiChannelPerlin multiPerlin = cVCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        multiPerlin.m_AmplitudeGain = intensity;

        startIntensity = intensity;
        shakeTimerTotal = time;
        shakeTimer = time;
    }
}

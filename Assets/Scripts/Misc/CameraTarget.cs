using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTarget : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position = ControlsManager.Instance.currentCharacter.transform.position + new Vector3(0, 1f, 0);
    }
}

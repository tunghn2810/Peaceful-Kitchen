using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlSwitch : MonoBehaviour
{
    public Image controlType;

    public Sprite gamepadControl;
    public Sprite touchControl;

    public void Switch()
    {
        if (ControlsManager.Instance.currentMode == 0)
        {
            ControlsManager.Instance.currentMode = 1;
            controlType.sprite = gamepadControl;
            controlType.SetNativeSize();
        }
        else if (ControlsManager.Instance.currentMode == 1)
        {
            ControlsManager.Instance.currentMode = 0;
            controlType.sprite = touchControl;
            controlType.SetNativeSize();
        }
    }
}

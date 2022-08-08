using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialScript : MonoBehaviour
{
    public void TutorialClosed()
    {
        if (ControlsManager.Instance.currentMode == 0)
        {
            ControlsManager.Instance.SwipeControl();
            GameStateScript.Instance.controlCanvas.SetActive(false);
            GameStateScript.Instance.pauseButtonCanvas.SetActive(true);
        }
        else if (ControlsManager.Instance.currentMode == 1)
        {
            ControlsManager.Instance.BasicControl();
            GameStateScript.Instance.controlCanvas.SetActive(true);
            GameStateScript.Instance.pauseButtonCanvas.SetActive(true);
        }

        gameObject.transform.parent.gameObject.SetActive(false);
    }
}

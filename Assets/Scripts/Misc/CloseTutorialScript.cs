using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseTutorialScript : MonoBehaviour
{
    public void CloseTutorial()
    {
        gameObject.transform.parent.gameObject.SetActive(false);

        GameStateScript.Instance.controlCanvas.SetActive(true);
        ControlsManager.Instance.BasicControl();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayFPS : MonoBehaviour
{
    float deltaTime = 0.0f;

    private void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
    }

    private void OnGUI()
    {
        int w = Screen.width, h = Screen.height;

        GUIStyle style = new GUIStyle();
        style.alignment = TextAnchor.UpperLeft;
        style.fontSize = h * 2 / 40;
        style.normal.textColor = new Color(0.2f, 1.0f, 0.3f, 1.0f);

        Rect rect = new Rect(50, 50, w, h * 2 / 100);
        float msec = deltaTime * 1000.0f;
        float fps = 1.0f / deltaTime;
        string text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
        GUI.Label(rect, text, style);

        Rect rect2 = new Rect(50, 120, w, h * 2 / 100);
        string text2 = DisplayTime(GameStateScript.Instance.timer);
        GUI.Label(rect2, text2, style);
    }

    private string DisplayTime(float time)
    {
        float minutes = Mathf.FloorToInt(time / 60);
        float seconds = Mathf.FloorToInt(time % 60);

        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}

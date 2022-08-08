using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsAnimScript : MonoBehaviour
{
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void SettingsOpened()
    {
        anim.SetBool("isFromMain", false);
    }

    public void SettingsClosed()
    {
        gameObject.GetComponent<RectTransform>().localScale = Vector3.one;
        gameObject.transform.parent.gameObject.SetActive(false);
    }
}

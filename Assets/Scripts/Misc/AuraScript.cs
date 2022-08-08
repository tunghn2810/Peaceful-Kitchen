using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuraScript : MonoBehaviour
{
    private Animator anim;
    private bool isMeat;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        StartAura();
    }

    public void ChangeAura()
    {
        isMeat = isMeat == true ? false : true;
        anim.SetBool("isMeat", isMeat);
    }

    public void StartAura()
    {
        isMeat = ControlsManager.Instance.currentLayer == Layer.Player_Meat ? true : false;
        anim.SetBool("isMeat", isMeat);
    }
}

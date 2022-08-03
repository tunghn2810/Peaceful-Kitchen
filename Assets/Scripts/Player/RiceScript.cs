using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiceScript : MonoBehaviour
{
    private GameObject groundCheck;
    private GameObject hitBox;

    private Animator anim;
    private Rigidbody2D rgbd;

    private PlayerControl playerControl;

    //Bool checks
    private float flip;
    bool isExplode;

    private void Awake()
    {
        groundCheck = transform.GetChild(0).gameObject;
        hitBox = transform.GetChild(1).gameObject;

        anim = GetComponent<Animator>();
        rgbd = GetComponent<Rigidbody2D>();
        playerControl = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>();

        flip = playerControl.flip;
        gameObject.transform.localScale = new Vector3(transform.localScale.x * flip, transform.localScale.y, transform.localScale.z);
    }

    private void Disappear()
    {
        Destroy(gameObject);
    }

    public void Explode()
    {
        isExplode = true;
        anim.SetBool("isExplode", isExplode);
        hitBox.SetActive(true);
        rgbd.constraints = RigidbodyConstraints2D.FreezeAll;
        gameObject.layer = Layer.NoCol;
        CameraShake.Instance.ShakeCamera();
    }
}

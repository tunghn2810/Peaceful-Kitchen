using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericButtonScript : MonoBehaviour
{
    public void GenericClosed()
    {
        gameObject.GetComponent<RectTransform>().localScale = Vector3.one;
        gameObject.transform.parent.gameObject.SetActive(false);
    }
}

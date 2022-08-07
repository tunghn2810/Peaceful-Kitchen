using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SugarPointer : MonoBehaviour
{
    //Singleton
    public static SugarPointer Instance { get; set; }

    private Transform targetPos;
    public RectTransform pointerMoveRect;
    public RectTransform pointerRotateRect;
    public RectTransform pointerIcon;
    private Camera mainCamera;

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
    }

    private void Start()
    {
        mainCamera = Camera.main;
        ChangeTarget();
    }

    private void LateUpdate()
    {
        if (targetPos != null)
        {
            Vector3 toPos = targetPos.position;
            Vector3 fromPos = mainCamera.transform.position;
            fromPos.z = -10;
            Vector3 direction = (toPos - fromPos).normalized;
            float distanceScaled = (toPos - fromPos).magnitude * 0.05f;
            float scaleClamped = Mathf.Clamp(distanceScaled, 0, 1f);

            float angle = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg) % 360;
            pointerRotateRect.localEulerAngles = new Vector3(0, 0, angle + 90);

            pointerMoveRect.localScale = new Vector3(scaleClamped, scaleClamped, pointerMoveRect.localScale.z);
            //pointerIcon.localScale = new Vector3(scaleClamped, scaleClamped, pointerIcon.localScale.z);

            float borderOffset = 200f * scaleClamped;

            Vector3 targetPosScreenPoint = mainCamera.WorldToScreenPoint(toPos);
            bool isOffScreen = targetPosScreenPoint.x <= borderOffset || targetPosScreenPoint.x >= Screen.width - borderOffset || targetPosScreenPoint.y <= borderOffset || targetPosScreenPoint.y >= Screen.height - borderOffset;

            if (isOffScreen)
            {
                pointerMoveRect.gameObject.SetActive(true);
                pointerIcon.gameObject.SetActive(true);

                Vector3 cappedTargetScreenPos = targetPosScreenPoint;
                cappedTargetScreenPos.x = Mathf.Clamp(cappedTargetScreenPos.x, borderOffset, Screen.width - borderOffset);
                cappedTargetScreenPos.y = Mathf.Clamp(cappedTargetScreenPos.y, borderOffset, Screen.height - borderOffset);

                pointerMoveRect.position = cappedTargetScreenPos;
                pointerMoveRect.localPosition = new Vector3(pointerMoveRect.localPosition.x, pointerMoveRect.localPosition.y, -10f);

                pointerIcon.position = new Vector3(pointerRotateRect.position.x, pointerRotateRect.position.y, pointerRotateRect.position.z);
            }
            else
            {
                //pointerRectTransform.position = targetPosScreenPoint;
                //pointerRectTransform.localPosition = new Vector3(pointerRectTransform.localPosition.x, pointerRectTransform.localPosition.y, -10f);
                //
                //pointerIcon.position = new Vector3(pointerRectTransform.position.x, pointerRectTransform.position.y - 30, pointerRectTransform.position.z);
                pointerMoveRect.gameObject.SetActive(false);
                pointerIcon.gameObject.SetActive(false);
            }
        }
        else
        {
            ChangeTarget();
        }
    }

    public void ChangeTarget()
    {
        targetPos = GameObject.FindGameObjectWithTag("SugarCube").transform;
    }
}

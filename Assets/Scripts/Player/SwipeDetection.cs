using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeDetection : MonoBehaviour
{
    //Limits for swiping
    [SerializeField]
    private float minDistance = 0.2f;
    [SerializeField]
    private float maxTime = 1f;
    [SerializeField]
    private float directionThreshold = 0.9f;

    private Vector2 startPosition;
    private float startTime;
    private Vector2 endPosition;
    private float endTime;

    private PlayerControl playerControl;

    private void Awake()
    {
        playerControl = GetComponent<PlayerControl>();
    }

    public void SwipeModeOn()
    {
        InputManager.Instance.OnStartTouch += SwipeStart;
        InputManager.Instance.OnEndTouch += SwipeEnd;
        InputManager.Instance.OnTapHold += TapAndHold;
        InputManager.Instance.OnHoldRelease += HoldRelease;
        InputManager.Instance.OnTap += Tap;
        InputManager.Instance.OnJumpTap += JumpTap;
    }

    public void SwipeModeOff()
    {
        InputManager.Instance.OnStartTouch -= SwipeStart;
        InputManager.Instance.OnEndTouch -= SwipeEnd;
        InputManager.Instance.OnTapHold -= TapAndHold;
        InputManager.Instance.OnTap -= Tap;
        InputManager.Instance.OnJumpTap -= JumpTap;
    }

    private void SwipeStart(Vector2 position, float time)
    {
        startPosition = position;
        startTime = time;
    }

    private void SwipeEnd(Vector2 position, float time)
    {
        endPosition = position;
        endTime = time;

        DetectSwipe();
    }

    private void DetectSwipe()
    {
        if (Vector3.Distance(startPosition, endPosition) >= minDistance &&
            (endTime - startTime) <= maxTime)
        {
            Debug.DrawLine(startPosition, endPosition, Color.red, 5f); //Draw a line in the scene view in the Editor

            Vector3 direction = endPosition - startPosition;
            Vector2 direction2D = new Vector2(direction.x, direction.y).normalized;
            SwipeDirection(direction2D);
        }
    }

    private void SwipeDirection(Vector2 direction)
    {
        if (Vector2.Dot(Vector2.up, direction) > directionThreshold)
        {
            //playerControl.JumpSwipe();
        }
        if (Vector2.Dot(Vector2.down, direction) > directionThreshold)
        {
            //Debug.Log("Down");
        }
        if (Vector2.Dot(Vector2.left, direction) > directionThreshold)
        {
            //playerControl.SlideAttack(false);
        }
        if (Vector2.Dot(Vector2.right, direction) > directionThreshold)
        {
            //playerControl.SlideAttack(true);
        }
    }

    public void TapAndHold(Vector2 position)
    {
        if (position.x < Screen.width/2)
        {
            playerControl.MoveStart(Vector2.left);
        }
        else
        {
            playerControl.MoveStart(Vector2.right);
        }
    }

    public void HoldRelease()
    {
        playerControl.MoveEnd();
    }

    public void Tap(Vector2 position)
    {
        if (position.x < Screen.width / 2)
        {
            playerControl.TapAttack(false);
        }
        else
        {
            playerControl.TapAttack(true);
        }
    }

    public void JumpTap(bool isHeld)
    {
        playerControl.Jump(isHeld);
    }
}

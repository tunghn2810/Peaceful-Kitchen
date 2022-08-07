using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(-1)]
public class InputManager : MonoBehaviour
{
    //Singleton
    public static InputManager Instance { get; set; }

    private PlayerInputActions playerInputActions;
    private Camera mainCamera;

    //Swipe
    public delegate void StartTouch(Vector2 position, float time);
    public event StartTouch OnStartTouch;
    public delegate void EndTouch(Vector2 position, float time);
    public event EndTouch OnEndTouch;

    //Tap
    public delegate void Tap(Vector2 position);
    public event Tap OnTap;
    public delegate void JumpTap(bool isHeld);
    public event JumpTap OnJumpTap;

    private bool isSecondTouch = false;

    //Tap and hold
    public delegate void TapHold(Vector2 position);
    public event TapHold OnTapHold;
    public delegate void TapHoldRelease();
    public event TapHoldRelease OnHoldRelease;

    //Keyboard + Gamepad
    public delegate void MoveButtonStart(Vector2 position);
    public event MoveButtonStart OnMoveStart;
    public delegate void MoveButtonEnd();
    public event MoveButtonEnd OnMoveEnd;
    public delegate void AttackButton();
    public event AttackButton OnAttack;
    public delegate void JumpButton(bool isHeld);
    public event JumpButton OnJump;

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

        Application.targetFrameRate = 120;

        playerInputActions = new PlayerInputActions();
        mainCamera = Camera.main;
    }

    private void OnEnable()
    {
        playerInputActions.Enable();
    }

    private void OnDisable()
    {
        playerInputActions.Disable();
    }

    private void Start()
    {
        //Swipe
        playerInputActions.Player_Swipe.PrimaryContact.started += ctx => StartTouchPrimary(ctx);
        playerInputActions.Player_Swipe.PrimaryContact.canceled += ctx => EndTouchPrimary(ctx);

        //Tap
        playerInputActions.Player_Swipe.Tap.performed += ctx => TapPrimary(ctx);
        playerInputActions.Player_Swipe.JumpTap.performed += ctx => JumpTapPrimary(ctx);
        playerInputActions.Player_Swipe.SecondContact.performed += ctx => SeconTouchPrimary(ctx);

        //Tap and hold
        playerInputActions.Player_Swipe.TapHold.performed += ctx => TapHoldPrimary(ctx);
        playerInputActions.Player_Swipe.TapHold.canceled += ctx => HoldRelease();

        //Keyboard + GamePad
        playerInputActions.Player_Basic.Move.performed += ctx => MoveStart(ctx);
        playerInputActions.Player_Basic.Move.canceled += ctx => MoveEnd(ctx);
        playerInputActions.Player_Basic.Attack.performed += ctx => Attack(ctx);
        playerInputActions.Player_Basic.Jump.performed += ctx => Jump(ctx);

        //Reset
        playerInputActions.Player_Basic.Reset.performed += ctx => ResetButton(ctx);
    }

    bool isTouching = false;

    private void Update()
    {
        if (isTouching)
        {
            //Debug.Log(PrimaryPosition());
        }
    }

    private void StartTouchPrimary(InputAction.CallbackContext context)
    {
        isTouching = true;
        //if (OnStartTouch != null)
        //{
        //    OnStartTouch(ScreenToWorld(mainCamera, playerInputActions.Player_Swipe.PrimaryPosition.ReadValue<Vector2>()), (float)//context.startTime);
        //}

        StartCoroutine(WaitStartTouch(context));
    }

    //Work around for a bug that makes the start position always at 0,0 the first time
    private IEnumerator WaitStartTouch(InputAction.CallbackContext context)
    {
        yield return new WaitForEndOfFrame();

        Vector2 position = playerInputActions.Player_Swipe.PrimaryPosition.ReadValue<Vector2>();
        if (position.x == 0 && position.y == 0)
        {
            yield return null;
        }

        if (OnStartTouch != null)
        {
            OnStartTouch(PrimaryPosition(), (float)context.startTime);
        }
    }

    private void EndTouchPrimary(InputAction.CallbackContext context)
    {
        isTouching = false;
        if (OnEndTouch != null)
        {
            OnEndTouch(PrimaryPosition(), (float)context.time);
        }
    }

    //Supplement function to calculate the position that is pressed on the screen
    public Vector2 PrimaryPosition()
    {
        return ScreenToWorld(mainCamera, playerInputActions.Player_Swipe.PrimaryPosition.ReadValue<Vector2>());
    }

    //Supplement function to calculate the ScreenToWorld position through the camera
    private Vector3 ScreenToWorld(Camera camera, Vector3 position)
    {
        position.z = camera.nearClipPlane;
        return camera.ScreenToWorldPoint(position);
    }

    private void TapPrimary(InputAction.CallbackContext context)
    {
        if (OnTap != null)
        {
            OnTap(playerInputActions.Player_Swipe.PrimaryPosition.ReadValue<Vector2>());
        }
    }

    private void JumpTapPrimary(InputAction.CallbackContext context)
    {
        if (OnJumpTap != null)
        {
            OnJumpTap(DoJump());
        }
    }

    private void SeconTouchPrimary(InputAction.CallbackContext context)
    {
         isSecondTouch = context.ReadValueAsButton();
    }

    private bool DoJump()
    {
        if (isSecondTouch)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void TapHoldPrimary(InputAction.CallbackContext context)
    {
        if (OnTapHold != null)
        {
            OnTapHold(playerInputActions.Player_Swipe.PrimaryPosition.ReadValue<Vector2>());
        }
    }

    private void HoldRelease()
    {
        if (OnHoldRelease != null)
        {
            OnHoldRelease();
        }
    }

    private void MoveStart(InputAction.CallbackContext context)
    {
        if (OnMoveStart != null)
        {
            OnMoveStart(playerInputActions.Player_Basic.Move.ReadValue<Vector2>());
        }
    }

    private void MoveEnd(InputAction.CallbackContext context)
    {
        if (OnMoveEnd != null)
        {
            OnMoveEnd();
        }
    }

    private void Attack(InputAction.CallbackContext context)
    {
        if (OnAttack != null)
        {
            OnAttack();
        }
    }

    private void Jump(InputAction.CallbackContext context)
    {
        if (OnJump != null)
        {
            OnJump(context.ReadValueAsButton());
        }
    }

    //Reset function
    public void ResetButton(InputAction.CallbackContext context)
    {
        RestartScript.Instance.RestartScore();
        RestartScript.Instance.RestartPlayer();
        RestartScript.Instance.RestartFridge();
    }
}

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

        //Application.targetFrameRate = 120;

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

        //Keyboard + GamePad
        playerInputActions.Player_Basic.Move.started += ctx => MoveStart(ctx);
        playerInputActions.Player_Basic.Move.canceled += ctx => MoveEnd(ctx);
        playerInputActions.Player_Basic.Attack.performed += ctx => Attack(ctx);
        playerInputActions.Player_Basic.Jump.performed += ctx => Jump(ctx);
    }

    private void StartTouchPrimary(InputAction.CallbackContext context)
    {
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
            OnTap(PrimaryPosition());
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
}

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
    public delegate void Tap(Vector2 position, float time);
    public event Tap OnTap;

    //Jump for keyboard
    public delegate void JumpButton();
    public event JumpButton OnJumpButton;

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
        playerInputActions.Player_Swipe.PrimaryContact.started += ctx => StartTouchPrimary(ctx);
        playerInputActions.Player_Swipe.PrimaryContact.canceled += ctx => EndTouchPrimary(ctx);

        playerInputActions.Player.Jump.performed += ctx => Jump(ctx);

        playerInputActions.Player_Swipe.Tap.performed += ctx => TapPrimary(ctx);
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
            OnStartTouch(ScreenToWorld(mainCamera, playerInputActions.Player_Swipe.PrimaryPosition.ReadValue<Vector2>()), (float)context.startTime);
        }
    }

    private void EndTouchPrimary(InputAction.CallbackContext context)
    {
        if (OnEndTouch != null)
        {
            OnEndTouch(ScreenToWorld(mainCamera, playerInputActions.Player_Swipe.PrimaryPosition.ReadValue<Vector2>()), (float)context.time);
        }
    }

    public Vector2 PrimaryPosition()
    {
        return ScreenToWorld(mainCamera, playerInputActions.Player_Swipe.PrimaryPosition.ReadValue<Vector2>());
    }

    private Vector3 ScreenToWorld(Camera camera, Vector3 position)
    {
        position.z = camera.nearClipPlane;
        return camera.ScreenToWorldPoint(position);
    }

    private void Jump(InputAction.CallbackContext context)
    {
        if (OnJumpButton != null)
        {
            OnJumpButton();
        }
    }

    private void TapPrimary(InputAction.CallbackContext context)
    {
        if (OnTap != null)
        {
            OnTap(ScreenToWorld(mainCamera, playerInputActions.Player_Swipe.PrimaryPosition.ReadValue<Vector2>()), (float)context.startTime);
        }
    }
}

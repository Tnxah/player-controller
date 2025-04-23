using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerControlManager))]
public class PlayerRotation : MonoBehaviour
{
    private ICameraRotation cameraRotationStrategy;

    [Header("Rotation Settings")]
    [SerializeField] private float sensitivityX = 1f;
    [SerializeField] private float sensitivityY = 1f;

    [Header("Camera")]
    [SerializeField] private CameraFollow cameraFollow;

    private PlayerControls inputActions;
    private Vector2 lookInput;


    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void Initialize(PlayerControls inputActions)
    {
        this.inputActions = inputActions;

        SubscribeToInputActions();
    }

    private void LateUpdate()
    {
        if(cameraRotationStrategy != null)
            cameraRotationStrategy.Tick(lookInput);
    }

    private void SwitchMode(BaseCameraRotation cameraRotation)
    {
        print(cameraRotation.name);
        cameraRotationStrategy = cameraRotation;
    }

    private void OnRotatePerformed(InputAction.CallbackContext ctx) => lookInput = ctx.ReadValue<Vector2>() * new Vector2(sensitivityX, sensitivityY);
    private void OnRotateCanceled(InputAction.CallbackContext ctx) => lookInput = Vector2.zero;

    private void SubscribeToInputActions()
    {
        inputActions.Player.Rotate.performed += OnRotatePerformed;
        inputActions.Player.Rotate.canceled += OnRotateCanceled;
    }

    private void UnsubscribeFromInputActions()
    {
        inputActions.Player.Rotate.performed -= OnRotatePerformed;
        inputActions.Player.Rotate.canceled -= OnRotateCanceled;
    }

    private void OnEnable()
    {
        EventBus.Subscribe<BaseCameraRotation>(SwitchMode);
    }

    private void OnDisable()
    {
        UnsubscribeFromInputActions();
    }
}
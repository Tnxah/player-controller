using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.Rendering.STP;

[RequireComponent(typeof(PlayerControlManager))]
public class PlayerRotation : MonoBehaviour
{
    private ICameraRotation cameraRotationStrategy;

    [Header("Rotation Settings")]
    [SerializeField] private float sensitivityX = 1f;
    [SerializeField] private float sensitivityY = 1f;

    [Header("View Targets")]
    [SerializeField] private Transform firstPersonTargetPosition;
    [SerializeField] private Transform thirdPersonTargetPosition;
    [SerializeField] private Transform cameraTarget;

    [Header("Camera")]
    [SerializeField] private CameraFollow cameraFollow;


    [SerializeField] private RotationConfig config;
    private CameraRigReference rig;


    private PlayerControls inputActions;
    private Vector2 lookInput;


    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        rig.Player = transform;
        rig.Target = cameraTarget;
    }

    public void Initialize(PlayerControls inputActions)
    {
        this.inputActions = inputActions;

        SubscribeToInputActions();
        cameraFollow.SetFollowTarget(cameraTarget);
    }

    private void LateUpdate()
    {
        if(cameraRotationStrategy != null)
            cameraRotationStrategy.Tick(lookInput);
    }

    private void SwitchMode(ViewMode mode)
    {
        print(mode);
        Transform anchor = mode == ViewMode.FirstPerson ? firstPersonTargetPosition : thirdPersonTargetPosition;

        cameraRotationStrategy = mode switch
        {
            ViewMode.FirstPerson => new FirstPersonCameraRotation(rig, config, anchor),
            ViewMode.ThirdPerson => new ThirdPersonCameraRotation(rig, config, anchor),
            _ => throw new NotImplementedException()
        };
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
        EventBus.Subscribe<ViewMode>(SwitchMode);
    }

    private void OnDisable()
    {
        UnsubscribeFromInputActions();
    }
}
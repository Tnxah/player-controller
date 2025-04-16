using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerControlManager))]
public class PlayerRotation : MonoBehaviour
{
    private ICameraRotation cameraRotation;

    [Header("Rotation Settings")]
    [SerializeField] private float mouseSensitivity = 1f;

    [Header("View Targets")]
    [SerializeField] private Transform firstPersonTargetPosition;
    [SerializeField] private Transform thirdPersonTargetPosition;
    [SerializeField] private Transform cameraTarget;

    [Header("Camera")]
    [SerializeField] private CameraFollow cameraFollow;

    private CameraRotationContext context;
    private PlayerControls inputActions;
    private Vector2 lookInput;
    private float pitch;
    private float yaw;

    private bool isThirdPerson; // default to FP

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void Initialize(PlayerControls inputActions)
    {
        this.inputActions = inputActions;

        context = new CameraRotationContext(cameraTarget, transform, firstPersonTargetPosition, thirdPersonTargetPosition);

        SubscribeToInputActions();
        SetViewMode(isThirdPerson); // Set initial mode
        cameraFollow.SetFollowTarget(cameraTarget);
    }

    private void LateUpdate()
    {
        cameraRotation.ApplyRotation();
    }

    private void ToggleView()
    {
        isThirdPerson = !isThirdPerson;
        SetViewMode(isThirdPerson);
    }

    private void SetViewMode(bool thirdPerson)
    {
        cameraRotation = thirdPerson ? new ThirdPersonCameraRotation(context) : new FirstPersonCameraRotation(context);
    }

    private void OnRotatePerformed(InputAction.CallbackContext ctx) => cameraRotation.PassInput(ctx.ReadValue<Vector2>() * mouseSensitivity);
    private void OnRotateCanceled(InputAction.CallbackContext ctx) => cameraRotation.PassInput(Vector2.zero);

    private void SubscribeToInputActions()
    {
        inputActions.Player.Rotate.performed += OnRotatePerformed;
        inputActions.Player.Rotate.canceled += OnRotateCanceled;

        inputActions.Player.ToggleView.performed += ctx => ToggleView();
    }

    private void UnsubscribeFromInputActions()
    {
        inputActions.Player.Rotate.performed -= OnRotatePerformed;
        inputActions.Player.Rotate.canceled -= OnRotateCanceled;

        inputActions.Player.ToggleView.performed -= ctx => ToggleView();
    }

    private void OnDisable()
    {
        UnsubscribeFromInputActions();
    }
}
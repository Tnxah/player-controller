using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerControlManager))]
public class PlayerRotation : MonoBehaviour
{
    [Header("Rotation Settings")]
    [SerializeField] private float mouseSensitivity = 1f;
    [SerializeField] private float pitchClamp = 90f;

    [Header("View Targets")]
    [SerializeField] private Transform firstPersonTarget;
    [SerializeField] private Transform thirdPersonTarget;

    [Header("Camera")]
    [SerializeField] private CameraFollow cameraFollow;

    private PlayerControls inputActions;
    private Vector2 lookInput;
    private float pitch;
    private float yaw;

    private bool isThirdPerson; // default to FP

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        yaw = transform.eulerAngles.y;
        pitch = firstPersonTarget.localEulerAngles.x;
    }

    public void Initialize(PlayerControls inputActions)
    {
        this.inputActions = inputActions;

        SubscribeToInputActions();
        SetViewMode(isThirdPerson); // Set initial mode
    }

    private void LateUpdate()
    {
        ApplyRotation();
    }

    private void ApplyRotation()
    {
        yaw += lookInput.x * mouseSensitivity;
        pitch -= lookInput.y * mouseSensitivity;

        pitch = Mathf.Clamp(pitch, -pitchClamp, pitchClamp);

        // Apply player yaw rotation (Y-axis)
        transform.rotation = Quaternion.Euler(0f, yaw, 0f);

        // Apply camera pitch (X-axis)
        Transform currentTarget = isThirdPerson ? thirdPersonTarget : firstPersonTarget;
        currentTarget.localRotation = Quaternion.Euler(pitch, 0f, 0f);
    }

    private void ToggleView()
    {
        isThirdPerson = !isThirdPerson;
        SetViewMode(isThirdPerson);
    }

    private void SetViewMode(bool thirdPerson)
    {
        cameraFollow.SetFollowTarget(thirdPerson ? thirdPersonTarget : firstPersonTarget);
    }

    private void OnRotatePerformed(InputAction.CallbackContext ctx) => lookInput = ctx.ReadValue<Vector2>();
    private void OnRotateCanceled(InputAction.CallbackContext ctx) => lookInput = Vector2.zero;

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

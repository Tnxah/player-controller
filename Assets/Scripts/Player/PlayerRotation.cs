using UnityEngine;

[RequireComponent(typeof(PlayerControlManager))]
public class PlayerRotation : MonoBehaviour
{
    [Header("Rotation Settings")]
    [SerializeField] private float mouseSensitivity = 20f;
    [SerializeField] private float rotationSmoothTime = 0.05f;

    [Header("Camera")]
    [SerializeField] private Transform cameraPosition;

    private PlayerControls inputActions;
    private Vector2 lookInput;

    private float pitch;
    private float yaw;
    private float pitchVelocity;
    private float yawVelocity;

    public void Initialize(PlayerControls inputActions)
    {
        this.inputActions = inputActions;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        SubscribeToInputActions();
    }

    private void LateUpdate()
    {
        ApplyRotation();
    }

    private void ApplyRotation()
    {
        var mouseX = lookInput.x * mouseSensitivity;
        var mouseY = lookInput.y * mouseSensitivity;

        pitch -= mouseY * Time.deltaTime;
        yaw += mouseX * Time.deltaTime;

        pitch = Mathf.Clamp(pitch, -90f, 90f);

        float smoothedPitch = Mathf.SmoothDampAngle(
            cameraPosition.localEulerAngles.x,
            pitch,
            ref pitchVelocity,
            rotationSmoothTime
        );

        float smoothedYaw = Mathf.SmoothDampAngle(
            transform.eulerAngles.y,
            yaw,
            ref yawVelocity,
            rotationSmoothTime
        );

        cameraPosition.localRotation = Quaternion.Euler(smoothedPitch, 0f, 0f);
        transform.rotation = Quaternion.Euler(0f, smoothedYaw, 0f);
    }

    private void SubscribeToInputActions()
    {
        inputActions.Player.Rotate.performed += ctx => lookInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Rotate.canceled += ctx => lookInput = Vector2.zero;
    }

    private void UnsubscribeFromInputActions()
    {
        inputActions.Player.Rotate.performed -= ctx => lookInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Rotate.canceled -= ctx => lookInput = Vector2.zero;
    }

    private void OnDisable()
    {
        UnsubscribeFromInputActions();
    }
}

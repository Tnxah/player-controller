using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerControlManager))]
public class PlayerRotation : MonoBehaviour
{
    private ICameraRotation cameraRotationStrategy;

    [Header("Rotation Settings")]
    [SerializeField] private float sensitivityX = 1f;
    [SerializeField] private float sensitivityY = 1f;

    private PlayerControls inputActions;
    private Vector2 lookInput;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        EventBus.Subscribe<BaseCameraRotation>(SwitchMode);
    }

    public void Initialize(PlayerControls inputActions)
    {
        this.inputActions = inputActions;

        SubscribeToInputActions();
    }

    private void SwitchMode(BaseCameraRotation cameraRotation)
    {
        cameraRotationStrategy = cameraRotation;
    }

    private void OnRotatePerformed(InputAction.CallbackContext ctx) => cameraRotationStrategy?.OnInput(ctx.ReadValue<Vector2>() * new Vector2(sensitivityX, sensitivityY));
    private void OnRotateCanceled(InputAction.CallbackContext ctx) => cameraRotationStrategy?.OnInput(Vector2.zero);
    
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

    private void OnDisable()
    {
        UnsubscribeFromInputActions();
        EventBus.Unsubscribe<BaseCameraRotation>(SwitchMode);
    }
}
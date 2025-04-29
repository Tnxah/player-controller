using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public class PlayerControlManager : MonoBehaviour
{
    private InputDevice _currentDevice;

    private PlayerControls PlayerControls;

    [SerializeField] private PlayerRotation PlayerRotation;
    [SerializeField] private PlayerMovement PlayerMovement;
    [SerializeField] private PlayerInteraction PlayerInteraction;
    [SerializeField] private ViewModeState ViewModeState;

    private void Awake()
    {
        PlayerControls = new PlayerControls();

        PlayerMovement.Initialize(PlayerControls);
        PlayerRotation.Initialize(PlayerControls);
        PlayerInteraction.Initialize(PlayerControls);
        ViewModeState.Initialize(PlayerControls);
    }

    private void OnInputEvent(InputEventPtr eventPtr, InputDevice inputDevice)
    {
        if (!eventPtr.IsA<StateEvent>() && !eventPtr.IsA<DeltaStateEvent>() || _currentDevice == inputDevice)
            return;

        _currentDevice = inputDevice;
        EventBus.Publish(_currentDevice);
    }

    private void OnEnable()
    {
        PlayerControls.Enable();
        InputSystem.onEvent += OnInputEvent;
    }

    private void OnDisable()
    {
        PlayerControls.Disable();
        InputSystem.onEvent -= OnInputEvent;
    }
}

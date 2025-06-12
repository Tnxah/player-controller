using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public class PlayerControlManager : MonoBehaviour
{
    private InputDevice _currentDevice;

    private PlayerControls PlayerControls;

    private void Awake()
    {
        PlayerControls = new PlayerControls();

        InitailizePlayerComponents();
    }

    private void InitailizePlayerComponents()
    {
        var playerComponents = GetComponents<PlayerComponentBase>();
        
        foreach (var pc in playerComponents)
            pc.Initialize(PlayerControls);
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

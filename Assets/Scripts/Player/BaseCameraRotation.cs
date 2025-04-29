using UnityEngine;
using UnityEngine.InputSystem;

public abstract class BaseCameraRotation : MonoBehaviour, ICameraRotation
{
    [SerializeField] protected RotationConfig cfg;
    [SerializeField] public Transform Target;
    [SerializeField] protected Transform Player;

    protected InputDevice _currentDevice;

    public abstract void OnInput(Vector2 input);

    private void SetDevice(InputDevice device) 
    { 
        _currentDevice = device;
    }

    public virtual void Enter()
    {
        EventBus.Subscribe<InputDevice>(SetDevice, true);
        this.enabled = true;
        print("Enter");
    }

    public virtual void Exit()
    {
        EventBus.Unsubscribe<InputDevice>(SetDevice);
        this.enabled = false;
        print("Exit");
    }
}

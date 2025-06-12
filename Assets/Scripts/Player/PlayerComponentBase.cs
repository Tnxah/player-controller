using UnityEngine;

public abstract class PlayerComponentBase : MonoBehaviour
{
    protected PlayerControls inputActions;

    public void Initialize(PlayerControls input)
    {
        inputActions = input;
        SubscribeToInputActions();
    }

    protected abstract void SubscribeToInputActions();
    protected abstract void UnsubscribeFromInputActions();

    public virtual void OnDisable()
    {
        UnsubscribeFromInputActions();
    }
}

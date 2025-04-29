using UnityEngine;

public interface ICameraRotation
{
    public abstract void OnInput(Vector2 input);
    public abstract void Enter();
    public abstract void Exit();
}

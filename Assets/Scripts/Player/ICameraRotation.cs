using UnityEngine;

public interface ICameraRotation
{
    public abstract void Tick(Vector2 input);
}

public class CameraRotationContext
{
    public Transform CameraTarget { get; }
    public Transform Player { get; }
    public Vector2 input { get; }

    public Transform firstPersonTargetPosition { get; }
    public Transform thirdPersonTargetPosition { get; }

    public CameraRotationContext(Transform cameraTarget, Transform player, Transform firstPersonTargetPosition, Transform thirdPersonTargetPosition)
    {
        this.CameraTarget = cameraTarget;
        this.Player = player;

        this.firstPersonTargetPosition = firstPersonTargetPosition;
        this.thirdPersonTargetPosition = thirdPersonTargetPosition;
    }
}

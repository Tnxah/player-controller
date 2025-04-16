using UnityEngine;

public class ThirdPersonCameraRotation : ICameraRotation
{
    private Transform target;
    private Transform player;
    private Vector2 input;

    public ThirdPersonCameraRotation(CameraRotationContext context)
    {
        this.target = context.CameraTarget;
        this.player = context.Player;

        target.position = context.thirdPersonTargetPosition.position;
        target.localRotation = context.thirdPersonTargetPosition.localRotation;
    }

    public void ApplyRotation()
    {
        target.RotateAround(player.position, Vector3.up, input.x);
        target.RotateAround(player.position, target.right, -input.y);

        target.LookAt(player);
    }

    public void PassInput(Vector2 input)
    {
        this.input = input;
    }
}

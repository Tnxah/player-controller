using UnityEngine;

public class FirstPersonCameraRotation : ICameraRotation
{
    private Transform target;
    private Transform player;
    private Vector2 input;

    private float pitch;
    private float yaw;
    private const float pitchClamp = 90f;

    public FirstPersonCameraRotation (CameraRotationContext context)
    {
        this.target = context.CameraTarget;
        this.player = context.Player;

        target.position = context.firstPersonTargetPosition.position;
        target.localRotation = context.firstPersonTargetPosition.localRotation;
    }

    public void ApplyRotation()
    {
        yaw += input.x;
        pitch -= input.y;

        pitch = Mathf.Clamp(pitch, -pitchClamp, pitchClamp);

        player.rotation = Quaternion.Euler(0f, yaw, 0f);
        target.localRotation = Quaternion.Euler(pitch, 0f, 0f);
    }

    public void PassInput(Vector2 input)
    {
        this.input = input;
    }
}

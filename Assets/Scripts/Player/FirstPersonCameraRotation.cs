using UnityEngine;

public class FirstPersonCameraRotation : BaseCameraRotation
{
    private float pitch;
    private float yaw;

    public FirstPersonCameraRotation(CameraRigReference rig, RotationConfig cfg, Transform anchor)
        : base(rig, cfg, anchor) { }

    public override void Tick(Vector2 input)
    {
        yaw += input.x;
        pitch -= input.y;

        pitch = Mathf.Clamp(pitch, -cfg.yawClamp, cfg.yawClamp);

        rig.Player.rotation = Quaternion.Euler(0f, yaw, 0f);
        rig.Target.localRotation = Quaternion.Euler(pitch, 0f, 0f);
    }
}

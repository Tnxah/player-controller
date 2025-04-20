using UnityEngine;

public class ThirdPersonCameraRotation : BaseCameraRotation
{
    public ThirdPersonCameraRotation(CameraRigReference rig, RotationConfig cfg, Transform anchor)
        : base(rig, cfg, anchor) { }

    public override void Tick(Vector2 input)
    {
        rig.Target.RotateAround(rig.Player.position, Vector3.up, input.x);
        rig.Target.RotateAround(rig.Player.position, rig.Target.right, -input.y);
        rig.Target.LookAt(rig.Player);
    }
}

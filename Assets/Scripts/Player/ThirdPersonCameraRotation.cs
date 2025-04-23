using UnityEngine;

public class ThirdPersonCameraRotation : BaseCameraRotation
{
    public override void Tick(Vector2 input)
    {
        Target.RotateAround(Player.position, Vector3.up, input.x);
        Target.RotateAround(Player.position, Target.right, -input.y);
        Target.LookAt(Player);
    }
}

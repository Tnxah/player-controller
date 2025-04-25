using UnityEngine;

public class ThirdPersonCameraRotation : BaseCameraRotation
{
    private Vector2 input;

    public override void OnInput(Vector2 input)
    {
        this.input = input;
    }

    private void Update()
    {
        if(Mathf.Abs(input.x) > 0)
            Target.RotateAround(Player.position, Vector3.up, input.x);
        if (Mathf.Abs(input.y) > 0)
            Target.RotateAround(Player.position, Target.right, -input.y);
        if(input.magnitude != 0)
            Target.LookAt(Player);
    }
}

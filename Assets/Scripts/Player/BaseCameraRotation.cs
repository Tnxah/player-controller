using UnityEngine;

public abstract class BaseCameraRotation : ICameraRotation
{
    protected readonly CameraRigReference rig;
    protected readonly RotationConfig cfg;

    protected BaseCameraRotation(CameraRigReference rig, RotationConfig cfg, Transform anchor)
    {
        this.rig = rig; 
        this.cfg = cfg;

        if (anchor)
        {
            rig.Target.position = anchor.position;
            rig.Target.rotation = anchor.rotation;
        }
    }

    public abstract void Tick(Vector2 input);
}

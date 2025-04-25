using UnityEngine;

public class FirstPersonCameraRotation : BaseCameraRotation
{
    private float pitch;
    private float yaw;

    private Rigidbody rb;

    private Vector2 input;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (Mathf.Abs(input.y) > 0)
        {
            pitch = Mathf.Clamp(pitch, -cfg.yawClamp, cfg.yawClamp);
            Target.localRotation = Quaternion.Euler(pitch, 0f, 0f);
        }
        
        if (Mathf.Abs(input.x) > 0) 
        { 
            rb.MoveRotation(Quaternion.Euler(0f, yaw, 0f));
        }
    }

    public override void OnInput(Vector2 input)
    {
        this.input = input;

        yaw += input.x;
        pitch -= input.y;
    }
}

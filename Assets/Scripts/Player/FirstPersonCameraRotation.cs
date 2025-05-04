using UnityEngine;
using UnityEngine.InputSystem;

public class FirstPersonCameraRotation : BaseCameraRotation
{
    private float pitch;
    private float yaw;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        var deviceMultiplayer = _currentDevice is Gamepad ? Time.deltaTime : 1;
        if (Mathf.Abs(input.y) > 0)
        {
            pitch -= input.y * deviceMultiplayer;

            pitch = Mathf.Clamp(pitch, -cfg.yawClamp, cfg.yawClamp);
            Target.localRotation = Quaternion.Euler(pitch, 0f, 0f);
        }
        
        if (Mathf.Abs(input.x) > 0) 
        {
            yaw += input.x * deviceMultiplayer;

            rb.MoveRotation(Quaternion.Euler(0f, yaw, 0f));
        }
    }

    public override void OnInput(Vector2 input)
    {
        this.input = input;
    }
}

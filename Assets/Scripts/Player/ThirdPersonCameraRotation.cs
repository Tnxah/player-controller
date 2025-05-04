using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonCameraRotation : BaseCameraRotation
{
    private Vector3 moveDirection;
    private float pitch;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public override void OnInput(Vector2 input)
    {
        this.input = input;
    }

    private void Update()
    {
        var deviceMultiplayer = _currentDevice is Gamepad ? Time.deltaTime : 1;

        if (Mathf.Abs(input.x) > 0)
            Target.RotateAround(Player.position, Vector3.up, input.x * deviceMultiplayer);
        if (Mathf.Abs(input.y) > 0)
        {
            float wantedPitch = Mathf.Clamp(pitch + (-input.y), -cfg.yawClamp, cfg.yawClamp);

            float allowedDelta = wantedPitch - pitch;
            pitch = wantedPitch;

            Target.RotateAround(Player.position, Target.right, allowedDelta * deviceMultiplayer);
        }
        if(input.magnitude != 0)
            Target.LookAt(Player);
    }

    private void FixedUpdate()
    {
        if (moveDirection != transform.forward && moveDirection.sqrMagnitude > 0.0001f)
        {
            Quaternion targetRott = Quaternion.LookRotation(moveDirection, Vector3.up);
         
            rb.MoveRotation(Quaternion.RotateTowards(rb.rotation, targetRott, cfg.turnSpeed * Time.fixedDeltaTime));
        }
    }

    private void GetMoveDirection(MoveDirectionEvent evt)
    {
        this.moveDirection = evt.MoveDirection;
    }

    public override void Enter()
    {
        base.Enter();
        EventBus.Subscribe<MoveDirectionEvent>(GetMoveDirection);
    }

    public override void Exit()
    {
        base.Exit();
        EventBus.Unsubscribe<MoveDirectionEvent>(GetMoveDirection);
    }
}

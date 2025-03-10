using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PlayerControlManager))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float walkSpeed = 6.5f;
    [SerializeField] private float groundAcceleration = 60f;
    [SerializeField] private float airSpeed = 3f;
    [SerializeField] private float airAcceleration = 30f;
    [SerializeField] private float jumpForce = 6f;

    [Header("Damping Settings")]
    [SerializeField] private float groundDamping = 6f;
    [SerializeField] private float airDamping = 0f;

    [Header("Deceleration Settings")]
    [SerializeField] private float decelerationForce = 10f;

    [Header("Ground Detection")]
    [SerializeField] private float groundCheckDistance = 0.15f;
    [SerializeField] private LayerMask groundMask;

    [Header("Body")]
    [SerializeField] private CapsuleCollider bodyCollider;

    private float moveX;
    private float moveZ;
    private Vector3 moveDirection;
    private bool jumpPressed;

    private bool isGrounded;
    private Rigidbody rb;
    private PlayerControls inputActions;

    public void Initialize(PlayerControls inputActions)
    {
        this.inputActions = inputActions;

        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        SubscribeToInputActions();
    }

    private void Update()
    {
        IsGrounded();
        DragControl();
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MovePlayer()
    {
        if (moveZ == 0f && rb.linearVelocity.z == 0f && moveX == 0f && rb.linearVelocity.x == 0f)
            return;

        var horizontalVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        var acceleration = isGrounded ? groundAcceleration : airAcceleration;
        var speed = isGrounded ? walkSpeed : airSpeed;

        moveDirection = (transform.right * moveX + transform.forward * moveZ).normalized;

        if (horizontalVelocity.magnitude > speed)
        {
            acceleration = 0f;
        }

        if (moveDirection.sqrMagnitude > 0.01f)
        {
            rb.AddForce(moveDirection * acceleration, ForceMode.Acceleration); //ForceMode.Force to make it dependent on the rb mass
        }
        else 
        {
            if (horizontalVelocity.magnitude > 0.05f)
            {
                var decelDirection = -horizontalVelocity.normalized;
                rb.AddForce(decelDirection * decelerationForce, ForceMode.Acceleration);
            }
        }
    }

    private void DragControl()
    {
        if (isGrounded)
            rb.linearDamping = groundDamping;
        else
            rb.linearDamping = airDamping;
    }

    private void IsGrounded()
    {
        var radius = bodyCollider.radius;

        isGrounded = Physics.OverlapSphere(transform.position + new Vector3(0f, radius - groundCheckDistance, 0f), radius, groundMask).Length > 0;
    }

    private void SubscribeToInputActions()
    {
        inputActions.Player.MoveX.performed += ctx => moveX = ctx.ReadValue<float>();
        inputActions.Player.MoveZ.performed += ctx => moveZ = ctx.ReadValue<float>();
        inputActions.Player.Jump.performed += ctx => print("jump");
        inputActions.Player.MoveX.canceled += ctx => moveX = 0f;
        inputActions.Player.MoveZ.canceled += ctx => moveZ = 0f;
    }

    private void UnsubscribeFromInputActions()
    {
        inputActions.Player.MoveX.performed -= ctx => moveX = ctx.ReadValue<float>();
        inputActions.Player.MoveZ.performed -= ctx => moveZ = ctx.ReadValue<float>();
        inputActions.Player.Jump.performed -= ctx => print("jump");
        inputActions.Player.MoveX.canceled -= ctx => moveX = 0f;
        inputActions.Player.MoveZ.canceled -= ctx => moveZ = 0f;
    }

    private void OnDisable()
    {
        UnsubscribeFromInputActions();
    }
}

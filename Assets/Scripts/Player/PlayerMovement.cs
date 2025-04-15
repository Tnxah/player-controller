using UnityEngine;
using UnityEngine.InputSystem;

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
    [SerializeField] private float inputDeadZone = 0.1f;

    [Header("Damping Settings")]
    [SerializeField] private float groundDamping = 6f;
    [SerializeField] private float airDamping = 0f;

    [Header("Deceleration Settings")]
    [SerializeField] private float decelerationForce = 10f;

    [Header("Ground Detection")]
    [SerializeField] private float groundCheckDistance = 0.1f;
    [SerializeField] private LayerMask groundMask;

    [Header("Body")]
    [SerializeField] private CapsuleCollider bodyCollider;

    private Vector2 moveInput;
    private Vector3 moveDirection;
    private bool jumpPressed;

    private bool isGrounded;
    private Rigidbody rb;
    private PlayerControls inputActions;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    public void Initialize(PlayerControls inputActions)
    {
        this.inputActions = inputActions;
        SubscribeToInputActions();
    }

    private void Update()
    {
        CheckGrounded();
        ApplyDrag();
    }

    private void FixedUpdate()
    {
        HandleJump();
        HandleMovement();
    }

    private void OnMovePerformed(InputAction.CallbackContext ctx) => moveInput = ctx.ReadValue<Vector2>();
    private void OnMoveCanceled(InputAction.CallbackContext ctx) => moveInput = Vector2.zero;
    private void OnJumpPerformed(InputAction.CallbackContext ctx) => jumpPressed = isGrounded ? true : false;

    private void HandleMovement()
    {
        if (moveInput.magnitude <= inputDeadZone && rb.linearVelocity.z <= inputDeadZone && rb.linearVelocity.x <= inputDeadZone)
            return;

        var horizontalVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        var acceleration = isGrounded ? groundAcceleration : airAcceleration;
        var speed = isGrounded ? walkSpeed : airSpeed;

        moveDirection = (transform.right * moveInput.x + transform.forward * moveInput.y).normalized;

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

    private void HandleJump()
    {
        if (!jumpPressed || !isGrounded) return;

        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        jumpPressed = false;
    }

    private void ApplyDrag()
    {
        rb.linearDamping = isGrounded ? groundDamping : airDamping;
    }

    private void CheckGrounded()
    {
        var radius = bodyCollider.radius;
        var spherePosition = transform.position + new Vector3(0f, radius - groundCheckDistance, 0f);
        isGrounded = Physics.CheckSphere(spherePosition, radius, groundMask);
    }

    private void SubscribeToInputActions()
    {
        inputActions.Player.Move.performed += OnMovePerformed;
        inputActions.Player.Move.canceled += OnMoveCanceled;
        inputActions.Player.Jump.performed += OnJumpPerformed;
    }

    private void UnsubscribeFromInputActions()
    {
        inputActions.Player.Move.performed -= OnMovePerformed;
        inputActions.Player.Move.canceled -= OnMoveCanceled;
        inputActions.Player.Jump.performed -= OnJumpPerformed;
    }

    private void OnDisable()
    {
        UnsubscribeFromInputActions();
    }

    private void OnDrawGizmosSelected()
    {
        if (bodyCollider == null) return;

        var radius = bodyCollider.radius;
        var spherePosition = transform.position + new Vector3(0f, radius - groundCheckDistance, 0f);

        Gizmos.color = isGrounded ? Color.green : Color.red;
        Gizmos.DrawWireSphere(spherePosition, radius);
    }

}

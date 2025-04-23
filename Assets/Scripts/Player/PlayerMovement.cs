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

    [Header("Jump")]
    [SerializeField] private float jumpForce = 6f;
    
    [SerializeField] private float inputDeadZone = 0.1f;

    [Header("Damping Settings")]
    [SerializeField] private float groundDamping = 6f;
    [SerializeField] private float airDamping = 0f;

    [Header("Deceleration Settings")]
    [SerializeField] private float decelerationForce = 10f;

    [Header("Turning (3rd?person)")]
    [SerializeField] private float turnSpeed = 720f; // deg/sec

    [Header("Ground Detection")]
    [SerializeField] private float groundCheckDistance = 0.1f;
    [SerializeField] private LayerMask groundMask;

    [Header("Body")]
    [SerializeField] private CapsuleCollider bodyCollider;

    private Vector3 moveDirection;
    private bool jumpPressed;

    private ViewModeState viewModeState;     // set from bootstrap

    private Rigidbody rb;
    private PlayerControls inputActions;

    private Vector2 moveInput;
    private bool isGrounded;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        viewModeState = GetComponent<ViewModeState>();
        rb.freezeRotation = true;
    }

    public void Initialize(PlayerControls input)
    {
        inputActions = input;
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
        //if (viewModeState.Current == ViewMode.ThirdPerson)
        //{
            // camera‑relative movement
            Vector3 camFwd = Camera.main.transform.forward; camFwd.y = 0; camFwd.Normalize();
            Vector3 camRight = Camera.main.transform.right; camRight.y = 0; camRight.Normalize();
            moveDirection = (camRight * moveInput.x + camFwd * moveInput.y).normalized;

            // smooth‑rotate player towards motion
            if (moveDirection.sqrMagnitude > 0.0001f)
            {
                Quaternion targetRot = Quaternion.LookRotation(moveDirection, Vector3.up);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, turnSpeed * Time.fixedDeltaTime);
            }
        //}
        //else // First‑person: use player local axes
        //{
        //    moveDirection = (transform.right * moveInput.x + transform.forward * moveInput.y).normalized;
        //}

        if (moveInput.magnitude <= inputDeadZone && rb.linearVelocity.z <= inputDeadZone && rb.linearVelocity.x <= inputDeadZone)
            return;

        var horizontalVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        var acceleration = isGrounded ? groundAcceleration : airAcceleration;
        var speed = isGrounded ? walkSpeed : airSpeed;

        //moveDirection = (transform.right * moveInput.x + transform.forward * moveInput.y).normalized;

        if (horizontalVelocity.magnitude > speed)
        {
            acceleration = 0f;
        }

        if (this.moveDirection.sqrMagnitude > 0.01f)
        {
            rb.AddForce(this.moveDirection * acceleration, ForceMode.Acceleration); //ForceMode.Force to make it dependent on the rb mass
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
        var spherePosition = transform.position + Vector3.down * ((bodyCollider.height / 2f) - bodyCollider.radius + groundCheckDistance);
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
        var spherePosition = transform.position + Vector3.down * ((bodyCollider.height / 2f) - bodyCollider.radius + groundCheckDistance);

        Gizmos.color = isGrounded ? Color.green : Color.red;
        Gizmos.DrawWireSphere(spherePosition, radius);
    }

}

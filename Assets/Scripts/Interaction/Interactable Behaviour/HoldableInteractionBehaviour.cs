using UnityEngine;
public class HoldableInteractionBehaviour : MonoBehaviour, IInteractableBehaviour
{
    private Transform instance; 
    private Transform holdPoint;
    private Rigidbody rb;
    
    private float springConstant = 60f;
    private float dampingFactor = 9f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Interact(InteractableBehaviourContext ctx)
    {
        if(ctx is not HoldInteractionBehaviourContext context)
        {
            Debug.LogWarning("Invalid context provided for HoldableInteractionBehaviour.");
            return;
        }

        holdPoint = context.holdPoint;
        rb.useGravity = false;
    }

    public void EndInteraction()
    {
        holdPoint = null;
        rb.useGravity = true; ;
    }

    private void Follow()
    {
        Vector3 displacement = transform.position - holdPoint.position;

        if (displacement.magnitude > 7f)
        {
            EndInteraction();
        }

        Vector3 springForce = -springConstant * displacement;
        Vector3 dampingForce = -dampingFactor * rb.linearVelocity;

        rb.AddForce(springForce + dampingForce, ForceMode.Force);
    }

    private void FixedUpdate()
    {
        if(holdPoint)
            Follow();
    }
}

public class HoldInteractionBehaviourContext : InteractableBehaviourContext
{
    public Transform holdPoint { get; }

    public HoldInteractionBehaviourContext(Transform holdPoint)
    {
        this.holdPoint = holdPoint;
    }
}

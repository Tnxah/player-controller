using UnityEngine;

public class RaycastInteractor : IInteractor
{
    public IInteractable TryInteract(InteractorContext ctx)
    {
        if (ctx is not RaycastInteractorContext context)
        {
            Debug.LogWarning("Invalid context provided for RaycastInteractor.");
            return null;
        }

        Ray ray = new Ray(context.source.position, context.source.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, context.interactionDistance) && hit.collider.TryGetComponent(out IInteractable interactable))
        {
            return interactable;
        }

        return null;
    }
}

public class RaycastInteractorContext : InteractorContext
{
    public Transform source { get; }
    public float interactionDistance { get; }

    public RaycastInteractorContext(Transform source, float interactionDistance)
    {
        this.source = source;
        this.interactionDistance = interactionDistance;
    }
}

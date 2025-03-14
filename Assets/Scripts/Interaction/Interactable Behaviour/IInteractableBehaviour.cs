public interface IInteractableBehaviour
{
    public abstract void Interact(InteractableBehaviourContext context);
    public abstract void EndInteraction();
}

public abstract class InteractableBehaviourContext { }
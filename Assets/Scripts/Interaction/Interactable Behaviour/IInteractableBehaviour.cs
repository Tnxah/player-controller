public interface IInteractableBehaviour
{
    public abstract void Interact(InteractableBehaviourContext context);
    public abstract void EndInteraction();
}

public class InteractableBehaviourContext { }
public interface IInteractor
{
    public abstract IInteractable TryInteract(InteractorContext context);
}

public abstract class InteractorContext { }

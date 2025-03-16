using UnityEngine;

public class PerformableInteractionBehaviour : MonoBehaviour, IInteractableBehaviour
{
    public void Interact(InteractableBehaviourContext context)
    {
        print("Action Performed");
    }

    public void EndInteraction()
    {
        //throw new System.NotImplementedException();
    }
}

public class PerformableInteractionBehaviourContext : InteractableBehaviourContext
{
}

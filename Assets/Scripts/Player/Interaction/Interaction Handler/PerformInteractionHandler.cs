using UnityEngine;

public class PerformInteractionHandler : MonoBehaviour
{
    private PerformableInteractionBehaviour performable;
    private InteractableBehaviourContext interactableBehaviourContext;

    private void Awake()
    {
        interactableBehaviourContext = new InteractableBehaviourContext();
    }

    private void OnInteract(InteractEvent evt)
    {
        var behaviour = evt.Interactable.GetInteractionBehaviour();

        if (behaviour is not PerformableInteractionBehaviour performableBehaviour)
        {
            Debug.LogWarning("Not related behavior. Ignored.");
            return;
        }

        if (performable == null)
        {
            performable = performableBehaviour;
            performable.Interact(interactableBehaviourContext);
        }
    }

    private void OnEndInteraction(EndInteractionEvent evt)
    {
        var behaviour = evt.Interactable.GetInteractionBehaviour();

        if (behaviour is not PerformableInteractionBehaviour performableBehaviour)
        {
            Debug.LogWarning("Not related behavior. Ignored.");
            return;
        }

        if (performable == performableBehaviour)
        {
            performable.EndInteraction();
            performable = null;
        }
    }

    private void OnEnable()
    {
        EventBus.Subscribe<InteractEvent>(OnInteract);
        EventBus.Subscribe<EndInteractionEvent>(OnEndInteraction);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe<InteractEvent>(OnInteract);
        EventBus.Unsubscribe<EndInteractionEvent>(OnEndInteraction);
    }
}

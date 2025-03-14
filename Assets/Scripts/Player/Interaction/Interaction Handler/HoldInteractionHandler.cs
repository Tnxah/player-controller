using UnityEngine;

public class HoldInteractionHandler : MonoBehaviour
{
    [SerializeField] private Transform holdPoint;

    private HoldableInteractionBehaviour holdable;
    private HoldInteractionBehaviourContext holdInteractionBehaviourContext;

    private void Awake()
    {
        holdInteractionBehaviourContext = new HoldInteractionBehaviourContext(holdPoint);
    }

    private void OnInteract(InteractEvent evt)
    {
        var behaviour = evt.Interactable.GetInteractionBehaviour();

        if (behaviour is not HoldableInteractionBehaviour holdableBehaviour) 
        {
            Debug.LogWarning("Not related behavior. Ignored.");
            return;
        }

        //TODO: I dont like this if statements
        //solvable by adding separate event (more info in PlayerInteraction TODO)
        if (holdable == null)
        {
            holdable = holdableBehaviour;
            holdable.Interact(holdInteractionBehaviourContext);
        }
        else if (holdable == holdableBehaviour)
        {
            holdable.EndInteraction();
            holdable = null;
        }
    }

    private void OnEnable()
    {
        EventBus.Subscribe<InteractEvent>(OnInteract);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe<InteractEvent>(OnInteract);
    }
}

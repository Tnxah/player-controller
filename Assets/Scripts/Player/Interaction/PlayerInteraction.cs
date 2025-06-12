using UnityEngine;

public class PlayerInteraction : PlayerComponentBase
{
    private IInteractor interactor;
    private IInteractable interactable;
    private InteractorContext interactorContext;
    private InteractionDetector interactionDetector;

    [Header("Raycast Interactor settings")]
    [SerializeField] private float interactionDistance = 3.5f;

    [Header("Interaction Detection settings")]
    [SerializeField] private float detectionDelaySeconds = 0.1f;

    private void Awake()
    {   
        interactionDetector = gameObject.AddComponent<InteractionDetector>();
        InitializeRaycastInteractor();
    }

    private void InitializeRaycastInteractor()
    {
        interactor = new RaycastInteractor();
        interactorContext = new RaycastInteractorContext(Camera.main.transform, interactionDistance);

        interactionDetector.Initialize(interactor, interactorContext, detectionDelaySeconds);
    }

    private void Interact()
    {
        interactable = interactor.TryInteract(interactorContext);

        if (interactable != null)
        {
            EventBus.Publish(new InteractEvent { Interactable = interactable });
        }
    }

    private void EndInteraction()
    {
        if (interactable != null)
        {
            EventBus.Publish(new EndInteractionEvent { Interactable = interactable });

            interactable = null;
        }
    }

    protected override void SubscribeToInputActions()
    {
        inputActions.Player.Interact.performed += ctx => Interact();
        inputActions.Player.Interact.canceled += ctx => EndInteraction();
    }

    protected override void UnsubscribeFromInputActions()
    {
        inputActions.Player.Interact.performed -= ctx => Interact();
        inputActions.Player.Interact.canceled -= ctx => EndInteraction();
    }
}

public class InteractEvent
{
    public IInteractable Interactable { get; set; }
}

public class EndInteractionEvent
{
    public IInteractable Interactable { get; set; }
}

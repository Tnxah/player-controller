using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private PlayerControls inputActions;

    private IInteractor interactor;
    private IInteractable interactable;
    private InteractorContext interactorContext;
    private InteractionDetector interactionDetector;

    [Header("Raycast Interactor settings")]
    [SerializeField] private float interactionDistance = 3.5f;

    [Header("Interaction Detection settings")]
    [SerializeField] private float detectionDelaySeconds = 0.1f;

    public void Initialize(PlayerControls inputActions)
    {
        this.inputActions = inputActions;
        interactionDetector = gameObject.AddComponent<InteractionDetector>();

        InitializeRaycastInteractor();

        SubscribeToInputActions();
    }

    private void InitializeRaycastInteractor() //TODO: Maybe can be simplified? Without manual preparations.
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

    private void SubscribeToInputActions()
    {
        inputActions.Player.Interact.performed += ctx => Interact();
        inputActions.Player.Interact.canceled += ctx => EndInteraction();
    }

    private void UnsubscribeFromInputActions()
    {
        inputActions.Player.Interact.performed -= ctx => Interact();
        inputActions.Player.Interact.canceled -= ctx => EndInteraction();
    }

    private void OnDisable()
    {
        UnsubscribeFromInputActions();
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

using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private PlayerControls inputActions;

    private IInteractor interactor;
    private IInteractable interactable;
    private InteractorContext interactorContext;

    [Header("Raycast Interactor settings")]
    [SerializeField] private float interactionDistance = 3.5f;

    public void Initialize(PlayerControls inputActions)
    {
        this.inputActions = inputActions;
        InitializeRaycastInteractor();

        SubscribeToInputActions();
    }

    private void InitializeRaycastInteractor()
    {
        interactor = new RaycastInteractor();
        interactorContext = new RaycastInteractorContext(Camera.main.transform, interactionDistance);
    }

    private void Interact()
    {
        interactable = interactor.TryInteract(interactorContext);

        if (interactable != null) 
        {
            IInteractionBehaviour behavior = interactable.GetInteractionBehaviour();
            behavior.Interact();
        }
    }

    private void EndInteraction()
    {
        if (interactable != null)
        {
            IInteractionBehaviour behavior = interactable.GetInteractionBehaviour();
            behavior.EndInteraction();

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

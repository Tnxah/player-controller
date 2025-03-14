using UnityEngine;

public class Interactable : MonoBehaviour, IInteractable
{
    private HoldableInteractionBehaviour interactionBehaviour; //TODO: No sence if i have instance of concrete behavior instead of IInteractableBehavior

    private void Awake()
    {
        interactionBehaviour = gameObject.AddComponent<HoldableInteractionBehaviour>();
        interactionBehaviour.Init(GetComponent<Rigidbody>());
    }

    public IInteractableBehaviour GetInteractionBehaviour()
    {
        return interactionBehaviour;
    }
}

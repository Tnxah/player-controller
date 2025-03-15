using UnityEngine;

public class Interactable : MonoBehaviour, IInteractable
{
    private IInteractableBehaviour interactableBehaviour;

    private void Awake()
    {
        interactableBehaviour = GetComponent<IInteractableBehaviour>();
    }

    public IInteractableBehaviour GetInteractionBehaviour()
    {
        return interactableBehaviour;
    }
}

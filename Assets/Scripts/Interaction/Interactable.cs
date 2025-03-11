using UnityEngine;

public class Interactable : MonoBehaviour, IInteractable
{
    public IInteractionBehaviour GetInteractionBehaviour()
    {
        throw new System.NotImplementedException();
    }
}

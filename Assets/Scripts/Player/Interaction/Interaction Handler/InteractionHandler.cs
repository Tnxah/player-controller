using System;
using System.Collections.Generic;
using UnityEngine;

public class InteractionHandler : MonoBehaviour
{
    private Dictionary<Type, InteractableBehaviourContext> contexts = new Dictionary<Type, InteractableBehaviourContext>();
    private IInteractableBehaviour activeBehaviour;

    [SerializeField] private Transform holdPoint;

    private void Awake()
    {
        contexts.Add(typeof(HoldableInteractionBehaviour), new HoldableInteractionBehaviourContext(holdPoint));
        contexts.Add(typeof(PerformableInteractionBehaviour), new PerformableInteractionBehaviourContext());
    }

    private void OnInteract(InteractEvent evt)
    {
        var behaviour = evt.Interactable.GetInteractionBehaviour();

        if (behaviour == null)
        {
            Debug.LogWarning("Not valid interaction behaviour found.");
            return;
        }

        var behaviourType = behaviour.GetType();
        if (!contexts.TryGetValue(behaviourType, out InteractableBehaviourContext context))
        {
            Debug.LogWarning($"No matching context found for {behaviourType.Name}. Ignored.");
            return;
        }

        if (activeBehaviour == null)
        {
            activeBehaviour = behaviour;
            activeBehaviour.Interact(context);
        }
    }

    private void OnEndInteraction(EndInteractionEvent evt)
    {
        var behaviour = evt.Interactable.GetInteractionBehaviour();

        if (behaviour == null || behaviour != activeBehaviour)
        {
            Debug.LogWarning("No valid interaction behavior to end.");
            return;
        }

        activeBehaviour.EndInteraction();
        activeBehaviour = null;
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

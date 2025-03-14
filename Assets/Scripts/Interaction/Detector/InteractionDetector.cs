using System.Collections;
using UnityEngine;

public class InteractionDetector : MonoBehaviour
{
    private IInteractor interactor;
    private InteractorContext interactorContext;
    private float detectionDelaySeconds;

    private bool detect;

    public void Initialize(IInteractor interactor, InteractorContext interactorContext, float detectionDelaySeconds)
    {
        this.interactor = interactor;
        this.interactorContext = interactorContext;
        this.detectionDelaySeconds = detectionDelaySeconds;

        StartDetection();
    }

    private void StartDetection()
    {
        detect = true;
        StartCoroutine(DetectInteraction());
    }

    private void StopDetection()
    {
        detect = false;
    }

    private IEnumerator DetectInteraction()
    {
        while (detect)
        {
            var interactable = interactor.TryInteract(interactorContext);

            EventBus.Publish(new InteractionDetectionEvent { Interactable = interactable });
            
            yield return new WaitForSeconds(detectionDelaySeconds);
        }
    }

    private void OnDisable()
    {
        StopDetection();
    }
}

public class InteractionDetectionEvent
{
    public IInteractable Interactable { get; set; }
}

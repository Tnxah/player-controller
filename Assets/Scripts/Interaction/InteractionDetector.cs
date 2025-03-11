using System.Collections;
using UnityEngine;

public class InteractionDetector : MonoBehaviour
{
    private IInteractor interactor;
    private InteractorContext interactorContext;

    private bool detect;

    public void Initialize(IInteractor interactor, InteractorContext interactorContext)
    {
        this.interactor = interactor;
        this.interactorContext = interactorContext;

        StartDetection();
    }

    private void StartDetection()
    {
        StartCoroutine(DetectInteraction());
    }

    private IEnumerator DetectInteraction()
    {
        while (detect)
        {
            if (interactor.TryInteract(interactorContext) != null)
            {
                Debug.Log("Fire detected event");
            }
            else
            {
                Debug.Log("Fire undetected event");
            }

            yield return new WaitForSeconds(0.2f);
        }
    }
}

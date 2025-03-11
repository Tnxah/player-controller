using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIInteractionManager : MonoBehaviour
{
    private Image image;
    private RectTransform rectTransform;

    [Header("Aim settings")]
    [SerializeField] private float aimInactiveSize = 4f;
    [SerializeField] private float aimActiveSize = 5f;
    [SerializeField] private float aimInactiveAlpha = 0.3f;
    [SerializeField] private float aimActiveAlpha = 1f;

    private IEnumerator coroutine;

    private void Awake()
    {
        image = GetComponent<Image>();
        rectTransform = GetComponent<RectTransform>();

        image.color = new Color(image.color.r, image.color.g, image.color.b, aimInactiveAlpha);
        rectTransform.sizeDelta = new Vector2(aimInactiveSize, aimInactiveSize);
    }

    private void OnInteractionDetected(InteractionDetectionEvent evt)
    {
        Activation(evt.Interactable != null);
    }

    private void Activation(bool activate)
    {
        if(coroutine != null)
            StopCoroutine(coroutine);

        coroutine = Scale(activate);

        StartCoroutine(coroutine);
    }

    private IEnumerator Scale(bool activate)
    {
        var targetAlpha = activate ? aimActiveAlpha : aimInactiveAlpha;
        var targetSize = activate ? aimActiveSize : aimInactiveSize;
        var step = 5f;

        var color = image.color;
        var alpha = color.a;
        var sizeX = rectTransform.sizeDelta.x;
        var sizeY = rectTransform.sizeDelta.y;

        while (!Mathf.Approximately(image.color.a, targetAlpha)
            || !Mathf.Approximately(rectTransform.sizeDelta.x, targetSize)
            || !Mathf.Approximately(rectTransform.sizeDelta.y, targetSize)) 
        {

            alpha = Mathf.MoveTowards(image.color.a, targetAlpha, step * Time.deltaTime);
            sizeX = Mathf.MoveTowards(rectTransform.sizeDelta.x, targetSize, step * Time.deltaTime);
            sizeY = Mathf.MoveTowards(rectTransform.sizeDelta.y, targetSize, step * Time.deltaTime);

            image.color = new Color(color.r, color.g, color.b, alpha);
            rectTransform.sizeDelta = new Vector2(sizeX, sizeY);

            yield return null;
        }
    }

    private void OnEnable()
    {
        EventBus.Subscribe<InteractionDetectionEvent>(OnInteractionDetected);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe<InteractionDetectionEvent>(OnInteractionDetected);
    }
}

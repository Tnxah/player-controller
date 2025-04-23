using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Smoothing")]
    [SerializeField] private float positionSmoothing = 15f;
    [SerializeField] private float rotationSmoothing = 15f;

    private Transform followTarget;
    private Vector3 velocity = Vector3.zero;

    private void Awake()
    {
        EventBus.Subscribe<BaseCameraRotation>(SetFollowTarget);
    }

    public void SetFollowTarget(BaseCameraRotation cameraRotation)
    {
        followTarget = cameraRotation.Target;
    }

    private void LateUpdate()
    {
        if (followTarget == null) return;

        if (transform.position != followTarget.position)
            // Smooth position
            transform.position = Vector3.SmoothDamp(
                transform.position,
                followTarget.position,
                ref velocity,
                1f / positionSmoothing,
                Mathf.Infinity,
                Time.deltaTime
            );

        if (transform.rotation != followTarget.rotation)
            // Smooth rotation
            transform.rotation = Quaternion.Slerp(
            transform.rotation,
            followTarget.rotation,
            Time.deltaTime * rotationSmoothing
            );
    }
}

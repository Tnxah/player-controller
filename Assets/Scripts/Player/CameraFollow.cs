using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Smoothing")]
    private float positionSmoothing = 100f;
    private float rotationSmoothing = 100f;

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
        //transform.position = followTarget.position;

        if (transform.rotation != followTarget.rotation)
            //Smooth rotation
            transform.rotation = Quaternion.Slerp(
            transform.rotation,
            followTarget.rotation,
            Time.deltaTime * rotationSmoothing
            );
        //transform.rotation = followTarget.rotation;
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe<BaseCameraRotation>(SetFollowTarget);
    }
}

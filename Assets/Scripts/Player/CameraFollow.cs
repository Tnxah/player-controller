using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Smoothing")]
    [SerializeField] private float positionSmoothing = 10f;
    [SerializeField] private float rotationSmoothing = 15f;

    [SerializeField] private Transform followTarget;
    private Vector3 velocity = Vector3.zero;

    public void SetFollowTarget(Transform target)
    {
        followTarget = target;
    }

    private void LateUpdate()
    {
        if (followTarget == null) return;

        // Smooth position
        transform.position = Vector3.SmoothDamp(
            transform.position,
            followTarget.position,
            ref velocity,
            1f / positionSmoothing,
            Mathf.Infinity,
            Time.deltaTime
        );

        // Smooth rotation
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            followTarget.rotation,
            Time.deltaTime * rotationSmoothing
        );
    }
}

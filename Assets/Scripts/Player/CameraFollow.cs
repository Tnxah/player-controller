using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Follow Settings")]
    [SerializeField] private Transform cameraTarget;
    [SerializeField] private float positionSmoothing = 50f;
    [SerializeField] private float rotationSmoothing = 300f;
    [SerializeField] private Vector3 offset = Vector3.zero;


    private void LateUpdate()
    {
        if (!cameraTarget) return;

        Follow();
    }

    private void Follow()
    {
        var desiredPosition = cameraTarget.position + offset;

        if (transform.position != desiredPosition && Mathf.Abs((desiredPosition - transform.position).magnitude) < 1f)
        {
            transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * positionSmoothing);
        }
        else
        {
            transform.position = desiredPosition;
        }

        if(transform.rotation != cameraTarget.rotation)
        transform.rotation = Quaternion.Lerp(transform.rotation, cameraTarget.rotation, Time.deltaTime * rotationSmoothing);
    }
}

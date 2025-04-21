using UnityEngine;

public class ThirdPersonCameraAnchor : MonoBehaviour
{
    public Transform player;

    [SerializeField] private Transform cameraTarget;

    public Transform Initialize(Transform player)
    {
        this.player = player;
        return cameraTarget;
    }

    private void FixedUpdate()
    {
        if (player != null)
        {
            transform.position = player.position;
        }
    }
}

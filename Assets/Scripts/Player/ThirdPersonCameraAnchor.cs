using UnityEngine;

public class ThirdPersonCameraAnchor : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Vector3 offset = new Vector3(0f, 1.5f, 0f);

    private void Update()
    {
        transform.position = player.position + offset;
    }
}

using UnityEngine;

public class ThirdPersonCameraAnchor : MonoBehaviour
{
    public Transform player;

    private void Update()
    {
        transform.position = player.position;
    }
}

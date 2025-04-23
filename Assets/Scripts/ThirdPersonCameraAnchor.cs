using UnityEngine;

public class ThirdPersonCameraAnchor : MonoBehaviour
{
    public Transform player;

    private void FixedUpdate()
    {
        if (player != null && player.transform.position != transform.position)
        {
            if (transform.position != player.position)
            {
                transform.position = Vector3.Lerp(transform.position, player.position, Time.fixedDeltaTime * 80);
            }
        }
    }
}

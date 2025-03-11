using UnityEngine;

public class PlayerControlManager : MonoBehaviour
{
    private PlayerControls PlayerControls;

    [SerializeField] private PlayerRotation PlayerRotation;
    [SerializeField] private PlayerMovement PlayerMovement;
    [SerializeField] private PlayerInteraction PlayerInteraction;

    private void Awake()
    {
        PlayerControls = new PlayerControls();

        PlayerMovement.Initialize(PlayerControls);
        PlayerRotation.Initialize(PlayerControls);
        PlayerInteraction.Initialize(PlayerControls);
    }

    private void OnEnable()
    {
        PlayerControls.Enable();
    }

    private void OnDisable()
    {
        PlayerControls.Disable();
    }
}

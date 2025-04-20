using UnityEngine;

public class PlayerControlManager : MonoBehaviour
{
    private PlayerControls PlayerControls;

    [SerializeField] private PlayerRotation PlayerRotation;
    [SerializeField] private PlayerMovement PlayerMovement;
    [SerializeField] private PlayerInteraction PlayerInteraction;
    [SerializeField] private ViewModeState ViewModeState;

    private void Awake()
    {
        PlayerControls = new PlayerControls();

        PlayerMovement.Initialize(PlayerControls);
        PlayerRotation.Initialize(PlayerControls);
        PlayerInteraction.Initialize(PlayerControls);
        ViewModeState.Initialize(PlayerControls);
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

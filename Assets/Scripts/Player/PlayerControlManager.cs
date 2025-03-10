using UnityEngine;

public class PlayerControlManager : MonoBehaviour
{
    private PlayerControls PlayerControls;

    [SerializeField] private PlayerRotation PlayerRotation;
    [SerializeField] private PlayerMovement PlayerMovement;
    [SerializeField] private PlayerInteraction PlayerHold;

    private void Awake()
    {
        PlayerControls = new PlayerControls();

        PlayerMovement.Initialize(PlayerControls);
        PlayerRotation.Initialize(PlayerControls);
        PlayerHold.Initialize(PlayerControls);
    }

    private void OnEnable()
    {
        PlayerControls.Enable();
    }

    private void OnDisable()
    {
        PlayerControls.Disable();
    }

    public Transform GetHeldObject() => PlayerHold.interactingObject;
}

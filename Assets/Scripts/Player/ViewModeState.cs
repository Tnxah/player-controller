using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ViewModeState : MonoBehaviour
{
    [SerializeField] private int currentViewModeNumber = 0;

    private List<BaseCameraRotation> cameraRotationBehaviours;

    private PlayerControls inputActions;

    private void Awake()
    {
        cameraRotationBehaviours = GetComponents<BaseCameraRotation>().ToList();
    }

    private void Start()
    {
        Toggle();
    }

    public void Initialize(PlayerControls input)
    {
        inputActions = input;
        inputActions.Player.ToggleView.performed += _ => Toggle();
    }

    private void OnDisable() => inputActions.Player.ToggleView.performed -= _ => Toggle();

    private void Toggle()
    {
        print("Toggle()");
        EventBus.Publish(cameraRotationBehaviours[currentViewModeNumber++]);
        currentViewModeNumber = currentViewModeNumber == cameraRotationBehaviours.Count ? 0 : currentViewModeNumber;
    }
}
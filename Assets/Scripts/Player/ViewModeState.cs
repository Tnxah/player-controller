using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ViewModeState : PlayerComponentBase
{
    [SerializeField] private int currentViewModeNumber = 0;

    private List<BaseCameraRotation> cameraRotationBehaviours;

    private void Awake()
    {
        cameraRotationBehaviours = GetComponents<BaseCameraRotation>().ToList();
        cameraRotationBehaviours.ForEach(beh => beh.enabled = false);
    }

    private void Start()
    {
        SetStartViewMode();
    }

    private void OnDisable() => inputActions.Player.ToggleView.performed -= _ => Toggle();

    private void SetStartViewMode()
    {
        EventBus.Publish(cameraRotationBehaviours[currentViewModeNumber]);
        cameraRotationBehaviours[currentViewModeNumber].Enter();
    }

    private void Toggle()
    {
        cameraRotationBehaviours[currentViewModeNumber].Exit();

        currentViewModeNumber = currentViewModeNumber == cameraRotationBehaviours.Count - 1 ? 0 : currentViewModeNumber + 1;

        EventBus.Publish(cameraRotationBehaviours[currentViewModeNumber]);
        
        cameraRotationBehaviours[currentViewModeNumber].Enter();
    }

    protected override void SubscribeToInputActions()
    {
        inputActions.Player.ToggleView.performed += _ => Toggle();
    }

    protected override void UnsubscribeFromInputActions()
    {
        inputActions.Player.ToggleView.performed -= _ => Toggle();
    }
}
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
        cameraRotationBehaviours.ForEach(beh => beh.enabled = false);
    }

    private void Start()
    {
        SetStartViewMode();
    }

    public void Initialize(PlayerControls input)
    {
        inputActions = input;
        inputActions.Player.ToggleView.performed += _ => Toggle();
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
}
using System;
using UnityEngine;

public class ViewModeState : MonoBehaviour
{
    [SerializeField] private ViewMode startMode = ViewMode.FirstPerson;

    private ViewMode _current;
    public ViewMode Current => _current;

    private PlayerControls inputActions;

    private void Awake()
    {
        _current = startMode;
    }

    public void Initialize(PlayerControls input)
    {
        inputActions = input;
        inputActions.Player.ToggleView.performed += _ => Toggle();
    }

    private void OnDisable() => inputActions.Player.ToggleView.performed -= _ => Toggle();

    private void Toggle()
    {
        _current = _current == ViewMode.FirstPerson ? ViewMode.ThirdPerson : ViewMode.FirstPerson;
        EventBus.Publish(_current);
    }
}
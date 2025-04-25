using UnityEngine;

public abstract class BaseCameraRotation : MonoBehaviour, ICameraRotation
{
    [SerializeField] protected RotationConfig cfg;
    [SerializeField] public Transform Target;
    [SerializeField] protected Transform Player;

    public abstract void OnInput(Vector2 input);
}

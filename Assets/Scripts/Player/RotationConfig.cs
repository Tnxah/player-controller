using UnityEngine;

[CreateAssetMenu(fileName = "RotationConfig", menuName = "Camera/RotationConfig")]
public class RotationConfig : ScriptableObject
{
    public float yawClamp = 90f;
    public float turnSpeed = 10f;
}
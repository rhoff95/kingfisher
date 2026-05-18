using UnityEngine;

[CreateAssetMenu(fileName = "Actor Properties", menuName = "Scriptable Objects/Actor Properties")]
public class ActorProperties : ScriptableObject
{
    [Header("Movement")]
    public Vector2 initialVelocity;
    [Range(0f, 1f)]public float linearVelocitySmoothTime;
    [Range(0f, 100f)] public float thrustAcceleration;
    [Range(0f, 100f)] public float gravityAcceleration;
    [Range(0f, 100f)] public float buoyancyAcceleration;
    [Range(0f, 100f)] public float maxSpeed;
    
    [Header("Rotation")]
    [Range(0f, 500f)] public float rotationSpeedNoThrust;
    [Range(0f, 500f)] public float rotationSpeedThrust;
}
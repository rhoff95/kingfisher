using System;
using Scripts;
using UnityEngine;

[RequireComponent(typeof(ActorController))]
public class BeeController : MonoBehaviour
{
    private ActorController _actorController;
    private Transform _actorTransform;

    [TagSelector] public string playerTag;

    [Range(0f, 90f)] public float angleThreshold;
    [Range(0f, 100f)] public float accelerationRange;
    [Range(0f, 100f)] public float closeupRange;
    public float waterLevel;
    
    private Color _thrustMode;
    
    private void Awake()
    {
        _actorController = GetComponent<ActorController>();

        var player = GameObject.FindGameObjectWithTag(playerTag);
        _actorTransform = player.transform;
    }

    private void Update()
    {
        var playerPosition = _actorTransform.position;
        var position = transform.position;
        var toPlayer = playerPosition - position;

        var right = _actorController.Rb.transform.right;

        var angle = Vector3.SignedAngle(right, toPlayer, Vector3.forward);
        var angleAbs = Mathf.Abs(angle);

        if (angleAbs > angleThreshold)
        {
            _actorController.SetRotationInput(Mathf.Sign(angle));
        }
        else
        {
            _actorController.SetRotationInput(0f);
        }

        var distanceToPlayerSqr = toPlayer.sqrMagnitude;
        
        if (distanceToPlayerSqr > accelerationRange * accelerationRange && angleAbs < 40f)
        {
            _thrustMode = Color.green;
            _actorController.SetThrustActive(true);
        }
        else if (transform.position.y < waterLevel && _actorController.Rb.rotation is > 20f and < 160f)
        {
            _thrustMode = Color.blue;
            _actorController.SetThrustActive(true);
        }
        else if (distanceToPlayerSqr < closeupRange * closeupRange)
        {
            _thrustMode = Color.magenta;
            _actorController.SetThrustActive(true);
        }
        else
        {
            _thrustMode = Color.red;
            _actorController.SetThrustActive(false);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, accelerationRange);
        Gizmos.DrawWireSphere(transform.position, closeupRange);
        Gizmos.color = _thrustMode;
        Gizmos.DrawWireSphere(transform.position, 1f);
    }
}
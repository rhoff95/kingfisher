using Scripts;
using UnityEngine;

[RequireComponent(typeof(ActorController))]
public class BeeController : MonoBehaviour
{
    private ActorController _actorController;
    private Transform _actorTransform;

    [TagSelector] public string playerTag;

    [Range(0f, 90f)] public float angleThreshold;

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

        if (Mathf.Abs(angle) > angleThreshold)
        {
            _actorController.SetRotationInput(Mathf.Sign(angle));
        }
        else
        {
            _actorController.SetRotationInput(0f);
        }
    }
}
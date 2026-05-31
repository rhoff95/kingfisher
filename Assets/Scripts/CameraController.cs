using Scripts;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Transform _playerTransform;
    private Vector3 _currentVelocity;

    public float leadDistance;
    public float zOffset = -10;
    [Range(0f, 10f)] public float smoothTime;
    [TagSelector] public string playerTag;

    private void Awake()
    {
        _playerTransform = GameObject.FindGameObjectWithTag(playerTag).transform;
    }

    private void FixedUpdate()
    {
        var targetPosition = _playerTransform.position + new Vector3(0f, 0f, zOffset) +
                             _playerTransform.right * leadDistance;

        transform.position = Vector3.SmoothDamp(
            transform.position, targetPosition, ref _currentVelocity, smoothTime
        );
    }
}
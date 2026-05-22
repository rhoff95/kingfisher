using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ActorController : MonoBehaviour
{
    #region Public

    public ActorProperties properties;
    public Vector2 initialVelocity;

    [Header("Debug")]
    public bool disableGravity;

    public bool disableInitialVelocity;

    #endregion

    #region Private

    private Rigidbody2D _rb;

    private Vector2 _linearVelocity;
    private float _direction;

    // Inputs
    private bool _thrustActive;
    private float _rotationInput;

    #endregion

    public Rigidbody2D Rb => _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();

        if (properties == null)
        {
            throw new Exception($"Properties is null for {name}");
        }
    }

    private void Start()
    {
        if (!disableInitialVelocity)
        {
            _rb.linearVelocity = initialVelocity;
        }
    }

    private void Update()
    {
        _rb.rotation = _direction;
    }

    private void FixedUpdate()
    {
        var rotationSpeed = _thrustActive ? properties.rotationSpeedThrust : properties.rotationSpeedNoThrust;

        _direction += Time.deltaTime * _rotationInput * rotationSpeed;
        _direction %= 360;

        var radians = _direction * Mathf.Deg2Rad;
        var direction = new Vector2(Mathf.Cos(radians), Mathf.Sin(radians));

        // Thrust based on input
        if (_thrustActive)
        {
            _rb.linearVelocity = Vector2.SmoothDamp(
                _rb.linearVelocity,
                direction.normalized * properties.thrustAcceleration,
                ref _linearVelocity,
                properties.linearVelocitySmoothTime
            );
        }
        // Downward gravity if no thrust and above horizon
        else if (transform.position.y > 0f && !disableGravity)
        {
            _rb.linearVelocity += Time.fixedDeltaTime * new Vector2(0f, -properties.gravityAcceleration);
        }

        if (transform.position.y < 0f)
        {
            if (_rb.linearVelocity.y > 0f)
            {
                _rb.linearVelocity += Time.fixedDeltaTime * new Vector2(0f, properties.buoyancyAcceleration);
            }
            else
            {
                _rb.linearVelocity += Time.fixedDeltaTime * new Vector2(0f, properties.buoyancyAcceleration);
            }
        }

        if (_rb.linearVelocity.magnitude > properties.maxSpeed)
        {
            _rb.linearVelocity = _rb.linearVelocity.normalized * properties.maxSpeed;
        }
    }

    private void OnDrawGizmos()
    {
        if (_rb != null)
        {
            Gizmos.DrawRay(transform.position, _rb.linearVelocity);
        }
    }

    public void SetThrustActive(bool thrustActive)
    {
        _thrustActive = thrustActive;
    }

    public void SetRotationInput(float rotationInput)
    {
        _rotationInput = rotationInput;
    }
}
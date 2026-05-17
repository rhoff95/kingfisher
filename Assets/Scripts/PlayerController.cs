using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour, PlayerActions.IGameplayActions
{
    private PlayerActions _playerActions;
    private PlayerActions.GameplayActions _gameplayActions;

    private Rigidbody2D _rb;

    private float _thrustInput;
    private float _rotateInput;

    private float _direction = 0f;

    public GameObject vfx;
    public ParticleSystem thrustPfx;

    public Vector2 initialVelocity;
    
    [Header("Movement")]
    [Range(0f, 100f)] public float thrustAcceleration;
    [Range(0f, 100f)] public float gravityAcceleration;
    [Range(0f, 100f)] public float buoyancyAccelerationGoingDown;
    [Range(0f, 100f)] public float buoyancyAccelerationGoingUp;
    [Range(0f, 100f)] public float maxSpeed;
    
    [Header("Rotation")]
    [Range(0f, 500f)] public float rotationSpeedNoThrust;
    [Range(0f, 500f)] public float rotationSpeedThrust;

    private void Awake()
    {
        _playerActions = new PlayerActions();
        _gameplayActions = _playerActions.Gameplay;
        _gameplayActions.AddCallbacks(this);

        _rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        _rb.linearVelocity = initialVelocity;
    }

    private void OnDestroy()
    {
        _playerActions.Dispose();
    }

    private void OnEnable()
    {
        _gameplayActions.Enable();
    }

    private void OnDisable()
    {
        _gameplayActions.Disable();
    }

    #region Interface implementation of PlayerActions.IGameplayActions

    public void OnMoveForward(InputAction.CallbackContext context)
    {
        var value = context.ReadValueAsButton();

        if (value)
        {
            _thrustInput = 1f;
            thrustPfx.Play();
        }
        else
        {
            _thrustInput = 0f;
            thrustPfx.Stop();
        }
    }

    public void OnTurn(InputAction.CallbackContext context)
    {
        var value = context.ReadValue<float>();

        _rotateInput = value;
    }

    #endregion

    private void Update()
    {

        vfx.transform.rotation = Quaternion.Euler(-_direction, 90, _direction);
    }

    private void FixedUpdate()
    {
        var rotationSpeed = _thrustInput > 0f ? rotationSpeedThrust : rotationSpeedNoThrust;
        
        _direction += Time.deltaTime * _rotateInput * rotationSpeed;
        _direction %= 360;
        
        var radians = _direction * Mathf.Deg2Rad;
        var direction = new Vector2(Mathf.Cos(radians), Mathf.Sin(radians));

        // Thrust based on input
        if (_thrustInput > 0f)
        {
            _rb.linearVelocity += direction.normalized * (Time.fixedDeltaTime * (_thrustInput * thrustAcceleration));
        }
        // Downward gravity if no thrust and above horizon
        else if (transform.position.y > 0f)
        {
            _rb.linearVelocity += Time.fixedDeltaTime * new Vector2(0f, -gravityAcceleration);
        }

        if (transform.position.y < 0f)
        {
            if (_rb.linearVelocity.y > 0f)
            {
                _rb.linearVelocity += Time.fixedDeltaTime * new Vector2(0f, buoyancyAccelerationGoingUp);
            }
            else
            {
                _rb.linearVelocity += Time.fixedDeltaTime * new Vector2(0f, buoyancyAccelerationGoingDown);
            }
        }

        if (_rb.linearVelocity.magnitude > maxSpeed)
        {
            _rb.linearVelocity = _rb.linearVelocity.normalized * maxSpeed;
        }
    }

    private void OnDrawGizmos()
    {
        if (_rb != null)
        {
            Gizmos.DrawRay(transform.position, _rb.linearVelocity);
        }
    }
}
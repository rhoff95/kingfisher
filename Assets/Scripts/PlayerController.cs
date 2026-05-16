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

    [Range(0f, 10f)] public float maxSpeed;
    [Range(0f, 10f)] public float maxRotationSpeed;

    private void Awake()
    {
        _playerActions = new PlayerActions();
        _gameplayActions = _playerActions.Gameplay;
        _gameplayActions.AddCallbacks(this);

        _rb = GetComponent<Rigidbody2D>();
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
        }
        else
        {
            _thrustInput = 0f;
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
        var radians = _direction * Mathf.Deg2Rad;
        var direction = new Vector2(Mathf.Cos(radians), Mathf.Sin(radians));

        _rb.linearVelocity = direction.normalized * (_thrustInput * maxSpeed);

        _direction += _rotateInput * maxRotationSpeed;
        _direction %= 360;

        vfx.transform.rotation = Quaternion.Euler(-_direction, 90, _direction);
    }
}
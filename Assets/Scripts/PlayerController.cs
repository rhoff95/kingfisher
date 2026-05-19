using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(ActorController))]
public class PlayerController : MonoBehaviour, PlayerActions.IGameplayActions
{
    private PlayerActions _playerActions;
    private PlayerActions.GameplayActions _gameplayActions;
    private ActorController _actorController;

    public ParticleSystem thrustPfx;

    private void Awake()
    {
        _playerActions = new PlayerActions();
        _gameplayActions = _playerActions.Gameplay;
        _gameplayActions.AddCallbacks(this);
        _actorController = GetComponent<ActorController>();
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
            _actorController.SetThrustActive(true);
            thrustPfx.Play();
        }
        else
        {
            _actorController.SetThrustActive(false);
            thrustPfx.Stop();
        }
    }

    public void OnTurn(InputAction.CallbackContext context)
    {
        var value = context.ReadValue<float>();
        _actorController.SetRotationInput(value);
    }

    #endregion
}
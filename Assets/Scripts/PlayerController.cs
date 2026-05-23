using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(ActorController))]
public class PlayerController : MonoBehaviour, PlayerActions.IGameplayActions
{
    private PlayerActions _playerActions;
    private PlayerActions.GameplayActions _gameplayActions;
    private ActorController _actorController;
    
    public Transform cameraTransform;
    public SpriteRenderer thrustSpriteRenderer;

    private void Awake()
    {
        _playerActions = new PlayerActions();
        _gameplayActions = _playerActions.Gameplay;
        _gameplayActions.AddCallbacks(this);
        _actorController = GetComponent<ActorController>();
        thrustSpriteRenderer.enabled = false;
    }
    
    private void Update()
    {
        cameraTransform.rotation = Quaternion.Euler(0f, 0f, 0f);//-2f * _actorController._rb.rotation);
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
            thrustSpriteRenderer.enabled = true;
        }
        else
        {
            _actorController.SetThrustActive(false);
            thrustSpriteRenderer.enabled = false;
        }
    }

    public void OnTurn(InputAction.CallbackContext context)
    {
        var value = context.ReadValue<float>();
        _actorController.SetRotationInput(value);
    }
    
    public void OnRestart(InputAction.CallbackContext context)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    #endregion
}
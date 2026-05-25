using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Actors
{
    [RequireComponent(typeof(Actor), typeof(ProjectileShooter))]
    public class Player : MonoBehaviour, PlayerActions.IGameplayActions
    {
        private PlayerActions _playerActions;
        private PlayerActions.GameplayActions _gameplayActions;
        private Actor _actor;
        private ProjectileShooter _projectileShooter;

        public SpriteRenderer thrustSpriteRenderer;

        private void Awake()
        {
            // Inputs
            _playerActions = new PlayerActions();
            _gameplayActions = _playerActions.Gameplay;
            _gameplayActions.AddCallbacks(this);

            // Components
            _actor = GetComponent<Actor>();
            _projectileShooter = GetComponent<ProjectileShooter>();

            // Initialization
            thrustSpriteRenderer.enabled = false;
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
                _actor.SetThrustActive(true);
                thrustSpriteRenderer.enabled = true;
            }
            else
            {
                _actor.SetThrustActive(false);
                thrustSpriteRenderer.enabled = false;
            }
        }

        public void OnTurn(InputAction.CallbackContext context)
        {
            var value = context.ReadValue<float>();
            _actor.SetRotationInput(value);
        }

        public void OnRestart(InputAction.CallbackContext context)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void OnFire(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _actor.StartFiring();
            }
            else if (context.canceled)
            {
                _actor.StopFiring();
            }
        }

        #endregion
    }
}
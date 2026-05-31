using System;
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

        public Animator thrustAnimator;
        public float maxHealthMaskSize;
        public Transform healthMask;
        public float healthRegenerationDelay;

        private static readonly int Active = Animator.StringToHash("active");

        private bool _regeneratingHealth = true;
        private float _regeneratingHealthTimer = 0f;

        private void Awake()
        {
            // Inputs
            _playerActions = new PlayerActions();
            _gameplayActions = _playerActions.Gameplay;
            _gameplayActions.AddCallbacks(this);

            // Components
            _actor = GetComponent<Actor>();
            _actor.OnHitCallback = () =>
            {
                _regeneratingHealth = false;
                _regeneratingHealthTimer = healthRegenerationDelay;
            };
        }

        private void Update()
        {
            var healthPart = (float)_actor.Health / Actor.MaxHealth;
            healthMask.localScale = Vector3.one * (healthPart * maxHealthMaskSize);
            Debug.Log(
                $"{_actor.Health} /  {Actor.MaxHealth} = {healthPart:F2} => {(healthPart * maxHealthMaskSize):F2}");

            if (!_regeneratingHealth)
            {
                _regeneratingHealthTimer -= Time.deltaTime;
                _regeneratingHealthTimer = Mathf.Max(_regeneratingHealthTimer, 0f);
            }

            if (_regeneratingHealthTimer <= 0f)
            {
                _regeneratingHealth = true;
            }

            if (_regeneratingHealth)
            {
                _actor.AddHealth(1);
            }
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
                thrustAnimator.SetBool(Active, true);
            }
            else
            {
                _actor.SetThrustActive(false);
                thrustAnimator.SetBool(Active, false);
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
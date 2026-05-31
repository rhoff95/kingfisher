using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Actors
{
    [RequireComponent(typeof(Actor), typeof(ProjectileShooter))]
    public class Player : MonoBehaviour, InputActions.IPlaneActions
    {
        private InputActions _inputActions;
        private InputActions.PlaneActions _planeActions;
        private Actor _actor;
        private bool _hitOverlayFlash;
        public float healthRegenerationRate;
       [Range(0f, 1f)] public float healthOverlayCutoff;
        
        public Animator thrustAnimator;
        public float maxHealthMaskSize;
        public Transform healthMask;
        public SpriteRenderer healthOverlay;
        public Color healthDanger;
        public Color healthRecover;

        private static readonly int Active = Animator.StringToHash("active");

        private void Awake()
        {
            // Inputs
            _inputActions = new InputActions();
            
            _planeActions = _inputActions.Plane;
            _planeActions.AddCallbacks(this);

            // Components
            _actor = GetComponent<Actor>();
            _actor.OnHitCallback = () =>
            {
                _hitOverlayFlash = true;
                healthOverlay.color = healthDanger;
            };
            _actor.enabled = false;
        }

        private void Update()
        {
            var healthPart = _actor.Health / Actor.MaxHealth;
            var adjustedHealthPart = healthPart > healthOverlayCutoff ? 1f : healthPart;
            
            healthMask.localScale = Vector3.one * (adjustedHealthPart * maxHealthMaskSize);

            if (_hitOverlayFlash)
            {
                _hitOverlayFlash = false;
            }
            else
            {
                healthOverlay.color = healthRecover;
            }

            if (!_actor.IsFiring())
            {
                _actor.AddHealth(healthRegenerationRate * Time.deltaTime);
            }
        }

        private void OnDestroy()
        {
            _inputActions.Dispose();
        }

        private void OnEnable()
        {
            _planeActions.Enable();
        }

        private void OnDisable()
        {
            _planeActions.Disable();
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
            Debug.Log("On Start Game");
            GameManager.Instance.StartGame();
            _actor.enabled = true;
            
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
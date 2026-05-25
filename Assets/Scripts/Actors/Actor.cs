using System;
using UnityEngine;

namespace Actors
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Actor : MonoBehaviour
    {
        #region Public

        public ActorProperties properties;
        public Vector2 initialVelocity;
        [Range(0f, 5f)] public float fireDelay;

        [Header("Debug")]
        public bool disableGravity;

        public bool disableInitialVelocity;

        #endregion

        #region Private

        private Rigidbody2D _rb;
        private ProjectileShooter _projectileShooter;

        private Vector2 _linearVelocity;
        private float _direction;

        // Inputs
        private bool _thrustActive;
        private float _rotationInput;
        private bool _isFiring = false;
        private float _timeToNextFire = 0f;

        #endregion

        public Rigidbody2D Rb => _rb;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _projectileShooter = GetComponent<ProjectileShooter>();

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

            _direction = _rb.rotation;
        }

        private void Update()
        {
            _timeToNextFire -= Time.deltaTime;
            
            if (_isFiring)
            {
                if (_timeToNextFire <= 0f)
                {
                    FireOnce();
                    _timeToNextFire += fireDelay;
                } 
            }

            if (!_isFiring)
            {
                _timeToNextFire = Mathf.Max(_timeToNextFire, 0f);
            }
        }

        private void FixedUpdate()
        {
            _rb.rotation = _direction;

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
                    direction.normalized * properties.maxSpeed,
                    ref _linearVelocity,
                    properties.linearVelocitySmoothTime
                );
            }
            // Downward gravity if no thrust and above horizon

            if (!disableGravity)
            {
                var gravityForce = properties.gravityAcceleration * (_thrustActive ? 0.15f : 1f);
                
                if (transform.position.y > 0f)
                {
                    _rb.linearVelocity += Time.deltaTime * new Vector2(0f, -gravityForce);
                }
            }

            if (transform.position.y < 0f)
            {
                if (_rb.linearVelocity.y > 0f)
                {
                    _rb.linearVelocity += Time.deltaTime * new Vector2(0f, properties.buoyancyAcceleration);
                }
                else
                {
                    _rb.linearVelocity += Time.deltaTime * new Vector2(0f, properties.buoyancyAcceleration);
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

        public void StartFiring()
        {
            _isFiring = true;
        }

        public void StopFiring()
        {
            _isFiring = false;
        }

        public void FireOnce()
        {
            if (_projectileShooter != null)
            {
                _projectileShooter.Fire(transform.position, _direction);
            }
        }

        public void ApplyDamage(Projectile projectile)
        {
            Debug.Log($"{name} has been damaged by {projectile.name}!");
        }
    }
}
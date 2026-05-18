using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ActorController : MonoBehaviour
{
    #region Public
    
    [Header("Visuals")]
    public GameObject vfx;
    
    [Header("Movement")]
    public Vector2 initialVelocity;
    [Range(0f, 1f)]public float linearVelocitySmoothTime;
    [Range(0f, 100f)] public float thrustAcceleration;
    [Range(0f, 100f)] public float gravityAcceleration;
    [Range(0f, 100f)] public float buoyancyAcceleration;
    [Range(0f, 100f)] public float maxSpeed;
    
    [Header("Rotation")]
    [Range(0f, 500f)] public float rotationSpeedNoThrust;
    [Range(0f, 500f)] public float rotationSpeedThrust;
    
    #endregion
    
    #region Private

    private Rigidbody2D _rb;
    private Vector2 _linearVelocity;
    private float _direction;

    // Inputs
    private bool _thrustActive;
    private float _rotationInput;

    #endregion
    
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }
    
    private void Start()
    {
        _rb.linearVelocity = initialVelocity;
    }
    
    private void Update()
    {

        vfx.transform.rotation = Quaternion.Euler(-_direction, 90, _direction);
    }
    
    private void FixedUpdate()
    {
        var rotationSpeed = _thrustActive ? rotationSpeedThrust : rotationSpeedNoThrust;
        
        _direction += Time.deltaTime * _rotationInput * rotationSpeed;
        _direction %= 360;
        
        var radians = _direction * Mathf.Deg2Rad;
        var direction = new Vector2(Mathf.Cos(radians), Mathf.Sin(radians));

        // Thrust based on input
        if (_thrustActive)
        {
            // _rb.linearVelocity += direction.normalized * (Time.fixedDeltaTime * (_thrustInput * thrustAcceleration));
            _rb.linearVelocity = Vector2.SmoothDamp(
                _rb.linearVelocity,
                direction.normalized * thrustAcceleration,
                ref _linearVelocity,
                linearVelocitySmoothTime
            );
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
                _rb.linearVelocity += Time.fixedDeltaTime * new Vector2(0f, buoyancyAcceleration);
            }
            else
            {
                _rb.linearVelocity += Time.fixedDeltaTime * new Vector2(0f, buoyancyAcceleration);
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

    public void SetThrustActive(bool thrustActive)
    {
        _thrustActive = thrustActive;
    }
    
    public void SetRotationInput(float rotationInput)
    {
        _rotationInput = rotationInput;
    }
}
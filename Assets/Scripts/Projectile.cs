using System;
using Actors;
using UnityEngine;

public enum Team
{
    Player,
    Enemies
}

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class Projectile : MonoBehaviour
{
    private float _direction;
    private float _speed;
    private Team _team;
    private int _damage;

    private Rigidbody2D _rb;
    private Collider2D _collider;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
    }

    private void Start()
    {
        _rb.rotation = _direction;
        var directionRad = _direction * Mathf.Deg2Rad;
        _rb.linearVelocity = new Vector3(Mathf.Cos(directionRad), Mathf.Sin(directionRad), 0f) * _speed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var damagable = other.attachedRigidbody.GetComponentInParent<IDamagable>();
        if (damagable == null)
        {
            Destroy(gameObject);
            return;
        }

        damagable.ApplyDamage(_damage);

        // Create explosion FX

        Destroy(gameObject);
    }

    public void SetDirection(float direction)
    {
        _direction = direction;
    }

    public void SetSpeed(float speed)
    {
        _speed = speed;
    }

    public void SetTeam(Team team)
    {
        _team = team;
    }
    
    public void SetDamage(int damage)
    {
        _damage = damage;
    }
}
using System;
using Fusion;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private PlayerRef _playerRef;
    private Unit _shooter;
    private Transform _target;
    private float _speed;
    private int _damage;
    private bool _isLaunched = false;
    private Rigidbody _rb;
    private readonly float _threshold = 1f;


    public void Shoot(Transform target, float speed, int damage, PlayerRef playerRef, Unit shooter)
    {
        _playerRef = playerRef;
        _shooter = shooter;
        _target = target;
        _speed = speed;
        _damage = damage;
        _isLaunched = true;
        _rb = GetComponent<Rigidbody>();
    }


    private void Update()
    {
        if (_isLaunched)
        {
            if (_target != null)
            {
                Vector3 position = _target.position + new Vector3(0, 2, 0);
                if (Vector3.Distance(transform.position, position) < _threshold)

                {
                    Unit unit = _target.GetComponent<Unit>();
                    unit.TakeDamage(_damage, _playerRef,_shooter);
                    Destroy(gameObject);
                }

                Quaternion lookRotation =
                    Quaternion.LookRotation((position - transform.position).normalized);
                transform.rotation = lookRotation;
                _rb.velocity = (position - transform.position).normalized * _speed;
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
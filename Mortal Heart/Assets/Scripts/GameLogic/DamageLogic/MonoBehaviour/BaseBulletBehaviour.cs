using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using System;
using UniRx;

[RequireComponent(typeof(Rigidbody))]
public class BaseBulletBehaviour : HitCollider
{
    public float startSpeed = 5f;

    [Header("Optional")] public GameObject gojEfxDestroyBulelt;

    private bool isShooted;

    public Rigidbody m_rigidbody
    {
        get
        {
            if (_rigidbody == null)
            {
                _rigidbody = GetComponent<Rigidbody>();

                if (_rigidbody == null)
                {
                    _rigidbody = gameObject.AddComponent<Rigidbody>();
                    _rigidbody.isKinematic = true;
                }
            }

            return _rigidbody;
        }
    }

    private Rigidbody _rigidbody;

    private void OnEnable()
    {
        isShooted = false;
    }

    public virtual void Shoot(Vector3 direct)
    {
        direction = direct.normalized;
        transform.rotation = Quaternion.LookRotation(Vector3.forward, direction);
        isShooted = true;
    }

    private void Update()
    {
        if (isShooted)
            transform.position += direction * startSpeed * Time.deltaTime;
    }

    public override void OnTriggerEnter(Collider coll)
    {
        if ((effectLayer.value & (1 << coll.gameObject.layer)) == 0) return;

        var health = coll.GetComponent<IHeath>();
        if (health != null)
        {
            damageType.Damage(coll.transform, _damage);
            Despawn();
        }
    }

    public override void OnTriggerStay(Collider coll)
    {
        if ((shieldLayer.value & (1 << coll.gameObject.layer)) == 0) return;
        {
            Despawn();
            _disposable?.Dispose();
            return;
        }
    }

    [HideInInspector] public Vector3 direction;
    public virtual void Despawn(bool culling = false)
    {
        if (gojEfxDestroyBulelt != null)
            SimplePool.Spawn(gojEfxDestroyBulelt, transform.position, transform.rotation);

        SimplePool.Despawn(gameObject);
    }
}
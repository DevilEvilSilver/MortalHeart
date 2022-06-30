using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;

public class HitCollider : MonoBehaviour
{
    [SerializeField] private LayerMask effectLayer;
    private float _damage;

    private void Awake()
    {
        _damage = 0;
    }

    public virtual void Init(float damage, float activeTime)
    {
        _damage = damage;

        Observable.Timer(TimeSpan.FromSeconds(activeTime)).Subscribe(_ =>
        {
            gameObject.SetActive(false);
        });
    }

    public virtual void OnTriggerEnter(Collider coll)
    {
        if ((effectLayer.value & (1 << coll.gameObject.layer)) == 0) return;

        var health = coll.GetComponent<IHeath>();
        if (health != null)
        {
            health.TakeDamage(_damage);
        }
    }
}

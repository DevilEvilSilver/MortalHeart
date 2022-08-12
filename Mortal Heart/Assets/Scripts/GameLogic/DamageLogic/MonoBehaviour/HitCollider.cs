using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using System;
using UniRx;

public class HitCollider : SerializedMonoBehaviour
{
    [SerializeField] protected LayerMask effectLayer;
    [SerializeField] protected LayerMask shieldLayer;
    [SerializeField] protected IDamage damageType;

    protected float _damage;
    protected IDisposable _disposable;

    private void Awake()
    {
        _damage = 0;
    }

    public virtual void Init(float damage, float activeTime = -1f)
    {
        _damage = damage;

        if (activeTime > 0f)
            _disposable = Observable.Timer(TimeSpan.FromSeconds(activeTime)).Subscribe(_ =>
            {
                gameObject.SetActive(false);
            });
    }

    public virtual void OnTriggerEnter(Collider coll)
    {
        if ((effectLayer.value & (1 << coll.gameObject.layer)) == 0) return;

        //if ((shieldLayer.value & (1 << coll.gameObject.layer)) != 0)
        //{
        //    Debug.Log("Shielded");
        //    gameObject.SetActive(false);
        //    _disposable?.Dispose();
        //    return;
        //}

        var health = coll.GetComponent<IHeath>();
        if (health != null)
        {
           damageType.Damage(coll.transform, _damage);
        }
    }

    public virtual void OnTriggerStay(Collider coll)
    {
        if ((shieldLayer.value & (1 << coll.gameObject.layer)) == 0) return;
        {
            //Debug.Log("Shielded");
            gameObject.SetActive(false);
            _disposable?.Dispose();
            return;
        }
    }
}

using System;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;
using Sirenix.OdinInspector;

public class CharacterSkillMelee : BaseAttackState
{
    [SerializeField] protected HitCollider hitCollider;
    [SerializeField] protected bool setDamage;
    [ShowIf("setDamage")]
    [SerializeField] protected float damage;

    private IDisposable _disposable;

    public override void OnEnter()
    {
        base.OnEnter();

        _disposable = Observable.Timer(TimeSpan.FromSeconds(delay)).Subscribe(_ =>
        {
            hitCollider.gameObject.SetActive(true);
            if (setDamage)
            {
                hitCollider.Init(damage, active);
            }
        });
    }

    public override void OnExit()
    {
        base.OnExit();

        _disposable?.Dispose();
    }

    public override void OnStop()
    {
        base.OnStop();

        _disposable?.Dispose();
    }
}
using UnityEngine;
using System;
using UniRx;
using Sirenix.OdinInspector;

public class LukeSuperSkill : BaseAttackState
{
    [SerializeField] private UpgradeData superUpgrade;
    [SerializeField] protected HitCollider hitCollider;
    [SerializeField] protected bool setDamage;
    [ShowIf("setDamage")]
    [SerializeField] protected float damage;

    public override void OnEnter()
    {
        base.OnEnter();
        (actorController as LukeController).ToggleInvulnerable(true);
        if (superUpgrade != null && superUpgrade.level > 0)
        {
            Observable.Timer(TimeSpan.FromSeconds(delay)).Subscribe(_ =>
            {
                hitCollider.gameObject.SetActive(true);
                if (setDamage)
                {
                    hitCollider.Init(damage, active);
                }
            });
        }

    }

    public override void OnFixedUpdate()
    {
    }

    public override void OnExit()
    {
        base.OnExit();
        (actorController as LukeController).ActivateSuper();
    }
}
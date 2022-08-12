using UnityEngine;
using System;
using UniRx;
using Sirenix.OdinInspector;

public class LukeSuperSkill : BaseAttackState
{
    [SerializeField] private UpgradeData superUpgrade;
    [SerializeField] protected IDamage superDamage;
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
                superDamage.DamageOnPosition(actorController.transform.position, damage);
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
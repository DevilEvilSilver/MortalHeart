using System;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;
using Sirenix.OdinInspector;

public class LukeMeleeSkill : CharacterSkillMelee
{
    [SerializeField] private UpgradeData boostDamage;

    private IDisposable _disposable;

    public override void OnEnter()
    {
        if (boostDamage != null && boostDamage.level > 0)
            damage *= 1.2f;

        base.OnEnter();
    }
}
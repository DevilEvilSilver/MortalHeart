using System;
using System.Collections;
using UniRx;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;

public class EnemySkillMeleeDouble : EnemySkillMelee
{
    [SerializeField] protected HitCollider hitCollider2;
    [SerializeField] protected bool setDamage2;
    [ShowIf("setDamage2")]
    [SerializeField] private float damage2;
    [SerializeField] protected float delay2;
    [SerializeField] protected float active2;

    protected IDisposable _disposable2;

    protected override IEnumerator StartAttack()
    {
        if (_player == null)
        {
            isLock = false;
            actorController.ChangeToIdle(overrideIdleTime);

            yield break;
        }

        while ((actorController.transform.position - _player.position).magnitude > attackRange)
        {
            actorController.MoveTo(_player.position + _playerPosOffset, actorController.baseSpeed);
            yield return new WaitForFixedUpdate();
        }

        actorController.Agent.isStopped = true;
        actorController.transform.DOLookAt(_player.position, 0.1f).OnComplete(() =>
        {
            actorController.animator.CrossFadeInFixedTime(attackAnim, 0.2f);

            _disposable = Observable.Timer(TimeSpan.FromSeconds(delay)).Subscribe(_ =>
            {
                hitCollider.gameObject.SetActive(true);
                if (setDamage)
                {
                    hitCollider.Init(damage, active);
                }
            });

            _disposable2 = Observable.Timer(TimeSpan.FromSeconds(delay2)).Subscribe(_ =>
            {
                hitCollider2.gameObject.SetActive(true);
                if (setDamage2)
                {
                    hitCollider2.Init(damage2, active2);
                }
            });
        });   

        yield return new WaitForSeconds(duration);

        isLock = false;
        actorController.ChangeToIdle(overrideIdleTime);
    }

    public override void OnExit()
    {
        base.OnExit();

        _disposable2?.Dispose();
    }

    public override void OnStop()
    {
        base.OnStop();

        _disposable2?.Dispose();
    }
}
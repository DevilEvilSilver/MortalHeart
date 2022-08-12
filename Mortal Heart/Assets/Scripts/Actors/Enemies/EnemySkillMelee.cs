using System;
using System.Collections;
using UniRx;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;

public class EnemySkillMelee : BaseEnemyAttackState
{
    [ValueDropdown("AllAnimations")]
    [SerializeField] protected string moveAnim;
    [SerializeField] protected float attackRange;
    [SerializeField] protected float duration;

    [SerializeField] protected HitCollider hitCollider;
    [SerializeField] protected bool setDamage;
    [ShowIf("setDamage")]
    [SerializeField] protected float damage;
    [SerializeField] protected float delay;
    [SerializeField] protected float active;

    protected IDisposable _disposable;
    protected IEnumerator _attackCoroutine;

    protected Transform _player;
    protected Vector3 _playerPosOffset;

    public override void OnEnter()
    {
        base.OnEnter();

        _player = UnityEngine.Object.FindObjectOfType<MainCharacterController>().transform;
        actorController.animator.CrossFadeInFixedTime(moveAnim, 0.2f);

        _attackCoroutine = StartAttack();
        actorController.StartCoroutine(_attackCoroutine);

        _playerPosOffset = new Vector3(UnityEngine.Random.Range(-attackRange / 2f, attackRange / 2f)
            , 0f
            , UnityEngine.Random.Range(-attackRange / 2f, attackRange / 2f));
    }

    protected virtual IEnumerator StartAttack()
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
        });   

        yield return new WaitForSeconds(duration);

        isLock = false;
        actorController.ChangeToIdle(overrideIdleTime);
    }

    public override void OnExit()
    {
        base.OnExit();

        actorController.Agent.isStopped = false;
        actorController.Stop();
        _disposable?.Dispose();
        if (_attackCoroutine != null)
            actorController.StopCoroutine(_attackCoroutine);
    }

    public override void OnStop()
    {
        base.OnStop();

        actorController.Agent.isStopped = false;
        actorController.Stop();
        _disposable?.Dispose();
        if (_attackCoroutine != null)
            actorController.StopCoroutine(_attackCoroutine);
    }
}
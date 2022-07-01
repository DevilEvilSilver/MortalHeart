using System;
using System.Collections;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;
using Sirenix.OdinInspector;

public class EnemySkillMelee : BaseEnemyAttackState
{
    [ValueDropdown("AllAnimations")]
    [SerializeField] private string moveAnim;

    [SerializeField] private HitCollider hitCollider;
    [SerializeField] private bool setDamage;
    [ShowIf("setDamage")]
    [SerializeField] private float damage;
    [SerializeField] private float attackRange;
    [SerializeField] private float duration;
    [SerializeField] private float delay;
    [SerializeField] private float active;

    private IDisposable _disposable;
    private Coroutine _attackCoroutine;

    private Transform _player;

    public override void OnEnter()
    {
        base.OnEnter();

        _player = UnityEngine.Object.FindObjectOfType<MainCharacterController>().transform;
        actorController.animator.Play(moveAnim);
        _attackCoroutine = actorController.StartCoroutine(StartAttack());
    }

    private IEnumerator StartAttack()
    {
        if (_player == null)
        {
            isLock = false;
            actorController.ChangeToIdle(overrideIdleTime);

            yield break;
        }

        while ((actorController.transform.position - _player.position).magnitude > attackRange)
        {
            actorController.Agent.SetDestination(_player.position);
            //actorController.Agent.speed = actorController.baseSpeed;
            yield return new WaitForFixedUpdate();
        }

        actorController.transform.LookAt(_player);
        actorController.animator.Play(attackAnim);

        _disposable = Observable.Timer(TimeSpan.FromSeconds(delay)).Subscribe(_ =>
        {
            hitCollider.gameObject.SetActive(true);
            if (setDamage)
            {
                hitCollider.Init(damage, active);
            }
        });

        yield return new WaitForSeconds(duration);

        isLock = false;
        actorController.ChangeToIdle(overrideIdleTime);
    }

    public override void OnExit()
    {
        base.OnExit();

        _disposable?.Dispose();
        if (_attackCoroutine != null)
            actorController.StopCoroutine(_attackCoroutine);
    }

    public override void OnStop()
    {
        base.OnStop();

        _disposable?.Dispose();
        if (_attackCoroutine != null)
            actorController.StopCoroutine(_attackCoroutine);
    }
}
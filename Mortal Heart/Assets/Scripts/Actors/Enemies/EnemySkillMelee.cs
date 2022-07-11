using System;
using System.Collections;
using UniRx;
using UnityEngine;
using DG.Tweening;
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
    private Vector3 _playerPosOffset;

    public override void OnEnter()
    {
        base.OnEnter();

        _player = UnityEngine.Object.FindObjectOfType<MainCharacterController>().transform;
        actorController.animator.Play(moveAnim);
        _attackCoroutine = actorController.StartCoroutine(StartAttack());

        _playerPosOffset = new Vector3(UnityEngine.Random.Range(-attackRange / 2f, attackRange / 2f)
            , 0f
            , UnityEngine.Random.Range(-attackRange / 2f, attackRange / 2f));
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
            actorController.MoveTo(_player.position + _playerPosOffset, actorController.baseSpeed);
            yield return new WaitForFixedUpdate();
        }

        actorController.Agent.isStopped = true;
        actorController.transform.DOLookAt(_player.position, 0.1f).OnComplete(() =>
        {
            actorController.animator.Play(attackAnim);

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
        _disposable?.Dispose();
        if (_attackCoroutine != null)
            actorController.StopCoroutine(_attackCoroutine);
    }

    public override void OnStop()
    {
        base.OnStop();

        actorController.Agent.isStopped = false;
        _disposable?.Dispose();
        if (_attackCoroutine != null)
            actorController.StopCoroutine(_attackCoroutine);
    }
}
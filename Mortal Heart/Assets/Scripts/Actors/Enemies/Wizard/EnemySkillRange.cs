using System;
using System.Collections;
using UniRx;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;

public class EnemySkillRange : BaseEnemyAttackState
{
    [ValueDropdown("AllAnimations")]
    [SerializeField] protected string moveAnim;
    [SerializeField] protected float attackRange;
    [SerializeField] protected float delay;
    [SerializeField] protected float delayEnd;

    [SerializeField] protected BaseBulletBehaviour bullet;
    [SerializeField] protected Transform shootPos;
    [SerializeField] protected float bulletCount;
    [SerializeField] protected float fireRate;
    [SerializeField] protected float damage;

    protected IDisposable _disposable;
    protected IEnumerator _attackCoroutine;

    protected Transform _player;
    private float _timeSinceLastShoot;
    protected bool _isStateRunning;

    public override void OnEnter()
    {
        base.OnEnter();
        _isStateRunning = true;
        _timeSinceLastShoot = 0f;
        _player = UnityEngine.Object.FindObjectOfType<MainCharacterController>().transform;

        _attackCoroutine = StartAttack();
        actorController.StartCoroutine(_attackCoroutine);
    }

    protected virtual IEnumerator StartAttack()
    {
        if (_player == null)
        {
            isLock = false;
            actorController.ChangeToIdle(overrideIdleTime);

            yield break;
        }

        actorController.Agent.isStopped = true;
        int currBullet = 0;
        actorController.transform.DOLookAt(_player.position, 0.1f);
        yield return new WaitForSeconds(0.1f);

        actorController.animator.CrossFadeInFixedTime(attackAnim, 0.2f);

        yield return new WaitForSeconds(delay);

        while (_isStateRunning && currBullet < bulletCount)
        {
            if (Time.time > _timeSinceLastShoot + fireRate)
            {
                Shoot();
                currBullet++;
                _timeSinceLastShoot = Time.time;
            }
            yield return null;
        }

        yield return new WaitForSeconds(delayEnd);

        isLock = false;
        actorController.ChangeToIdle(overrideIdleTime);
    }

    protected virtual void Shoot()
    {
        var b = SimplePool.Spawn(bullet.gameObject, shootPos.position, Quaternion.identity);
        var comp = b.GetComponent<BaseBulletBehaviour>();
        comp.Init(damage, 2f);
        var dir = _player.position - shootPos.position;
        dir.y = 0f;
        comp.Shoot(dir);
    }

    public override void OnExit()
    {
        base.OnExit();
        _isStateRunning = false;

        actorController.Agent.isStopped = false;
        actorController.Stop();
        _disposable?.Dispose();
        if (_attackCoroutine != null)
            actorController.StopCoroutine(_attackCoroutine);
    }

    public override void OnStop()
    {
        base.OnStop();
        _isStateRunning = false;

        actorController.Agent.isStopped = false;
        actorController.Stop();
        _disposable?.Dispose();
        if (_attackCoroutine != null)
            actorController.StopCoroutine(_attackCoroutine);
    }
}
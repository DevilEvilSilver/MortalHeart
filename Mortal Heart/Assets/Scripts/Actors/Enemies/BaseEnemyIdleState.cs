using System;
using System.Collections;
using UniRx;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;

public class BaseEnemyIdleState : BaseEnemyState
{
    [ValueDropdown("AllAnimations")]
    [SerializeField] private string idleAnim;
    [SerializeField] private float outerRange;
    [SerializeField] private float innerRange;
    [SerializeField] private float speedMultiplier;
    //[SerializeField] private bool isTriggerInRange;
    //[ShowIf("setDamage")]
    //[SerializeField] private float triggerRange;

    private Transform _player;
    private Vector3 _playerPosOffset;
    private Vector3 _cachePos;

    private IEnumerator _idleCoroutine;

    private bool isStateRunning;

    public override void OnEnter()
    {
        base.OnEnter();
        actorController.animator.CrossFadeInFixedTime(idleAnim, 0.2f);
        isStateRunning = true;
        _cachePos = Vector3.zero;

        _player = UnityEngine.Object.FindObjectOfType<MainCharacterController>().transform;
        actorController.animator.Play(idleAnim);
        _idleCoroutine = StartIdle();
        actorController.StartCoroutine(_idleCoroutine);

        FindIdlePos();
    }

    private void FindIdlePos()
    {
        var x = UnityEngine.Random.Range(-1f, 1f) < 0
                   ? UnityEngine.Random.Range(-outerRange, -innerRange)
                   : UnityEngine.Random.Range(innerRange, outerRange);
        var z = UnityEngine.Random.Range(-1f, 1f) < 0
            ? UnityEngine.Random.Range(-outerRange, -innerRange)
            : UnityEngine.Random.Range(innerRange, outerRange);
        _playerPosOffset = new Vector3(x, 0f, z);
    }

    private IEnumerator StartIdle()
    {
        if (_player == null)
        {
            yield break;
        }

        actorController.MoveTo(_player.position + _playerPosOffset, actorController.baseSpeed * speedMultiplier);

        while (isStateRunning)
        {
            if (Vector3.Distance(actorController.transform.position, _cachePos) < 0.05f)
            {
                FindIdlePos();
                actorController.MoveTo(_player.position + _playerPosOffset
                    , actorController.baseSpeed * speedMultiplier);
            }
            _cachePos = actorController.transform.position;

            yield return new WaitForFixedUpdate();
        }
    }

    public override void OnExit()
    {
        base.OnExit();

        isStateRunning = false;
        actorController.Stop();
        if (_idleCoroutine != null)
            actorController.StopCoroutine(_idleCoroutine);
    }

    public override void OnStop()
    {
        base.OnStop();

        isStateRunning = false;
        actorController.Stop();
        if (_idleCoroutine != null)
            actorController.StopCoroutine(_idleCoroutine);
    }
}
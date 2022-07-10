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
    //[SerializeField] private bool isTriggerInRange;
    //[ShowIf("setDamage")]
    //[SerializeField] private float triggerRange;

    private Transform _player;
    private Vector3 _playerPosOffset;

    private Coroutine _idleCoroutine;

    private bool isStateRunning;

    public override void OnEnter()
    {
        base.OnEnter();
        actorController.animator.Play(idleAnim);
        isStateRunning = true;

        _player = UnityEngine.Object.FindObjectOfType<MainCharacterController>().transform;
        actorController.animator.Play(idleAnim);
        _idleCoroutine = actorController.StartCoroutine(StartIdle());

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

        while (isStateRunning)
        {
            actorController.MoveTo(_player.position + _playerPosOffset, actorController.baseSpeed / 3f);
            yield return new WaitForFixedUpdate();
        }
    }

    public override void OnExit()
    {
        base.OnExit();

        isStateRunning = false;
        if (_idleCoroutine != null)
            actorController.StopCoroutine(_idleCoroutine);
    }

    public override void OnStop()
    {
        base.OnStop();

        isStateRunning = false;
        if (_idleCoroutine != null)
            actorController.StopCoroutine(_idleCoroutine);
    }
}
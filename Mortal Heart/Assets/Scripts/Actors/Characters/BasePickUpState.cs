using Sirenix.OdinInspector;
using System;
using UnityEngine;
using UniRx;

public class BasePickUpState : BaseCharacterState
{
    [ValueDropdown("AllAnimations")]
    public string pickUpAnim;
    public float animDuration;
    public LayerMask itemLayer;
    public float range;

    private IDisposable _disposable;

    public override void OnEnter()
    {
        base.OnEnter();
        isLock = true;

        var items = Physics.OverlapSphere(actorController.transform.position, range, itemLayer);
        Debug.Log(items);
        if (items != null && items.Length > 0 && items[0].GetComponentInParent<ItemObject>().PickUpObject())
        {
            actorController.animator.CrossFadeInFixedTime(pickUpAnim, 0.2f);
            _disposable = Observable.Timer(TimeSpan.FromSeconds(animDuration)).Subscribe(_ =>
            {
                isLock = false;
                actorController.ChangeToIdle();
                return;
            });
        }
        else
        {
            isLock = false;
            actorController.ChangeToIdle();
        }
    }

    public override void OnExit()
    {
        base.OnExit();

        _disposable?.Dispose();
    }

    public override void OnStop()
    {
        base.OnStop();

        _disposable?.Dispose();
    }
}
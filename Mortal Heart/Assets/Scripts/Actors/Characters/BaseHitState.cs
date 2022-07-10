using Sirenix.OdinInspector;
using System;
using UniRx;

public class BaseHitState : BaseCharacterState
{
    [ValueDropdown("AllAnimations")]
    public string hitAnim;
    public float duration;

    private IDisposable _disposable;

    public override void OnEnter()
    {
        base.OnEnter();
        isLock = true;

        actorController.animator.CrossFadeInFixedTime(hitAnim, 0.2f);
        _disposable = Observable.Timer(TimeSpan.FromSeconds(duration)).Subscribe(_ =>
        {
            isLock = false;
            actorController.ChangeToIdle();
            return;
        });
    }

    public override void OnExit()
    {
        base.OnExit();

        _disposable?.Dispose();
    }
}
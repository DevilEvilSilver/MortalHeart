using Sirenix.OdinInspector;
using System;
using UniRx;

public class BaseUseItemState : BaseCharacterState
{
    [ValueDropdown("AllAnimations")]
    public string nonItemAnim;
    public float delayNonItemAnim;

    private InventoryItemData _currentItem;
    private IDisposable _disposable, _disposable_2;

    public override void OnEnter()
    {
        base.OnEnter();
        isLock = true;

        _currentItem = InventorySystem.Instance.GetCurrentItem();

        if (_currentItem == null)
        {
            actorController.animator.CrossFadeInFixedTime(nonItemAnim, 0.2f);
            _disposable = Observable.Timer(TimeSpan.FromSeconds(delayNonItemAnim)).Subscribe(_ =>
            {
                isLock = false;
                actorController.ChangeToIdle();
                return;
            });
        }
        else
        {
            actorController.animator.CrossFadeInFixedTime(_currentItem.anim.name, 0.2f);
            _disposable = Observable.Timer(TimeSpan.FromSeconds(_currentItem.delay)).Subscribe(_ =>
            {
                InventorySystem.Instance.UseItem();
            });
            _disposable_2 = Observable.Timer(TimeSpan.FromSeconds(_currentItem.anim.length)).Subscribe(_ =>
            {
                isLock = false;
                actorController.ChangeToIdle();
                return;
            });
        }
    }

    public override void OnExit()
    {
        base.OnExit();

        _disposable?.Dispose();
        _disposable_2?.Dispose();
    }
}
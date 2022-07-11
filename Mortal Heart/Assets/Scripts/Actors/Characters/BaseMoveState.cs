using UnityEngine.InputSystem;
using UnityEngine;
using Sirenix.OdinInspector;
using DG.Tweening;

public class BaseMoveState : BaseCharacterState
{
    [ValueDropdown("AllAnimations")]
    public string moveAnim;

    private Vector3 _direction;
    private float _speed;

    public override void OnEnter()
    {
        base.OnEnter();
        actorController.animator.CrossFadeInFixedTime(moveAnim, 0.2f);
        _direction = Vector3.zero;
        _speed = actorController.playerData.Speed;
    }

    public override void OnActionCallback(InputAction.CallbackContext ctx)
    {
        base.OnActionCallback(ctx);
        var direction = new Vector3(ctx.ReadValue<Vector2>().x, 0f, ctx.ReadValue<Vector2>().y);
        if (_direction == direction || direction == Vector3.zero)
            return;

        actorController.RigidBody.velocity = direction * _speed;
        actorController.transform.DOKill();
        var rot = Quaternion.LookRotation(direction, Vector3.up).eulerAngles;
        actorController.transform.DORotate(rot, 0.1f).SetEase(Ease.Linear);
        _direction = direction;
    }

    public override void OnExit()
    {
        base.OnExit();
        if (actorController != null)
            actorController.RigidBody.velocity = Vector2.zero;
    }
}
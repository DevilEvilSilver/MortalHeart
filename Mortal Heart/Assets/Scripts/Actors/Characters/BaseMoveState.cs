using UnityEngine.InputSystem;
using UnityEngine;
using Sirenix.OdinInspector;

public class BaseMoveState : BaseCharacterState
{
    [ValueDropdown("AllAnimations")]
    public string moveAnim;

    private Vector3 _direction;
    private float _speed;

    public override void OnEnter()
    {
        base.OnEnter();
        actorController.animator.Play(moveAnim);
        _direction = actorController.transform.forward;
        _speed = actorController.baseSpeed;
    }

    public override void OnActionCallback(InputAction.CallbackContext ctx)
    {
        base.OnActionCallback(ctx);
        _direction = new Vector3(ctx.ReadValue<Vector2>().x, 0f, ctx.ReadValue<Vector2>().y);
        actorController.RigidBody.velocity = _direction * _speed;
        Vector3 target = actorController.transform.position + _direction;
        actorController.transform.LookAt(target, Vector3.up);
    }

    public override void OnExit()
    {
        base.OnExit();
        actorController.RigidBody.velocity = Vector2.zero;
    }
}
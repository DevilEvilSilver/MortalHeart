using UnityEngine;
using UnityEngine.InputSystem;
using Sirenix.OdinInspector;

public class BaseAttackState : BaseCharacterState
{
    [ValueDropdown("AllAnimations")]
    public string attacckAnim;
    public bool isTimeBased;
    [ShowIf("isTimeBased")]
    public float duration;
    [ShowIf("isTimeBased")]
    public float delay;
    [ShowIf("isTimeBased")]
    public float active;

    private InputAction _inputAction;
    protected float _timeSinceInit;

    public override void OnEnter()
    {
        base.OnEnter();
        isLock = true;
        _timeSinceInit = 0f;

        actorController.animator.Play(attacckAnim);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        _timeSinceInit += Time.deltaTime;

        if (isTimeBased && _timeSinceInit > duration)
        {
            isLock = false;
            actorController.ChangeToIdle();
            return;
        }
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();

        if (isTimeBased && actorController.dodgeAction.IsPressed() && _timeSinceInit > delay + active)
        {
            // for combo chain mix dodge
            actorController.CancelAttackToDodge(this, _inputAction);
        }
    }

    public override void OnActionCallback(InputAction.CallbackContext ctx)
    {
        base.OnActionCallback(ctx);
        _inputAction = ctx.action;
    }

    public override void OnExit()
    {
        base.OnExit();
    }
}
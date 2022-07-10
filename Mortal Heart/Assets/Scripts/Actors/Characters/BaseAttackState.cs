using UnityEngine;
using UnityEngine.InputSystem;
using Sirenix.OdinInspector;

public class BaseAttackState : BaseCharacterState
{
    [ValueDropdown("AllAnimations")]
    public string attackAnim;
    public bool isComboChain;

    public bool isTimeBased;
    [ShowIf("isTimeBased")]
    public float duration;
    [ShowIf("isTimeBased")]
    public float delay;
    [ShowIf("isTimeBased")]
    public float active;

    protected InputAction.CallbackContext _ctx;
    protected float _timeSinceInit;
    protected bool _isDodgeCancel;

    public override void OnEnter()
    {
        base.OnEnter();
        isLock = true;
        _timeSinceInit = 0f;
        _isDodgeCancel = false;

        actorController.animator.CrossFadeInFixedTime(attackAnim, 0.2f);
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

        if (isComboChain && _timeSinceInit > delay + active)
        {
            // dodge
            if (actorController.dodgeAction.IsPressed())
            {
                // for combo chain mix dodge
                _isDodgeCancel = true;
                actorController.CancelAttackToDodge(_ctx);
            }

            // combo chain
            if (_ctx.action != null && _ctx.action.IsPressed())
            {
                actorController.ChainNextCombo(_ctx);
            }
        }
    }

    public override void OnActionCallback(InputAction.CallbackContext ctx)
    {
        base.OnActionCallback(ctx);
        _ctx = ctx;
    }

    public override void OnExit()
    {
        base.OnExit();

        if (isComboChain && !_isDodgeCancel)
            actorController.skillSetIndex = 0;
    }
}
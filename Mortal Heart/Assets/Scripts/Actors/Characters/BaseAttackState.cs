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
    protected bool _isComboChained;

    public override void OnEnter()
    {
        base.OnEnter();
        isLock = true;
        _timeSinceInit = 0f;
        _isDodgeCancel = false;
        _isComboChained = false;

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

    private void OnDodge(InputAction.CallbackContext ctx)
    {
        if (isComboChain && _timeSinceInit > delay + active)
        {
            // for combo chain mix dodge
            actorController.dodgeAction.performed -= OnDodge;
            _isDodgeCancel = true;
            actorController.CancelAttackToDodge(_ctx);
        }
    }

    private void OnComboChain(InputAction.CallbackContext ctx)
    {
        if (isComboChain && _timeSinceInit > delay + active)
        {
            _ctx.action.performed -= OnComboChain;
            _isComboChained = true;
            actorController.ChainNextCombo(_ctx);
        }
    }

    public override void OnActionCallback(InputAction.CallbackContext ctx)
    {
        base.OnActionCallback(ctx);
        _ctx = ctx;

        actorController.dodgeAction.performed += OnDodge;
        _ctx.action.performed += OnComboChain;
    }

    public override void OnExit()
    {
        base.OnExit();

        actorController.dodgeAction.performed -= OnDodge;
        _ctx.action.performed -= OnComboChain;

        if (isComboChain && !_isDodgeCancel && !_isComboChained)
            actorController.skillSetIndex = 0;
    }
}
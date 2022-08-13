using UnityEngine.InputSystem;
using UnityEngine;
using Sirenix.OdinInspector;

public class BaseDodgeState : BaseCharacterState
{
    [ValueDropdown("AllAnimations")]
    public string dodgeAnim;
    public float distance;
    public float duration;
    public float iframe;
    public float swtichDirectionTime;

    private Vector3 _direction;
    private bool isInitDir;
    private float _timeSinceInit;
    protected bool _isComboChained;
    internal InputAction.CallbackContext comboCtx;

    public override void OnEnter()
    {
        base.OnEnter();
        isLock = true;
        //actorController.ToggleInvulnerable(true);
        _timeSinceInit = 0f;
        isInitDir = false;
        _isComboChained = false;

        actorController.animator.CrossFadeInFixedTime(dodgeAnim, 0.2f);
        _direction = actorController.transform.forward;
        actorController.RigidBody.velocity = _direction * distance / duration;

        if (actorController.chainAttackAction != null)
        {
            actorController.chainAttackAction.performed += OnMixComboChain;
        }
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        _timeSinceInit += Time.deltaTime;

        if (_timeSinceInit > iframe)
        {
            //actorController.ToggleInvulnerable(false);
            if (_timeSinceInit > duration)
            {
                isLock = false;
                actorController.ChangeToIdle();
                return;
            }
        }
    }

    private void OnMixComboChain(InputAction.CallbackContext ctx)
    {
        if (_timeSinceInit > iframe)
        {
            // for combo chain mix dodge
            actorController.chainAttackAction.performed -= OnMixComboChain;
            _isComboChained = true;
            actorController.GoToNextState(comboCtx, true);
        }
    }

    public override void OnActionCallback(InputAction.CallbackContext ctx)
    {
        base.OnActionCallback(ctx);
        if (!isInitDir && _timeSinceInit < swtichDirectionTime)
        {
            _direction = new Vector3(ctx.ReadValue<Vector2>().x, 0f, ctx.ReadValue<Vector2>().y);
            actorController.RigidBody.velocity = _direction * distance / duration;
            Vector3 target = actorController.transform.position + _direction;
            actorController.transform.LookAt(target, Vector3.up);
            isInitDir = true;
        }
    }

    public override void OnExit()
    {
        base.OnExit();
        if (actorController != null)
            actorController.RigidBody.velocity = Vector2.zero;

        if (actorController.chainAttackAction != null)
        {
            actorController.chainAttackAction.performed -= OnMixComboChain;
            if (!_isComboChained)
                actorController.skillSetIndex = 0;
        }
        actorController.chainAttackAction = null;
    }

    public override void OnStop()
    {
        base.OnStop();
        if (actorController != null)
            actorController.RigidBody.velocity = Vector2.zero;

        if (actorController.chainAttackAction != null)
        {
            actorController.chainAttackAction.performed -= OnMixComboChain;
            if (!_isComboChained)
                actorController.skillSetIndex = 0;
        }
        actorController.chainAttackAction = null;
    }
}
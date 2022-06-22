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

    public override void OnEnter()
    {
        base.OnEnter();
        isLock = true;
        actorController.canTakeDamage = false;
        _timeSinceInit = 0f;
        isInitDir = false;

        actorController.animator.Play(dodgeAnim);
        _direction = actorController.transform.forward;
        actorController.RigidBody.velocity = _direction * distance / duration;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        _timeSinceInit += Time.deltaTime;

        if (_timeSinceInit > iframe)
        {
            actorController.canTakeDamage = true;
            if (_timeSinceInit > duration)
            {
                isLock = false;
                actorController.ChangeToIdle();
                return;
            }
        }
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();

        if (actorController.chainAttackAction != null && actorController.chainAttackAction.IsPressed() && _timeSinceInit > iframe)
        {
            // for combo chain mix dodge
            actorController.GoToNextState(true);
            actorController.chainAttackAction = null;
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
        actorController.RigidBody.velocity = Vector2.zero;
    }
}
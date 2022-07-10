using UnityEngine;
using UnityEngine.InputSystem;
using Sirenix.OdinInspector;

public class LukeShieldSkill : BaseAttackState
{
    [SerializeField] private Shield shield;
    [SerializeField] private float parryWindomTime;

    private bool _isParry;

    public override void OnEnter()
    {
        base.OnEnter();

        _isParry = false;

        shield.gameObject.SetActive(true);
    }

    public override void OnFixedUpdate()
    {
        if (!_ctx.action.IsPressed())
        {
            if (_isParry)
                actorController.ChainNextCombo(_ctx);
            else
                actorController.ChangeToIdle();
        }
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        if (shield.blockedCount > 0 && _timeSinceInit <= parryWindomTime)
            _isParry = true;
    }

    public override void OnExit()
    {
        base.OnExit();

        shield.gameObject.SetActive(false);
    }
}
using UnityEngine;
using UnityEngine.InputSystem;
using Sirenix.OdinInspector;

public class LukeSuperSkill : BaseAttackState
{
    public override void OnEnter()
    {
        base.OnEnter();
        (actorController as LukeController).ToggleInvulnerable(true);
    }

    public override void OnFixedUpdate()
    {
    }

    public override void OnExit()
    {
        base.OnExit();
        (actorController as LukeController).ActivateSuper();
    }
}
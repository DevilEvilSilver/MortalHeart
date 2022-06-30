using UnityEngine;
using UnityEngine.InputSystem;
using Sirenix.OdinInspector;

public class BaseEnemyAttackState : BaseEnemyState
{
    [ValueDropdown("AllAnimations")]
    public string attackAnim;
    public float overrideIdleTime;

    protected float _timeSinceInit;

    public override void OnEnter()
    {
        base.OnEnter();
        isLock = true;
        _timeSinceInit = 0f;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        _timeSinceInit += Time.deltaTime;
    }

    public override void OnExit()
    {
        base.OnExit();
    }
}
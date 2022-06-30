using Sirenix.OdinInspector;

public class BaseEnemyIdleState : BaseEnemyState
{
    [ValueDropdown("AllAnimations")]
    public string idleAnim;

    public override void OnEnter()
    {
        base.OnEnter();
        actorController.animator.Play(idleAnim);
    }
}
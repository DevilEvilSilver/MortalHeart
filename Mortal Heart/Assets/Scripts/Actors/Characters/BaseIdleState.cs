using Sirenix.OdinInspector;

public class BaseIdleState : BaseCharacterState
{
    [ValueDropdown("AllAnimations")]
    public string idleAnim;

    public override void OnEnter()
    {
        base.OnEnter();
        actorController.animator.CrossFadeInFixedTime(idleAnim, 0.2f);
    }
}
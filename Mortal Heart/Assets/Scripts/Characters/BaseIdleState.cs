using Sirenix.OdinInspector;

public class BaseIdleState : BaseActorState
{
    [ValueDropdown("AllAnimations")]
    public string idleAnim;

    public override void OnEnter()
    {
        base.OnEnter();
        actorController.animator.Play(idleAnim);
    }
}
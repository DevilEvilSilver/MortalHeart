using Sirenix.OdinInspector;
using UnityEngine.InputSystem;

public abstract class BaseCharacterState : BaseState
{
#if UNITY_EDITOR
    [ReadOnly]
    public MainCharacterController actorControllerForEditor;

    protected string[] AllAnimations
    {
        get
        {
            return actorControllerForEditor.allAnimations;
        }
    }

#endif

    protected MainCharacterController actorController;

    public virtual void OnInit(MainCharacterController controller)
    {
        actorController = controller;
    }
}